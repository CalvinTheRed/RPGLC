using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.math;

namespace com.rpglc.subevent;

public class HealingDelivery : Subevent {

    public HealingDelivery() : base("healing_delivery") { }

    public override Subevent Clone() {
        Subevent clone = new HealingDelivery();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new HealingDelivery();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override HealingDelivery? Invoke(RPGLContext context, JsonArray originPoint) {
        return (HealingDelivery?) base.Invoke(context, originPoint);
    }

    public override HealingDelivery JoinSubeventData(JsonObject other) {
        return (HealingDelivery) base.JoinSubeventData(other);
    }

    public override HealingDelivery Prepare(RPGLContext context, JsonArray originPoint) {
        json.PutIfAbsent("healing", new JsonArray());
        return this;
    }

    public override HealingDelivery Run(RPGLContext context, JsonArray originPoint) {
        return this;
    }

    public override HealingDelivery SetOriginItem(string? originItem) {
        return (HealingDelivery) base.SetOriginItem(originItem);
    }

    public override HealingDelivery SetSource(RPGLObject source) {
        return (HealingDelivery) base.SetSource(source);
    }

    public override HealingDelivery SetTarget(RPGLObject target) {
        return (HealingDelivery) base.SetTarget(target);
    }

    public HealingDelivery MaximizeHealingDice() {
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

    public long GetHealing() {
        JsonArray healingArray = json.GetJsonArray("healing");
        long healing = 0L;
        for (int i = 0; i < healingArray.Count(); i++) {
            JsonObject healingJson = healingArray.GetJsonObject(i);
            healing += (long) healingJson.GetLong("bonus");
            JsonArray dice = healingJson.GetJsonArray("dice");
            for (int j = 0; j < dice.Count(); j++) {
                healing += (long) dice.GetJsonObject(j).GetLong("roll");
            }
        }
        return healing;
    }

};
