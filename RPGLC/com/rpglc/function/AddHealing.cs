using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Adds a list of bonuses to a healing collection according to defined formulae.
///   
///   <code>
///   {
///     "function": "add_healing",
///     "healing": [
///       &lt;bonus formula&gt;
///     ]
///   }
///   </code>
///   
///   <i>Note that this function should be paired with a condition checking for the subevent to have either the "base" or "target" tag. Neglecting this check will cause the bonus to be applied to a healing subevent more than once.</i>
///   
///   <list type="bullet">
///     <item>"healing" is a list of bonus formulae to add to the healing collection.</item>
///   </list>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>HealingCollection</item>
///   </list>
///   
/// </summary>
public class AddHealing : Function {

    public AddHealing() : base("add_healing") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is HealingCollection healingCollection) {
            JsonArray healingArray = functionJson.GetJsonArray("healing");
            for (int i = 0; i < healingArray.Count(); i++) {
                healingCollection.AddHealing(CalculationSubevent.ProcessBonusJson(rpglEffect, subevent, healingArray.GetJsonObject(i), context));
            }
        }
    }

};
