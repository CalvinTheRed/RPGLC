using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

public class MaximizeDamage : Function {

    public MaximizeDamage() : base("maximize_damage") {

    }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is DamageRoll damageRoll) {
            damageRoll.MaximizeDamageDice(functionJson.GetString("damage_type") ?? "");
        } else if (subevent is DamageDelivery damageDelivery) {
            damageDelivery.MaximizeDamageDice(functionJson.GetString("damage_type") ?? "");
        }
    }

};
