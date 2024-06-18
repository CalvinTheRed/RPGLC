using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

public class HealingCollection : Subevent {

    public HealingCollection() : base("healing_collection") {

    }

    public override Subevent Clone() {
        Subevent clone = new HealingCollection();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new HealingCollection();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override HealingCollection? Invoke(RPGLContext context, JsonArray originPoint) {
        return (HealingCollection?) base.Invoke(context, originPoint);
    }

    public override HealingCollection JoinSubeventData(JsonObject other) {
        return (HealingCollection) base.JoinSubeventData(other);
    }

    public override HealingCollection Prepare(RPGLContext context, JsonArray originPoint) {
        json.PutIfAbsent("healing", new JsonArray());
        PrepareHealing(context);
        return this;
    }

    public override HealingCollection Run(RPGLContext context, JsonArray originPoint) {
        return this;
    }

    public override HealingCollection SetOriginItem(string? originItem) {
        return (HealingCollection) base.SetOriginItem(originItem);
    }

    public override HealingCollection SetSource(RPGLObject source) {
        return (HealingCollection) base.SetSource(source);
    }

    public override HealingCollection SetTarget(RPGLObject target) {
        return (HealingCollection) base.SetTarget(target);
    }

    private void PrepareHealing(RPGLContext context) {
        JsonArray healingArray = json.RemoveJsonArray("healing");
        json.PutJsonArray("healing", new());

        RPGLEffect rpglEffect = new();
        rpglEffect.SetSource(GetSource().GetUuid());
        rpglEffect.SetTarget(null);
        for (int i = 0; i < healingArray.Count(); i++) {
            AddHealing(CalculationSubevent.ProcessBonusJson(
                rpglEffect,
                this,
                healingArray.GetJsonObject(i),
                context
            ));
        }
    }

    public HealingCollection AddHealing(JsonObject healingJson) {
        GetHealingCollection().AddJsonObject(healingJson);
        return this;
    }

    public JsonArray GetHealingCollection() {
        return json.GetJsonArray("healing");
    }

};
