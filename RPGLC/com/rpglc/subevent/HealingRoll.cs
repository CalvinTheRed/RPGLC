using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.math;

namespace com.rpglc.subevent;

public class HealingRoll : Subevent {

    public HealingRoll() : base("healing_roll") {

    }

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
        return (HealingRoll) base.Invoke(context, originPoint);
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

    public HealingRoll RerollHealingDice(long upperBound, long lowerBound) {
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

    public HealingRoll SetHealingDice(long set, long upperBound, long lowerBound) {
        JsonArray healingArray = json.GetJsonArray("healing");
        for (int i = 0; i < healingArray.Count(); i++) {
            JsonArray dice = healingArray.GetJsonObject(i).GetJsonArray("dice");
            for (int j = 0; j < dice.Count(); j++) {
                JsonObject die = dice.GetJsonObject(j);
                long roll = (long) die.GetLong("roll");
                if (roll <= upperBound && roll >= lowerBound) {
                    die.PutLong("roll", set);
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
