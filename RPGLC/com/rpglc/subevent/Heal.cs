using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

public class Heal : Subevent {

    public Heal() : base("heal") { }

    public override Subevent Clone() {
        Subevent clone = new Heal();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new Heal();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Heal? Invoke(RPGLContext context, JsonArray originPoint) {
        return (Heal?) base.Invoke(context, originPoint);
    }

    public override Heal JoinSubeventData(JsonObject other) {
        return (Heal) base.JoinSubeventData(other);
    }

    public override Heal Prepare(RPGLContext context, JsonArray originPoint) {
        GetBaseHealing(context, originPoint);
        return this;
    }

    public override Heal Run(RPGLContext context, JsonArray originPoint) {
        GetTargetHealing(context, originPoint);
        DeliverHealing(context, originPoint);
        return this;
    }

    public override Heal SetOriginItem(string? originItem) {
        return (Heal) base.SetOriginItem(originItem);
    }

    public override Heal SetSource(RPGLObject source) {
        return (Heal) base.SetSource(source);
    }

    public override Heal SetTarget(RPGLObject target) {
        return (Heal) base.SetTarget(target);
    }

    private void GetBaseHealing(RPGLContext context, JsonArray originPoint) {
        RPGLObject source = GetSource();

        HealingCollection baseHealingCollection = new HealingCollection()
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "healing": {{json.GetJsonArray("healing")}},
                    "tags": {{GetTags().DeepClone().AddString("base_healing_collection")}}
                }
                """))
            .SetOriginItem(GetOriginItem())
            .SetSource(source)
            .Prepare(context, originPoint)
            .SetTarget(source)
            .Invoke(context, originPoint);

        HealingRoll baseHealingRoll = new HealingRoll()
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "healing": {{baseHealingCollection.GetHealingCollection()}},
                    "tags": {{GetTags().DeepClone().AddString("base_healing_roll")}}
                }
                """))
            .SetOriginItem(GetOriginItem())
            .SetSource(source)
            .Prepare(context, originPoint)
            .SetTarget(source)
            .Invoke(context, originPoint);

        json.PutJsonArray("healing", baseHealingRoll.GetHealing());
    }

    private void GetTargetHealing(RPGLContext context, JsonArray originPoint) {
        RPGLObject source = GetSource();
        RPGLObject target = GetTarget();

        HealingCollection targetHealingCollection = new HealingCollection()
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "tags": {{GetTags().DeepClone().AddString("target_healing_collection")}}
                }
                """))
            .SetOriginItem(GetOriginItem())
            .SetSource(source)
            .Prepare(context, originPoint)
            .SetTarget(target)
            .Invoke(context, originPoint);

        HealingRoll targetHealingRoll = new HealingRoll()
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "healing": {{targetHealingCollection.GetHealingCollection()}},
                    "tags": {{GetTags().DeepClone().AddString("target_healing_roll")}}
                }
                """))
            .SetOriginItem(GetOriginItem())
            .SetSource(source)
            .Prepare(context, originPoint)
            .SetTarget(target)
            .Invoke(context, originPoint);

        json.GetJsonArray("healing").AsList().AddRange(targetHealingRoll.GetHealing().AsList());
    }

    private void DeliverHealing(RPGLContext context, JsonArray originPoint) {
        RPGLObject target = GetTarget();

        HealingDelivery healingDelivery = new HealingDelivery()
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "healing": {{json.GetJsonArray("healing")}},
                    "tags": {{json.GetJsonArray("tags")}}
                }
                """))
            .SetOriginItem(GetOriginItem())
            .SetSource(GetSource())
            .Prepare(context, originPoint)
            .SetTarget(target)
            .Invoke(context, originPoint);

        target.ReceiveHealing(healingDelivery, context);
    }

};
