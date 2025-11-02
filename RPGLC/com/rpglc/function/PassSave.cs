using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Causes a saving throw to resolve as a pass regardless of the result of the roll.
///   
///   <code>
///   {
///     "function": "pass_save"
///   }
///   </code>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>SavingThrow</item>
///   </list>
///   
/// </summary>
public class PassSave : Function {

    public PassSave() : base("pass_save") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is SavingThrow savingThrow) {
            savingThrow.Pass();
        }
    }

};
