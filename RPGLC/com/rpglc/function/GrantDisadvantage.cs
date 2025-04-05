using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Grants disadvantage to a d20 subevent.
///   
///   <code>
///   {
///     "function": "grant_disadvantage"
///   }
///   </code>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>AbilityCheck</item>
///     <item>AttackRoll</item>
///     <item>SavingThrow</item>
///   </list>
///   
/// </summary>
public class GrantDisadvantage : Function {

    public GrantDisadvantage() : base("grant_disadvantage") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is RollSubevent rollSubevent) {
            rollSubevent.GrantDisadvantage();
        }
    }

};
