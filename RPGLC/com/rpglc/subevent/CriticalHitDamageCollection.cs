﻿using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

public class CriticalHitDamageCollection : Subevent, IDamageTypeSubevent {

    public CriticalHitDamageCollection() : base("critical_hit_damage_collection") {

    }

    public override Subevent Clone() {
        Subevent clone = new CriticalHitDamageCollection();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new CriticalHitDamageCollection();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override CriticalHitDamageCollection? Invoke(RPGLContext context, json.JsonArray originPoint) {
        return (CriticalHitDamageCollection) base.Invoke(context, originPoint);
    }

    public override CriticalHitDamageCollection JoinSubeventData(json.JsonObject other) {
        return (CriticalHitDamageCollection) base.JoinSubeventData(other);
    }

    public override CriticalHitDamageCollection Prepare(RPGLContext context, json.JsonArray originPoint) {
        json.PutIfAbsent("damage", new JsonArray());
        return this;
    }

    public override CriticalHitDamageCollection Run(RPGLContext context, JsonArray originPoint) {
        return this;
    }

    public override CriticalHitDamageCollection SetOriginItem(string? originItem) {
        return (CriticalHitDamageCollection) base.SetOriginItem(originItem);
    }

    public override CriticalHitDamageCollection SetSource(RPGLObject source) {
        return (CriticalHitDamageCollection) base.SetSource(source);
    }

    public override CriticalHitDamageCollection SetTarget(RPGLObject target) {
        return (CriticalHitDamageCollection) base.SetTarget(target);
    }

    public bool IncludesDamageType(string damageType) {
        JsonArray damageDiceArray = json.GetJsonArray("damage");
        for (int i = 0; i < damageDiceArray.Count(); i++) {
            if (Equals(damageDiceArray.GetJsonObject(i).GetString("damage_type"), damageType)) {
                return true;
            }
        }
        return false;
    }

    public void AddDamage(JsonObject damageJson) {
        GetDamageCollection().AddJsonObject(damageJson);
    }

    public JsonArray GetDamageCollection() {
        return json.GetJsonArray("damage");
    }

    internal void DoubleDice() {
        JsonArray damageCollection = GetDamageCollection();
        for (int i = 0; i < damageCollection.Count(); i++) {
            JsonArray typedDamageDice = damageCollection.GetJsonObject(i).GetJsonArray("dice");
            typedDamageDice.AsList().AddRange(typedDamageDice.DeepClone().AsList());
        }
    }

};
