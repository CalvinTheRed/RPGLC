using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Grants expertise to an ability check.
///   
///   <code>
///   {
///     "function": "grant_skill_expertise"
///   }
///   </code>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>AbilityCheck</item>
///   </list>
///   
/// </summary>
public class GrantSkillExpertise : Function {

    public GrantSkillExpertise() : base("grant_skill_expertise") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is AbilityCheck abilityCheck) {
            abilityCheck.GrantExpertise();
        }
    }

};
