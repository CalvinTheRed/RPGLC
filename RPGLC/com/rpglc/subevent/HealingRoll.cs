using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.math;

namespace com.rpglc.subevent;

/// <summary>
///   Rolls dice collected in a HealingCollection subevent.
///   
///   <br /><br />
///   <i>This subevent is unavailable to be used directly inside an RPGLEvent.</i>
///   
///   <br /><br />
///   <b>Special Functions</b>
///   <list type="bullet">
///     <item>MaximizeHealing</item>
///     <item>OverrideHealingDice</item>
///     <item>RerollHealingDice</item>
///   </list>
///   
/// </summary>
public class HealingRoll : Subevent {

    public HealingRoll() : base("healing_roll") { }

    public override Subevent Clone() {
        Subevent clone = new HealingRoll();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new HealingRoll();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override HealingRoll? Invoke(RPGLContext context, JsonArray originPoint) {
        return (HealingRoll?) base.Invoke(context, originPoint);
    }

    public override HealingRoll JoinSubeventData(JsonObject other) {
        return (HealingRoll) base.JoinSubeventData(other);
    }

    public override HealingRoll Prepare(RPGLContext context, JsonArray originPoint) {
        json.PutIfAbsent("healing", new JsonArray());
        Roll();
        return this;
    }

    public override HealingRoll Run(RPGLContext context, JsonArray originPoint) {
        return this;
    }

    public override HealingRoll SetOriginItem(string? originItem) {
        return (HealingRoll) base.SetOriginItem(originItem);
    }

    public override HealingRoll SetSource(RPGLObject source) {
        return (HealingRoll) base.SetSource(source);
    }

    public override HealingRoll SetTarget(RPGLObject target) {
        return (HealingRoll) base.SetTarget(target);
    }

    private void Roll() {
        JsonArray healingArray = json.GetJsonArray("healing");
        for (int i = 0; i < healingArray.Count(); i++) {
            JsonArray dice = healingArray.GetJsonObject(i).GetJsonArray("dice");
            for (int j = 0; j < dice.Count(); j++) {
                Die.Roll(dice.GetJsonObject(j));
            }
        }
    }

    public HealingRoll RerollHealingDice(long lowerBound, long upperBound) {
        JsonArray healingArray = json.GetJsonArray("healing");
        for (int i = 0; i < healingArray.Count(); i++) {
            JsonArray dice = healingArray.GetJsonObject(i).GetJsonArray("dice");
            for (int j = 0; j < dice.Count(); j++) {
                JsonObject die = dice.GetJsonObject(j);
                long roll = (long) die.GetLong("roll");
                if (roll <= upperBound && roll >= lowerBound) {
                    Die.Roll(die);
                }
            }
        }
        return this;
    }

    public HealingRoll OverrideHealingDice(RPGLEffect rpglEffect, JsonObject functionJson, RPGLContext context) {
        long upperBound = functionJson.GetLong("upper_bound") ?? long.MaxValue;
        long lowerBound = functionJson.GetLong("lower_bound") ?? 0L;
        JsonArray healingArray = json.GetJsonArray("healing");
        for (int i = 0; i < healingArray.Count(); i++) {
            JsonArray dice = healingArray.GetJsonObject(i).GetJsonArray("dice");
            for (int j = 0; j < dice.Count(); j++) {
                JsonObject die = dice.GetJsonObject(j);
                long roll = (long) die.GetLong("roll");
                if (roll <= upperBound && roll >= lowerBound) {
                    die.PutLong("roll", CalculationSubevent.ProcessFormulaJson(
                        CalculationSubevent.SimplifyCalculationFormula(rpglEffect, this, functionJson.GetJsonObject("override"), context))
                    );
                }
            }
        }
        return this;
    }

    public HealingRoll MaximizeHealingDice() {
        JsonArray healingArray = json.GetJsonArray("healing");
        for (int i = 0; i < healingArray.Count(); i++) {
            JsonArray dice = healingArray.GetJsonObject(i).GetJsonArray("dice");
            for (int j = 0; j < dice.Count(); j++) {
                JsonObject die = dice.GetJsonObject(j);
                die.PutLong("roll", die.GetLong("size"));
            }
        }
        return this;
    }

    public JsonArray GetHealing() {
        return json.GetJsonArray("healing");
    }

};
