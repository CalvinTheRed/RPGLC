using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Grants half proficiency to a roll subevent.
///   
///   <code>
///   {
///     "function": "grant_roll_half_proficiency"
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
public class GrantRollHalfProficiency: Function {

    public GrantRollHalfProficiency() : base("grant_roll_half_proficiency") {
        functionSteps.AddRange([
            (rpglEffect, subevent, functionJson, context) => {
                if (subevent is RollSubevent rollSubevent) {
                    rollSubevent.GrantHalfProficiency();
                }
                return new() {
                    dependency = null,
                    stepCompleted = true,
                };
            },
        ]);
    }

    public override GrantRollHalfProficiency Clone() {
        return new();
    }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is AbilityCheck abilityCheck) {
            abilityCheck.GrantHalfProficiency();
        }
    }

};
