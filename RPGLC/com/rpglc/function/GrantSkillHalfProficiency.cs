using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Grants half proficiency to an ability check.
///   
///   <code>
///   {
///     "function": "grant_skill_half_proficiency"
///   }
///   </code>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>AbilityCheck</item>
///   </list>
///   
/// </summary>
public class GrantSkillHalfProficiency: Function {

    public GrantSkillHalfProficiency() : base("grant_skill_half_proficiency") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is AbilityCheck abilityCheck) {
            abilityCheck.GrantHalfProficiency();
        }
    }

};
