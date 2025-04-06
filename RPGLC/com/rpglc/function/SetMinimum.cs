using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Sets the minimum value of a calculation subevent according to a defined formula.
///   
///   <code>
///   {
///     "function": "set_minimum",
///     "minimum": &lt;calculation formula&gt;
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"minimum" is a calculation formula that defines the minimum value for a calculation-type subevent. If the function's minimum formula provides a number less than the subevent's current minimum, nothing changes.</item>
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
public class SetMinimum : Function {

    public SetMinimum() : base("set_minimum") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is CalculationSubevent calculationSubevent) {
            calculationSubevent.SetMinimum(CalculationSubevent.ProcessFormulaJson(
                CalculationSubevent.SimplifyCalculationFormula(rpglEffect, subevent, functionJson.GetJsonObject("minimum"), context))
            );
        }
    }

};
