using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

/// <summary>
///   Delivers calculated damage to an object.
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
///     <item>MaximizeDamage</item>
///   </list>
///   
/// </summary>
public class DamageDelivery : Subevent, IDamageTypeSubevent {

    public DamageDelivery() : base("damage_delivery") { }

    public override Subevent Clone() {
        Subevent clone = new DamageDelivery();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new DamageDelivery();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override DamageDelivery? Invoke(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return (DamageDelivery?) base.Invoke(context, originPoint, invokingEffect);
    }

    public override DamageDelivery JoinSubeventData(JsonObject other) {
        return (DamageDelivery) base.JoinSubeventData(other);
    }

    public override DamageDelivery Prepare(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        json.PutIfAbsent("damage", new JsonArray());
        json.PutIfAbsent("damage_proportion", "all");
        return this;
    }

    public override DamageDelivery Run(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        // Apply damage affinities
        CalculateRawDamage();
        JsonObject damageJson = json.RemoveJsonObject("damage");

        DamageAffinity damageAffinity = new DamageAffinity()
            .JoinSubeventData(new JsonObject()
                .PutJsonArray("tags", GetTags().DeepClone())
            )
            .SetOriginItem(GetOriginItem())
            .SetSource(GetSource())
            .Prepare(context, originPoint);

        foreach (string key in damageJson.AsDict().Keys) {
            damageAffinity.AddDamageType(key);
        }
        
        damageAffinity
            .SetTarget(GetTarget())
            .Invoke(context, originPoint, invokingEffect);

        JsonObject damageWithAffinity = new();
        foreach (string key in damageJson.AsDict().Keys) {
            if (!damageAffinity.IsImmune(key)) {
                long typedDamage = (long) damageJson.GetLong(key);
                if (damageAffinity.IsResistant(key)) {
                    typedDamage /= 2;
                }
                if (damageAffinity.IsVulnerable(key)) {
                    typedDamage *= 2;
                }
                damageWithAffinity.PutLong(key, typedDamage);
            }
        }
        json.PutJsonObject("damage", damageWithAffinity);
        GetTarget().ReceiveDamage(this, context); // TODO bypass this if total damage is 0?

        return this;
    }

    public override DamageDelivery SetOriginItem(string? originItem) {
        return (DamageDelivery) base.SetOriginItem(originItem);
    }

    public override DamageDelivery SetSource(RPGLObject source) {
        return (DamageDelivery) base.SetSource(source);
    }

    public override DamageDelivery SetTarget(RPGLObject target) {
        return (DamageDelivery) base.SetTarget(target);
    }

    public bool IncludesDamageType(string damageType) {
        JsonArray damageArray = json.GetJsonArray("damage");
        for (int i = 0; i < damageArray.Count(); i++) {
            if (Equals(damageType, damageArray.GetJsonObject(i).GetString("damage_type"))) {
                return true;
            }
        }
        return false;
    }

    public DamageDelivery MaximizeDamageDice(string damageType) {
        JsonArray damageArray = json.GetJsonArray("damage");
        for (int i = 0; i < damageArray.Count(); i++) {
            JsonObject damageJson = damageArray.GetJsonObject(i);
            if (damageType == "*" || damageType == damageJson.GetString("damage_type")) {
                JsonArray dice = damageJson.GetJsonArray("dice");
                for (int j = 0; j < dice.Count(); j++) {
                    JsonObject die = dice.GetJsonObject(j);
                    die.PutLong("roll", die.GetLong("size"));
                }
            }
        }
        return this;
    }

    private void CalculateRawDamage() {
        string damageProportion = json.GetString("damage_proportion");
        if (!Equals(damageProportion, "none")) {
            JsonObject damage = new();
            JsonArray damageArray = json.RemoveJsonArray("damage");
            for (int i = 0; i < damageArray.Count(); i++) {
                JsonObject damageJson = damageArray.GetJsonObject(i);
                long total = (long) damageJson.GetLong("bonus");
                JsonArray dice = damageJson.GetJsonArray("dice");
                for (int j = 0; j < dice.Count(); j++) {
                    total += (long) dice.GetJsonObject(j).GetLong("roll");
                }
                string damageType = damageJson.GetString("damage_type");
                damage.PutLong(damageType, CalculationSubevent.Scale(
                    (damage.GetLong(damageType) ?? 0L) + total,
                    damageJson.GetJsonObject("scale")
                ));
            }
            if (Equals(damageProportion, "half")) {
                foreach (string key in damage.AsDict().Keys) {
                    damage.PutLong(key, damage.GetLong(key) / 2);
                }
            }
            json.PutJsonObject("damage", damage);
        } else {
            json.PutJsonObject("damage", new());
        }
    }

    public JsonObject GetDamage() {
        return json.GetJsonObject("damage");
    }

};
