using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

public class DamageCollection : Subevent, IDamageTypeSubevent {

    public DamageCollection() : base("damage_collection") {

    }

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

    public override DamageCollection? Invoke(RPGLContext context, JsonArray originPoint) {
        return (DamageCollection) base.Invoke(context, originPoint);
    }

    public override DamageCollection? JoinSubeventData(JsonObject other) {
        return (DamageCollection) base.JoinSubeventData(other);
    }

    public override DamageCollection Prepare(RPGLContext context, JsonArray originPoint) {
        json.PutIfAbsent("damage", new JsonArray());
        PrepareDamage(context);
        return this;
    }

    public override Subevent Run(RPGLContext context, JsonArray originPoint) {
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
        rpglEffect.SetTarget(GetTarget().GetUuid());
        for (int i = 0; i < damageArray.Count(); i++) {
            JsonObject damageElement = damageArray.GetJsonObject(i);
            JsonObject damage = CalculationSubevent.ProcessBonusJson(rpglEffect, this, damageElement, context);
            damage.PutString("damage_type", damageElement.GetString("damage_type"));
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
