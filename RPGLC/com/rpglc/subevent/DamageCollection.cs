﻿using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

/// <summary>
///   Collects damage for a damaging subevent.
///   
///   <br /><br />
///   <i>This subevent is unavailable to be used directly inside an RPGLEvent.</i>
///   
///   <br /><br />
///   <i>Note that all damaging subevents will create two DamageCollection subevents. The first will have the "base" tag, and will represent damage that is applied to all targets of the damaging subevent. The second will have the "target" tag, and will represent damage that is only applied to a specific target of the damaging subevent.</i>
///   
///   <br /><br />
///   <b>Special Conditions</b>
///   <list type="bullet">
///     <item>IncludesDamageType</item>
///   </list>
///   
///   <b>Special Functions</b>
///   <list type="bullet">
///     <item>AddDamage</item>
///     <item>RepeatDamageDice</item>
///   </list>
///   
/// </summary>
public class DamageCollection : Subevent, IDamageTypeSubevent {

    public DamageCollection() : base("damage_collection") { }

    public override Subevent Clone() {
        Subevent clone = new DamageCollection();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new DamageCollection();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override DamageCollection? Invoke(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return (DamageCollection?) base.Invoke(context, originPoint, invokingEffect);
    }

    public override DamageCollection JoinSubeventData(JsonObject other) {
        return (DamageCollection) base.JoinSubeventData(other);
    }

    public override DamageCollection Prepare(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        json.PutIfAbsent("damage", new JsonArray());
        PrepareDamage(context);
        return this;
    }

    public override Subevent Run(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return this;
    }

    public override DamageCollection SetOriginItem(string? originItem) {
        return (DamageCollection) base.SetOriginItem(originItem);
    }

    public override DamageCollection SetSource(RPGLObject source) {
        return (DamageCollection) base.SetSource(source);
    }

    public override DamageCollection SetTarget(RPGLObject target) {
        return (DamageCollection) base.SetTarget(target);
    }

    public bool IncludesDamageType(string damageType) {
        JsonArray damageDiceArray = GetDamageCollection();
        for (int i = 0; i < damageDiceArray.Count(); i++) {
            if (Equals(damageDiceArray.GetJsonObject(i).GetString("damage_type"), damageType)) {
                return true;
            }
        }
        return false;
    }

    private void PrepareDamage(RPGLContext context) {
        JsonArray damageArray = json.RemoveJsonArray("damage");
        json.PutJsonArray("damage", new());

        RPGLEffect rpglEffect = new();
        rpglEffect.SetSource(GetSource().GetUuid());
        rpglEffect.SetTarget(null);
        for (int i = 0; i < damageArray.Count(); i++) {
            JsonObject damageJson = damageArray.GetJsonObject(i);
            JsonObject damage = CalculationSubevent.SimplifyCalculationFormula(rpglEffect, this, damageJson, context);
            damage.PutString("damage_type", damageJson.GetString("damage_type"));
            AddDamage(damage);
        }
    }

    public DamageCollection AddDamage(JsonObject damageJson) {
        GetDamageCollection().AddJsonObject(damageJson);
        return this;
    }

    public JsonArray GetDamageCollection() {
        return json.GetJsonArray("damage");
    }

};
