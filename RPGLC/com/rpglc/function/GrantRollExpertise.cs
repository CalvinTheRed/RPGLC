using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Grants expertise to a roll subevent.
///   
///   <code>
///   {
///     "function": "grant_roll_expertise"
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
public class GrantRollExpertise : Function {

    public GrantRollExpertise() : base("grant_roll_expertise") {
        functionSteps.AddRange([
            (rpglEffect, subevent, functionJson, context) => {
                if (subevent is RollSubevent rollSubevent) {
                    rollSubevent.GrantExpertise();
                }
                return new() {
                    dependency = null,
                    stepCompleted = true,
                };
            },
        ]);
    }

    public override GrantRollExpertise Clone() {
        return new();
    }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is AbilityCheck abilityCheck) {
            abilityCheck.GrantExpertise();
        }
    }

};
