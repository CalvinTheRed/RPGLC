using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

public class DamageAffinity : Subevent, IDamageTypeSubevent {

    public DamageAffinity() : base("damage_affinity") {

    }

    public override Subevent Clone() {
        Subevent clone = new DamageAffinity();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new DamageAffinity();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override DamageAffinity? Invoke(RPGLContext context, JsonArray originPoint) {
        return (DamageAffinity?) base.Invoke(context, originPoint);
    }

    public override DamageAffinity JoinSubeventData(JsonObject other) {
        return (DamageAffinity) base.JoinSubeventData(other);
    }

    public override DamageAffinity Prepare(RPGLContext context, JsonArray originPoint) {
        json.PutIfAbsent("affinities", new JsonArray());
        return this;
    }

    public override DamageAffinity Run(RPGLContext context, JsonArray originPoint) {
        return this;
    }

    public override DamageAffinity SetOriginItem(string? originItem) {
        return (DamageAffinity) base.SetOriginItem(originItem);
    }

    public override DamageAffinity SetSource(RPGLObject source) {
        return (DamageAffinity) base.SetSource(source);
    }

    public override DamageAffinity SetTarget(RPGLObject target) {
        return (DamageAffinity) base.SetTarget(target);
    }

    public bool IncludesDamageType(string damageType) {
        JsonArray affinities = GetAffinities();
        for (int i = 0; i < affinities.Count(); i++) {
            if (Equals(damageType, affinities.GetJsonObject(i).GetString("damage_type"))) {
                return true;
            }
        }
        return false;
    }

    public DamageAffinity AddDamageType(string damageType) {
        json.PutIfAbsent("affinities", new JsonArray());
        JsonArray affinities = GetAffinities();
        for (int i = 0; i < affinities.Count(); i++) {
            JsonObject affinity = affinities.GetJsonObject(i);
            if (Equals(damageType, affinity.GetString("damage_type"))) {
                // Short-circuit if damage type is already included
                return this;
            }
        }

        // Add new damage type to affinities array
        GetAffinities().AddJsonObject(new JsonObject()
                .PutString("damage_type", damageType)
                .PutBool("immunity", false)
                .PutBool("resistance", false)
                .PutBool("vulnerability", false)
                .PutBool("immunity_revoked", false)
                .PutBool("resistance_revoked", false)
                .PutBool("vulnerability_revoked", false)
            );

        return this;
    }

    public JsonArray GetAffinities() {
        return json.GetJsonArray("affinities");
    }

    public DamageAffinity GrantImmunity(string damageType) {
        JsonArray affinities = GetAffinities();
        for (int i = 0; i < affinities.Count(); i++) {
            JsonObject affinity = affinities.GetJsonObject(i);
            if (Equals(damageType, "") || Equals(damageType, affinity.GetString("damage_type"))) {
                affinity.PutBool("immunity", true);
            }
        }
        return this;
    }

    public DamageAffinity GrantResistance(string damageType) {
        JsonArray affinities = GetAffinities();
        for (int i = 0; i < affinities.Count(); i++) {
            JsonObject affinity = affinities.GetJsonObject(i);
            if (Equals(damageType, "") || Equals(damageType, affinity.GetString("damage_type"))) {
                affinity.PutBool("resistance", true);
            }
        }
        return this;
    }

    public DamageAffinity GrantVulnerability(string damageType) {
        JsonArray affinities = GetAffinities();
        for (int i = 0; i < affinities.Count(); i++) {
            JsonObject affinity = affinities.GetJsonObject(i);
            if (Equals(damageType, "") || Equals(damageType, affinity.GetString("damage_type"))) {
                affinity.PutBool("vulnerability", true);
            }
        }
        return this;
    }

    public DamageAffinity RevokeImmunity(string damageType) {
        JsonArray affinities = GetAffinities();
        for (int i = 0; i < affinities.Count(); i++) {
            JsonObject affinity = affinities.GetJsonObject(i);
            if (Equals(damageType, "") || Equals(damageType, affinity.GetString("damage_type"))) {
                affinity.PutBool("immunity_revoked", true);
            }
        }
        return this;
    }

    public DamageAffinity RevokeResistance(string damageType) {
        JsonArray affinities = GetAffinities();
        for (int i = 0; i < affinities.Count(); i++) {
            JsonObject affinity = affinities.GetJsonObject(i);
            if (Equals(damageType, "") || Equals(damageType, affinity.GetString("damage_type"))) {
                affinity.PutBool("resistance_revoked", true);
            }
        }
        return this;
    }

    public DamageAffinity RevokeVulnerability(string damageType) {
        JsonArray affinities = GetAffinities();
        for (int i = 0; i < affinities.Count(); i++) {
            JsonObject affinity = affinities.GetJsonObject(i);
            if (Equals(damageType, "") || Equals(damageType, affinity.GetString("damage_type"))) {
                affinity.PutBool("vulnerability_revoked", true);
            }
        }
        return this;
    }

    public bool IsImmune(string damageType) {
        JsonArray affinities = GetAffinities();
        for (int i = 0; i < affinities.Count(); i++) {
            JsonObject affinity = affinities.GetJsonObject(i);
            if (Equals(damageType, affinity.GetString("damage_type"))) {
                return (bool) affinity.GetBool("immunity") && !(bool) affinity.GetBool("immunity_revoked");
            }
        }
        return false;
    }

    public bool IsResistant(string damageType) {
        JsonArray affinities = GetAffinities();
        for (int i = 0; i < affinities.Count(); i++) {
            JsonObject affinity = affinities.GetJsonObject(i);
            if (Equals(damageType, affinity.GetString("damage_type"))) {
                return (bool) affinity.GetBool("resistance") && !(bool) affinity.GetBool("resistance_revoked");
            }
        }
        return false;
    }

    public bool IsVulnerable(string damageType) {
        JsonArray affinities = GetAffinities();
        for (int i = 0; i < affinities.Count(); i++) {
            JsonObject affinity = affinities.GetJsonObject(i);
            if (Equals(damageType, affinity.GetString("damage_type"))) {
                return (bool) affinity.GetBool("vulnerability") && !(bool) affinity.GetBool("vulnerability_revoked");
            }
        }
        return false;
    }

};
