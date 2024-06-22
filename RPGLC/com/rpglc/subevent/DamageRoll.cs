using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.math;

namespace com.rpglc.subevent;

public class DamageRoll : Subevent, IDamageTypeSubevent {

    public DamageRoll() : base("damage_roll") { }

    public override Subevent Clone() {
        Subevent clone = new DamageRoll();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new DamageRoll();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override DamageRoll? Invoke(RPGLContext context, JsonArray originPoint) {
        return (DamageRoll?) base.Invoke(context, originPoint);
    }

    public override DamageRoll JoinSubeventData(JsonObject other) {
        return (DamageRoll) base.JoinSubeventData(other);
    }

    public override DamageRoll Prepare(RPGLContext context, JsonArray originPoint) {
        json.PutIfAbsent("damage", new JsonArray());
        Roll();
        return this;
    }

    public override DamageRoll Run(RPGLContext context, JsonArray originPoint) {
        return this;
    }

    public override DamageRoll SetOriginItem(string? originItem) {
        return (DamageRoll) base.SetOriginItem(originItem);
    }

    public override DamageRoll SetSource(RPGLObject source) {
        return (DamageRoll) base.SetSource(source);
    }

    public override DamageRoll SetTarget(RPGLObject target) {
        return (DamageRoll) base.SetTarget(target);
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

    private void Roll() {
        JsonArray typedDamageArray = json.GetJsonArray("damage");
        for (int i = 0; i < typedDamageArray.Count(); i++) {
            JsonArray typedDamageDieArray = typedDamageArray.GetJsonObject(i).GetJsonArray("dice") ?? new();
            for (int j = 0; j < typedDamageDieArray.Count(); j++) {
                Die.Roll(typedDamageDieArray.GetJsonObject(j));
            }
        }
    }

    public void RerollDamageDice(string damageType, long lowerBound, long upperBound) {
        JsonArray typedDamageArray = json.GetJsonArray("damage");
        for (int i = 0; i < typedDamageArray.Count(); i++) {
            JsonObject typedDamage = typedDamageArray.GetJsonObject(i);
            if (Equals(damageType, "") || Equals(damageType, typedDamage.GetString("damage_type"))) {
                JsonArray typedDamageDieArray = typedDamage.GetJsonArray("dice") ?? new();
                for (int j = 0; j < typedDamageDieArray.Count(); j++) {
                    JsonObject typedDamageDie = typedDamageDieArray.GetJsonObject(j);
                    long roll = (long) typedDamageDie.GetLong("roll");
                    if (roll <= upperBound && roll >= lowerBound) {
                        Die.Roll(typedDamageDie);
                    }
                }
            }
        }
    }

    public void SetDamageDice(string damageType, long set, long lowerBound, long upperBound) {
        JsonArray typedDamageArray = json.GetJsonArray("damage");
        for (int i = 0; i < typedDamageArray.Count(); i++) {
            JsonObject typedDamage = typedDamageArray.GetJsonObject(i);
            if (Equals(damageType, "") || Equals(damageType, typedDamage.GetString("damage_type"))) {
                JsonArray typedDamageDieArray = typedDamage.GetJsonArray("dice") ?? new();
                for (int j = 0; j < typedDamageDieArray.Count(); j++) {
                    JsonObject typedDamageDie = typedDamageDieArray.GetJsonObject(j);
                    long roll = (long) typedDamageDie.GetLong("roll");
                    if (roll < upperBound && roll > lowerBound) {
                        typedDamageDie.PutLong("roll", set);
                    }
                }
            }
        }
    }

    public void MaximizeDamageDice(string damageType) {
        JsonArray typedDamageArray = json.GetJsonArray("damage");
        for (int i = 0; i < typedDamageArray.Count(); i++) {
            JsonObject typedDamage = typedDamageArray.GetJsonObject(i);
            if (Equals(damageType, "") || Equals(damageType, typedDamage.GetString("damage_type"))) {
                JsonArray typedDamageDieArray = typedDamage.GetJsonArray("dice") ?? new();
                for (int j = 0; j < typedDamageDieArray.Count(); j++) {
                    JsonObject typedDamageDie = typedDamageDieArray.GetJsonObject(j);
                    typedDamageDie.PutLong("roll", typedDamageDie.GetLong("size"));
                }
            }
        }
    }

    public JsonArray GetDamage() {
        return json.GetJsonArray("damage");
    }

};
