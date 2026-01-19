using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Causes a critical hit to be treated as a normal hit.
///   
///   <code>
///   {
///     "function": "suppress_critical_damage"
///   }
///   </code>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>CriticalDamageConfirmation</item>
///   </list>
///   
/// </summary>
public class SuppressCriticalDamage : Function {

    public SuppressCriticalDamage() : base("suppress_critical_damage") {
        functionSteps.AddRange([
            (rpglEffect, subevent, functionJson, context) => {
                if (subevent is CriticalDamageConfirmation criticalDamageConfirmation) {
                    criticalDamageConfirmation.SuppressCriticalDamage();
                }
                return new() {
                    dependency = null,
                    stepCompleted = true,
                };
            },
        ]);
    }

    public override SuppressCriticalDamage Clone() {
        return new();
    }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is CriticalDamageConfirmation criticalDamageConfirmation) {
            criticalDamageConfirmation.SuppressCriticalDamage();
        }
    }

};
