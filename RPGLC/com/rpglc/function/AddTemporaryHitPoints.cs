using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Adds a list of bonuses to a temporary hit point collection according to defined formulae.
///   
///   <code>
///   {
///     "function": "add_healing",
///     "temporary_hit_points": [
///       &lt;calculation formula&gt;
///     ]
///   }
///   </code>
///   
///   <i>Note that this function should be paired with a condition checking for the subevent to have either the "base" or "target" tag. Neglecting this check will cause the bonus to be applied to a temporary hit point collection more than once.</i>
///   
///   <list type="bullet">
///     <item>"temporary_hit_points" is a list of calculation formulae to add to a temporary hit point collection.</item>
///   </list>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>TemporaryHitPointCollection</item>
///   </list>
///   
/// </summary>
public class AddTemporaryHitPoints : Function {

    public AddTemporaryHitPoints() : base("add_temporary_hit_points") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is TemporaryHitPointCollection temporaryHitPointCollection) {
            JsonArray temporaryHitPointArray = functionJson.GetJsonArray("temporary_hit_points");
            for (int i = 0; i < temporaryHitPointArray.Count(); i++) {
                temporaryHitPointCollection.AddTemporaryHitPoints(CalculationSubevent.SimplifyCalculationFormula(
                    rpglEffect,
                    subevent,
                    temporaryHitPointArray.GetJsonObject(i),
                    context
                ));
            }
        }
    }

};
