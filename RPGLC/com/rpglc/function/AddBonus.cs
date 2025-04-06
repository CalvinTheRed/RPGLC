using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Adds a list of bonuses to a calculation subevent according to defined formulae.
///   
///   <code>
///   {
///     "function": "add_bonus",
///     "bonus": [
///       &lt;bonus formula&gt;
///     ]
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"bonus" is a list of bonus formulae to add to a calculation-type subevent.</item>
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
public class AddBonus : Function {

    public AddBonus() : base("add_bonus") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is CalculationSubevent calculationSubevent) {
            JsonArray bonusArray = functionJson.GetJsonArray("bonus");
            for (int i = 0; i < bonusArray.Count(); i++) {
                calculationSubevent.AddBonus(CalculationSubevent.ProcessBonusJson(rpglEffect, subevent, bonusArray.GetJsonObject(i), context));
            }
        }
    }

};
