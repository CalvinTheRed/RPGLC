using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Grants half proficiency to a roll subevent.
///   
///   <code>
///   {
///     "function": "grant_roll_proficiency"
///   }
///   </code>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>AbilityCheck</item>
///     <item>AbilitySave</item>
///     <item>AttackRoll</item>
///     <item>SavingThrow</item>
///   </list>
///   
/// </summary>
public class GrantRollProficiency: Function {

    // TODO refactor to apply to all RollSubevents

    public GrantRollProficiency() : base("grant_roll_proficiency") {
        functionSteps.AddRange([
            (rpglEffect, subevent, functionJson, context) => {
                if (subevent is RollSubevent rollSubevent) {
                    rollSubevent.GrantProficiency();
                }
                return new() {
                    dependency = null,
                    stepCompleted = true,
                };
            },
        ]);
    }

    public override GrantRollProficiency Clone() {
        return new();
    }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is AbilityCheck abilityCheck) {
            abilityCheck.GrantProficiency();
        }
    }

};
