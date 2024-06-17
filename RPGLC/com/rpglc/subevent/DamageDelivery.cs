using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

public class DamageDelivery : Subevent, IDamageTypeSubevent {

    public DamageDelivery() : base("damage_delivery") {

    }

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

    public override DamageDelivery? Invoke(RPGLContext context, JsonArray originPoint) {
        return (DamageDelivery) base.Invoke(context, originPoint);
    }

    public override DamageDelivery JoinSubeventData(JsonObject other) {
        return (DamageDelivery) base.JoinSubeventData(other);
    }

    public override DamageDelivery Prepare(RPGLContext context, JsonArray originPoint) {
        json.PutIfAbsent("damage_proportion", "all");
        return this;
    }

    public override DamageDelivery Run(RPGLContext context, JsonArray originPoint) {
        // Apply damage affinities
        CalculateRawDamage();
        JsonObject damageJson = json.RemoveJsonObject("damage");

        DamageAffinity damageAffinity = new DamageAffinity()
            .JoinSubeventData(new JsonObject()
                .PutJsonArray("tags", GetTags().DeepClone())
            );
        foreach (string key in damageJson.AsDict().Keys) {
            damageAffinity.AddDamageType(key);
        }
        damageAffinity
            .SetSource(GetSource())
            .Prepare(context, originPoint)
            .SetTarget(GetTarget())
            .Invoke(context, originPoint);

        JsonObject damagewithAffinity = new();
        foreach (string key in damageJson.AsDict().Keys) {
            if (!damageAffinity.IsImmune(key)) {
                long typedDamage = (long) damageJson.GetLong(key);
                if (damageAffinity.IsResistant(key)) {
                    typedDamage /= 2;
                }
                if (damageAffinity.IsVulnerable(key)) {
                    typedDamage *= 2;
                }
                damagewithAffinity.PutLong(key, typedDamage);
            }
        }
        json.PutJsonObject("damage", damagewithAffinity);
        GetTarget().ReceiveDamage(this, context);

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
            if (Equals(damageType, "") || Equals(damageType, damageJson.GetString("damage_type"))) {
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
                    total + damage.GetLong(damageType) ?? 0L,
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
