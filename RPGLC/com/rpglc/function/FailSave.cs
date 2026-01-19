using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Causes a saving throw to resolve as a fail regardless of the result of the roll.
///   
///   <code>
///   {
///     "function": "fail_save"
///   }
///   </code>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>SavingThrow</item>
///   </list>
///   
/// </summary>
public class FailSave : Function {

    public FailSave() : base("fail_save") {
        functionSteps.AddRange([
            (rpglEffect, subevent, functionJson, context) => {
                if (subevent is SavingThrow savingThrow) {
                    savingThrow.Fail();
                }
                return new() {
                    dependency = null,
                    stepCompleted = true,
                };
            },
        ]);
    }

    public override FailSave Clone() {
        return new();
    }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is SavingThrow savingThrow) {
            savingThrow.Fail();
        }
    }

};
