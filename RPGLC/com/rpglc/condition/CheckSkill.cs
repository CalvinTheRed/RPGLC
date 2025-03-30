using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.condition;

public class CheckSkill: Condition {

    public CheckSkill() : base("check_skill") { }

    public override bool Run(RPGLEffect rpglEffect, Subevent subevent, JsonObject conditionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is AbilityCheck abilityCheck) {
            return Equals(abilityCheck.GetSkill(), conditionJson.GetString("skill"));
        }
        return false;
    }

};
