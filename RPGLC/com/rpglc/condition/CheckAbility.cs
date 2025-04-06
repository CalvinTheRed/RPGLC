using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.condition;

/// <summary>
///   Returns true if the subevent's ability matches the condition's ability.
///   
///   <code>
///   {
///     "condition": "check_ability",
///     "ability": &lt;string&gt;
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"ability" indicates which ability the subevent is expected to use.</item>
///   </list>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>AbilityCheck</item>
///     <item>AbilitySave</item>
///     <item>AttackRoll</item>
///     <item>CalculateAbilityScore</item>
///     <item>SavingThrow</item>
///   </list>
///   
/// </summary>
public class CheckAbility : Condition {

    public CheckAbility() : base("check_ability") { }

    public override bool Run(RPGLEffect rpglEffect, Subevent subevent, JsonObject conditionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is IAbilitySubevent abilitySubevent) {
            return Equals(abilitySubevent.GetAbility(context), conditionJson.GetString("ability"));
        }
        return false;
    }

};
