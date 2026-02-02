using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.condition;

/// <summary>
///   Returns true if the subevent's skill matches the condition's skill.
///   
///   <code>
///   {
///     "condition": "check_level",
///     "skill": &lt;string&gt;
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"skill" indicates which skill the subevent is expected to use.</item>
///   </list>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>AbilitySave</item>
///   </list>
///   
/// </summary>
public class CheckSkill: Condition {

    public CheckSkill() : base("check_skill") {
        conditionSteps.AddRange([
            (rpglEffect, subevent, conditionJson, context) => {
                if (subevent is AbilitySave abilitySave) {
                    evaluation = Equals(conditionJson.GetString("skill"), abilitySave.GetSkill());
                }

                return new() {
                    conditionDependency = null,
                    subeventDependency = null,
                    stepCompleted = true,
                };
            },
        ]);
    }

    public override CheckSkill Clone() {
        return new();
    }

    public override bool Run(RPGLEffect rpglEffect, Subevent subevent, JsonObject conditionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is AbilitySave abilitySave) {
            return Equals(abilitySave.GetSkill(), conditionJson.GetString("skill"));
        }
        return false;
    }

};
