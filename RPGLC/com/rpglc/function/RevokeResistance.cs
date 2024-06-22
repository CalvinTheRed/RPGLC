using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

public class RevokeResistance : Function {

    public RevokeResistance() : base("revoke_resistance") {

    }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is DamageAffinity damageAffinity) {
            damageAffinity.RevokeResistance(functionJson.GetString("damage_type") ?? "");
        }
    }

};
