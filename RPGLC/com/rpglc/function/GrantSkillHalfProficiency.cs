using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

public class GrantSkillHalfProficiency: Function {

    public GrantSkillHalfProficiency() : base("grant_skill_half_proficiency") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is AbilityCheck abilityCheck) {
            abilityCheck.GrantHalfProficiency();
        }
    }

};
