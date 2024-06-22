using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.condition;

public class IncludesDamageType : Condition {

    public IncludesDamageType() : base("includes_damage_type") { }

    public override bool Run(RPGLEffect rpglEffect, Subevent subevent, JsonObject conditionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is IDamageTypeSubevent damageTypeSubevent) {
            return damageTypeSubevent.IncludesDamageType(conditionJson.GetString("damage_type"));
        }
        return false;
    }

};
