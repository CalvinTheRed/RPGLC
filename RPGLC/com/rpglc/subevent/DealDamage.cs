using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

public class DealDamage : Subevent, IVampiricSubevent {

    public DealDamage() : base("deal_damage") { }

    public override Subevent Clone() {
        Subevent clone = new DealDamage();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new DealDamage();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override DealDamage? Invoke(RPGLContext context, JsonArray originPoint) {
        return (DealDamage?) base.Invoke(context, originPoint);
    }

    public override DealDamage JoinSubeventData(JsonObject other) {
        return (DealDamage) base.JoinSubeventData(other);
    }

    public override DealDamage Prepare(RPGLContext context, JsonArray originPoint) {
        GetBaseDamage(context, originPoint);
        return this;
    }

    public override DealDamage Run(RPGLContext context, JsonArray originPoint) {
        GetTargetDamage(context, originPoint);
        DeliverDamage(context, originPoint);
        return this;
    }

    public override DealDamage SetOriginItem(string? originItem) {
        return (DealDamage) base.SetOriginItem(originItem);
    }

    public override DealDamage SetSource(RPGLObject source) {
        return (DealDamage) base.SetSource(source);
    }

    public override DealDamage SetTarget(RPGLObject target) {
        return (DealDamage) base.SetTarget(target);
    }

    private void GetBaseDamage(RPGLContext context, JsonArray originPoint) {
        RPGLObject source = GetSource();

        DamageCollection baseDamageCollection = new DamageCollection()
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "damage": {{json.GetJsonArray("damage")}},
                    "tags": {{GetTags().DeepClone().AddString("base_damage_collection")}}
                }
                """))
            .SetOriginItem(GetOriginItem())
            .SetSource(source)
            .Prepare(context, originPoint)
            .SetTarget(source)
            .Invoke(context, originPoint);

        DamageRoll baseDamageRoll = new DamageRoll()
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "damage": {{baseDamageCollection.GetDamageCollection()}},
                    "tags": {{GetTags().DeepClone().AddString("base_damage_roll")}}
                }
                """))
            .SetOriginItem(GetOriginItem())
            .SetSource(source)
            .Prepare(context, originPoint)
            .SetTarget(source)
            .Invoke(context, originPoint);

        json.PutJsonArray("damage", baseDamageRoll.GetDamage());
    }

    private void GetTargetDamage(RPGLContext context, JsonArray originPoint) {
        RPGLObject source = GetSource();
        RPGLObject target = GetTarget();

        DamageCollection targetDamageCollection = new DamageCollection()
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "tags": {{GetTags().DeepClone().AddString("target_damage_collection")}}
                }
                """))
            .SetOriginItem(GetOriginItem())
            .SetSource(source)
            .Prepare(context, originPoint)
            .SetTarget(target)
            .Invoke(context, originPoint);

        DamageRoll targetDamageRoll = new DamageRoll()
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "damage": {{targetDamageCollection.GetDamageCollection()}},
                    "tags": {{GetTags().DeepClone().AddString("target_damage_roll")}}
                }
                """))
            .SetOriginItem(GetOriginItem())
            .SetSource(source)
            .Prepare(context, originPoint)
            .SetTarget(target)
            .Invoke(context, originPoint);

        json.GetJsonArray("damage").AsList().AddRange(targetDamageRoll.GetDamage().AsList());
    }

    private void DeliverDamage(RPGLContext context, JsonArray originPoint) {
        DamageDelivery damageDelivery = new DamageDelivery()
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "damage": {{json.GetJsonArray("damage")}},
                    "tags": {{json.GetJsonArray("tags")}}
                }
                """))
            .SetOriginItem(GetOriginItem())
            .SetSource(GetSource())
            .Prepare(context, originPoint)
            .SetTarget(GetTarget())
            .Invoke(context, originPoint);

        JsonObject damageByType = damageDelivery.GetDamage();
        if (json.AsDict().ContainsKey("vampirism")) {
            // TODO excess damage shouldn't contribute to vampirism... should this be handled as a part of DamageDelivery?
            IVampiricSubevent.HandleVampirism(this, damageByType, context, originPoint);
        }
    }

};
