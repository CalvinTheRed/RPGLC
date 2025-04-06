using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.math;

namespace com.rpglc.subevent;

/// <summary>
///   Rolls dice collected in a TemporaryHitPointCollection subevent.
///   
///   <br /><br />
///   <i>This subevent is unavailable to be used directly inside an RPGLEvent.</i>
///   
///   <br /><br />
///   <b>Special Functions</b>
///   <list type="bullet">
///     <item>MaximizeTemporaryHitPoints</item>
///     <item>OverrideTemporaryHitPointDice</item>
///     <item>RerollTemporaryHitPointDice</item>
///   </list>
///   
/// </summary>
public class TemporaryHitPointRoll : Subevent {

    public TemporaryHitPointRoll() : base("temporary_hit_point_roll") { }

    public override Subevent Clone() {
        Subevent clone = new TemporaryHitPointRoll();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new TemporaryHitPointRoll();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override TemporaryHitPointRoll? Invoke(RPGLContext context, JsonArray originPoint) {
        return (TemporaryHitPointRoll?) base.Invoke(context, originPoint);
    }

    public override TemporaryHitPointRoll JoinSubeventData(JsonObject other) {
        return (TemporaryHitPointRoll) base.JoinSubeventData(other);
    }

    public override TemporaryHitPointRoll Prepare(RPGLContext context, JsonArray originPoint) {
        json.PutIfAbsent("temporary_hit_points", new JsonArray());
        Roll();
        return this;
    }

    public override TemporaryHitPointRoll Run(RPGLContext context, JsonArray originPoint) {
        return this;
    }

    public override TemporaryHitPointRoll SetOriginItem(string? originItem) {
        return (TemporaryHitPointRoll) base.SetOriginItem(originItem);
    }

    public override TemporaryHitPointRoll SetSource(RPGLObject source) {
        return (TemporaryHitPointRoll) base.SetSource(source);
    }

    public override TemporaryHitPointRoll SetTarget(RPGLObject target) {
        return (TemporaryHitPointRoll) base.SetTarget(target);
    }

    private void Roll() {
        JsonArray temporaryHitPointArray = json.GetJsonArray("temporary_hit_points");
        for (int i = 0; i < temporaryHitPointArray.Count(); i++) {
            JsonArray dice = temporaryHitPointArray.GetJsonObject(i).GetJsonArray("dice");
            for (int j = 0; j < dice.Count(); j++) {
                Die.Roll(dice.GetJsonObject(j));
            }
        }
    }

    public TemporaryHitPointRoll RerollTemporaryHitPointDice(long lowerBound, long upperBound) {
        JsonArray temporaryHitPointArray = json.GetJsonArray("temporary_hit_points");
        for (int i = 0; i < temporaryHitPointArray.Count(); i++) {
            JsonArray dice = temporaryHitPointArray.GetJsonObject(i).GetJsonArray("dice");
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

    public TemporaryHitPointRoll OverrideTemporaryHitPointDice(RPGLEffect rpglEffect, JsonObject functionJson, RPGLContext context) {
        long upperBound = functionJson.GetLong("upper_bound") ?? long.MaxValue;
        long lowerBound = functionJson.GetLong("lower_bound") ?? 0L;
        JsonArray temporaryHitPointArray = json.GetJsonArray("temporary_hit_points");
        for (int i = 0; i < temporaryHitPointArray.Count(); i++) {
            JsonArray dice = temporaryHitPointArray.GetJsonObject(i).GetJsonArray("dice");
            for (int j = 0; j < dice.Count(); j++) {
                JsonObject die = dice.GetJsonObject(j);
                long roll = (long) die.GetLong("roll");
                if (roll <= upperBound && roll >= lowerBound) {
                    die.PutLong("roll", CalculationSubevent.ProcessFormulaJson(
                        CalculationSubevent.SimplifyCalculationFormulaJson(rpglEffect, this, functionJson.GetJsonObject("override"), context))
                    );
                }
            }
        }
        return this;
    }

    public TemporaryHitPointRoll MaximizeTemporaryHitPointDice() {
        JsonArray temporaryHitPointArray = json.GetJsonArray("temporary_hit_points");
        for (int i = 0; i < temporaryHitPointArray.Count(); i++) {
            JsonArray dice = temporaryHitPointArray.GetJsonObject(i).GetJsonArray("dice");
            for (int j = 0; j < dice.Count(); j++) {
                JsonObject die = dice.GetJsonObject(j);
                die.PutLong("roll", die.GetLong("size"));
            }
        }
        return this;
    }

    public JsonArray GetTemporaryHitPoints() {
        return json.GetJsonArray("temporary_hit_points");
    }

};
