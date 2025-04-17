using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

/// <summary>
///   Restores hit points to an object. This subevent will not raise an object's hit points beyond its hit point maximum.
///   
///   <code>
///   {
///     "subevent": "heal",
///     "tags": [
///       &lt;string&gt;
///     ],
///     "healing": [
///       &lt;bonus formula&gt;
///     ]
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"tags" is an optional field and will default to a value of [ ] if left unspecified. Any tags provided will be inherited by any nested subevents.</item>
///     <item>"healing" is an optional field and it will default to a value of [ ] if left unspecified. This field indicates how many hit points will be restored, if any.</item>
///   </list>
///   
/// </summary>
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

    public override Heal? Invoke(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return (Heal?) base.Invoke(context, originPoint, invokingEffect);
    }

    public override Heal JoinSubeventData(JsonObject other) {
        return (Heal) base.JoinSubeventData(other);
    }

    public override Heal Prepare(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        json.PutIfAbsent("healing", new JsonArray());
        GetBaseHealing(context, originPoint, invokingEffect);
        return this;
    }

    public override Heal Run(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        GetTargetHealing(context, originPoint, invokingEffect);
        DeliverHealing(context, originPoint, invokingEffect);
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

    private void GetBaseHealing(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
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
            .Prepare(context, originPoint, invokingEffect)
            .SetTarget(source)
            .Invoke(context, originPoint, invokingEffect);

        HealingRoll baseHealingRoll = new HealingRoll()
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "healing": {{baseHealingCollection.GetHealingCollection()}},
                    "tags": {{GetTags().DeepClone().AddString("base_healing_roll")}}
                }
                """))
            .SetOriginItem(GetOriginItem())
            .SetSource(source)
            .Prepare(context, originPoint, invokingEffect)
            .SetTarget(source)
            .Invoke(context, originPoint, invokingEffect);

        json.PutJsonArray("healing", baseHealingRoll.GetHealing());
    }

    private void GetTargetHealing(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
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
            .Prepare(context, originPoint, invokingEffect)
            .SetTarget(target)
            .Invoke(context, originPoint, invokingEffect);

        HealingRoll targetHealingRoll = new HealingRoll()
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "healing": {{targetHealingCollection.GetHealingCollection()}},
                    "tags": {{GetTags().DeepClone().AddString("target_healing_roll")}}
                }
                """))
            .SetOriginItem(GetOriginItem())
            .SetSource(source)
            .Prepare(context, originPoint, invokingEffect)
            .SetTarget(target)
            .Invoke(context, originPoint, invokingEffect);

        json.GetJsonArray("healing").AsList().AddRange(targetHealingRoll.GetHealing().AsList());
    }

    private void DeliverHealing(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        RPGLObject target = GetTarget();

        HealingDelivery healingDelivery = new HealingDelivery()
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "healing": {{json.GetJsonArray("healing")}},
                    "tags": {{GetTags()}}
                }
                """))
            .SetOriginItem(GetOriginItem())
            .SetSource(GetSource())
            .Prepare(context, originPoint, invokingEffect)
            .SetTarget(target)
            .Invoke(context, originPoint, invokingEffect);

        target.ReceiveHealing(healingDelivery, context);
    }

};
