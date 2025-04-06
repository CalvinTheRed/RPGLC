using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Sets the base value of a calculation subevent according to a defined formula.
///   
///   <code>
///   {
///     "function": "set_base",
///     "base": &lt;base formula&gt;
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"base" is a base formula that sets the base of a calculation-type subevent.</item>
///   </list>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>CalculateAbilityScore</item>
///     <item>CalculateArmorClass</item>
///     <item>CalculateCriticalHitThreshhold</item>
///     <item>CalculateDifficultyClass</item>
///     <item>CalculateMaximumHitPoints</item>
///     <item>CalculateProficiencyBonus</item>
///   </list>
///   
/// </summary>
public class SetBase : Function {

    public SetBase() : base("set_base") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is CalculationSubevent calculationSubevent) {
            calculationSubevent.SetBase(CalculationSubevent.ProcessSetJson(rpglEffect, subevent, functionJson.GetJsonObject("base"), context));
        }
    }

};
