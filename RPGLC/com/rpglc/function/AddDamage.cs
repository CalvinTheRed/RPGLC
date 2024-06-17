using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

public class AddDamage : Function {

    public AddDamage() : base("add_damage") {

    }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is DamageCollection damageCollection) {
            JsonArray damageArray = functionJson.GetJsonArray("damage");
            for (int i = 0; i < damageArray.Count(); i++) {
                JsonObject damageElement = damageArray.GetJsonObject(i);
                JsonObject damage = CalculationSubevent.ProcessBonusJson(rpglEffect, subevent, damageElement, context);
                string? damageType = damageElement.GetString("damage_type");
                if (damageType is null) {
                    damage.PutString("damage_type", damageCollection.GetDamageCollection().GetJsonObject(0).GetString("damage_type"));
                } else {
                    damage.PutString("damage_type", damageType);
                }
                damageCollection.AddDamage(damage);
            }
        } else if (subevent is CriticalHitDamageCollection criticalHitDamageCollection) {
            JsonArray damageArray = functionJson.GetJsonArray("damage");
            for (int i = 0; i < damageArray.Count(); i++) {
                JsonObject damageElement = damageArray.GetJsonObject(i);
                JsonObject damage = CalculationSubevent.ProcessBonusJson(rpglEffect, subevent, damageElement, context);
                string? damageType = damageElement.GetString("damage_type");
                if (damageType is null) {
                    damage.PutString("damage_type", criticalHitDamageCollection.GetDamageCollection().GetJsonObject(0).GetString("damage_type"));
                } else {
                    damage.PutString("damage_type", damageType);
                }
                criticalHitDamageCollection.AddDamage(damage);
            }
        }
    }

};
