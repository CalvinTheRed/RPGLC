using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

public class GrantImmunity : Function {

    public GrantImmunity() : base("grant_immunity") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is DamageAffinity damageAffinity) {
            damageAffinity.GrantImmunity(functionJson.GetString("damage_type") ?? "");
        }
    }

};
