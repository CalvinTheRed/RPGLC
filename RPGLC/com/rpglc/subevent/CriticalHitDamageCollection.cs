using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

/// <summary>
///   Collects damage for a critical hit. By default, this subevent will contain all damage collected by the base DamageCalculation of the parent AttackRoll subevent, but with twice as many dice. Any further damage bonuses will not be doubled.
///   
///   <br /><br />
///   <i>This subevent is unavailable to be used directly inside an RPGLEvent.</i>
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
public class CriticalHitDamageCollection : Subevent, IDamageTypeSubevent {

    public CriticalHitDamageCollection() : base("critical_hit_damage_collection") { }

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

    public override CriticalHitDamageCollection? Invoke(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return (CriticalHitDamageCollection?) base.Invoke(context, originPoint, invokingEffect);
    }

    public override CriticalHitDamageCollection JoinSubeventData(JsonObject other) {
        return (CriticalHitDamageCollection) base.JoinSubeventData(other);
    }

    public override CriticalHitDamageCollection Prepare(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        json.PutIfAbsent("damage", new JsonArray());
        return this;
    }

    public override CriticalHitDamageCollection Run(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
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

};
