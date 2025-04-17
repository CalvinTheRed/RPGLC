using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

/// <summary>
///   Deals damage directly with no contest to reduce or avoid that damage.
///   
///   <code>
///   {
///     "subevent": "deal_damage",
///     "tags": [
///       &lt;string&gt;
///     ],
///     "damage": [
///       &lt;bonus formula&gt;
///     ],
///     "vampirism": [
///       &lt;vampirism formula&gt;
///     ]
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"tags" is an optional field and will default to a value of [ ] if left unspecified. Any tags provided will be inherited by any nested subevents.</item>
///     <item>"damage" is an optional field and it will default to a value of [ ] if left unspecified. This field indicates how much damage will be delivered, if any.</item>
///     <item>"vampirism" is an optional field and it will default to a value of [ ] if left unspecified. This field indicates whether and to what extent the damage dealt by this subevent restores hit points to the source.</item>
///   </list>
///   
/// </summary>
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

    public override DealDamage? Invoke(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return (DealDamage?) base.Invoke(context, originPoint, invokingEffect);
    }

    public override DealDamage JoinSubeventData(JsonObject other) {
        return (DealDamage) base.JoinSubeventData(other);
    }

    public override DealDamage Prepare(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        GetBaseDamage(context, originPoint, invokingEffect);
        return this;
    }

    public override DealDamage Run(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
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

    private void GetBaseDamage(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
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
            .Prepare(context, originPoint, invokingEffect)
            .SetTarget(source)
            .Invoke(context, originPoint, invokingEffect);

        DamageRoll baseDamageRoll = new DamageRoll()
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "damage": {{baseDamageCollection.GetDamageCollection()}},
                    "tags": {{GetTags().DeepClone().AddString("base_damage_roll")}}
                }
                """))
            .SetOriginItem(GetOriginItem())
            .SetSource(source)
            .Prepare(context, originPoint, invokingEffect)
            .SetTarget(source)
            .Invoke(context, originPoint, invokingEffect);

        json.PutJsonArray("damage", baseDamageRoll.GetDamage());
    }

    private void GetTargetDamage(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
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
            .Prepare(context, originPoint, invokingEffect)
            .SetTarget(target)
            .Invoke(context, originPoint, invokingEffect);

        DamageRoll targetDamageRoll = new DamageRoll()
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "damage": {{targetDamageCollection.GetDamageCollection()}},
                    "tags": {{GetTags().DeepClone().AddString("target_damage_roll")}}
                }
                """))
            .SetOriginItem(GetOriginItem())
            .SetSource(source)
            .Prepare(context, originPoint, invokingEffect)
            .SetTarget(target)
            .Invoke(context, originPoint, invokingEffect);

        json.GetJsonArray("damage").AsList().AddRange(targetDamageRoll.GetDamage().AsList());
    }

    private void DeliverDamage(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        DamageDelivery damageDelivery = new DamageDelivery()
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "damage": {{json.GetJsonArray("damage")}},
                    "tags": {{GetTags()}}
                }
                """))
            .SetOriginItem(GetOriginItem())
            .SetSource(GetSource())
            .Prepare(context, originPoint, invokingEffect)
            .SetTarget(GetTarget())
            .Invoke(context, originPoint, invokingEffect);

        JsonObject damageByType = damageDelivery.GetDamage();
        if (json.AsDict().ContainsKey("vampirism")) {
            // TODO excess damage shouldn't contribute to vampirism... should this be handled as a part of DamageDelivery?
            IVampiricSubevent.HandleVampirism(this, damageByType, context, originPoint);
        }
    }

};
