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
///     <item>AbilityCheck</item>
///   </list>
///   
/// </summary>
public class CheckSkill: Condition {

    public CheckSkill() : base("check_skill") { }

    public override bool Run(RPGLEffect rpglEffect, Subevent subevent, JsonObject conditionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is AbilityCheck abilityCheck) {
            return Equals(abilityCheck.GetSkill(), conditionJson.GetString("skill"));
        }
        return false;
    }

};
