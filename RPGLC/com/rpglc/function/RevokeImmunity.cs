using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

public class RevokeImmunity : Function {

    public RevokeImmunity() : base("revoke_immunity") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is DamageAffinity damageAffinity) {
            damageAffinity.RevokeImmunity(functionJson.GetString("damage_type") ?? "");
        }
    }

};
