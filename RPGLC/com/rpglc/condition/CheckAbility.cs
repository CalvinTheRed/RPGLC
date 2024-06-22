using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.condition;

public class CheckAbility : Condition {

    public CheckAbility() : base("check_ability") { }

    public override bool Run(RPGLEffect rpglEffect, Subevent subevent, JsonObject conditionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is IAbilitySubevent abilitySubevent) {
            return Equals(abilitySubevent.GetAbility(context), conditionJson.GetString("ability"));
        }
        return false;
    }

};
