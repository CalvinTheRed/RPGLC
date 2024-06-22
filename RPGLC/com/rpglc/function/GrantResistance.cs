using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

public class GrantResistance : Function {

    public GrantResistance() : base("grant_resistance") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is DamageAffinity damageAffinity) {
            damageAffinity.GrantResistance(functionJson.GetString("damage_type") ?? "");
        }
    }

};
