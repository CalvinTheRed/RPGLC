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

    public GrantSkillExpertise() : base("grant_skill_expertise") {
        functionSteps.AddRange([
            (rpglEffect, subevent, functionJson, context) => {
                if (subevent is AbilityCheck abilityCheck) {
                    abilityCheck.GrantExpertise();
                }
                return new() {
                    dependency = null,
                    stepCompleted = true,
                };
            },
        ]);
    }

    public override GrantSkillExpertise Clone() {
        return new();
    }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is AbilityCheck abilityCheck) {
            abilityCheck.GrantExpertise();
        }
    }

};
