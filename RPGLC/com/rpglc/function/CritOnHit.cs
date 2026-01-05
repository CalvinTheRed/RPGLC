using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Causes an attack roll hit to resolve as a critical hit.
///   
///   <code>
///   {
///     "function": "crit_on_hit"
///   }
///   </code>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>AttackRoll</item>
///   </list>
///   
/// </summary>
public class CritOnHit : Function {

    public CritOnHit() : base("crit_on_hit") {
        functionSteps.AddRange([
            (rpglEffect, subevent, functionJson, context) => {
                if (subevent is AttackRoll attackRoll) {
                    attackRoll.SetCritOnHit();
                }
                return new() {
                    dependency = null,
                    stepCompleted = true,
                };
            },
        ]);
    }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is AttackRoll attackRoll) {
            attackRoll.SetCritOnHit();
        }
    }

};
