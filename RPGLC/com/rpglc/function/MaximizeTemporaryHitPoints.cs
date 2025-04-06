using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Causes all temporary hit point dice to roll their largest value.
///   
///   <code>
///   {
///     "function": "maximize_temporary_hit_points"
///   }
///   </code>
///   
///   <i>Note that this function should be paired with a condition checking for the subevent to have either the "base" or "target" tag. Neglecting this check may cause the maximization to be applied to a temporary hit point subevent at undesired times.</i>
///   
///   <br /><br />
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>TemporaryHitPointRoll</item>
///     <item>TemporaryHitPointDelivery</item>
///   </list>
///   
/// </summary>
public class MaximizeTemporaryHitPoints : Function {

    public MaximizeTemporaryHitPoints() : base("maximize_temporary_hit_points") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is TemporaryHitPointRoll temporaryHitPointRoll) {
            temporaryHitPointRoll.MaximizeTemporaryHitPointDice();
        } else if (subevent is TemporaryHitPointDelivery temporaryHitPointDelivery) {
            temporaryHitPointDelivery.MaximizeTemporaryHitPointDice();
        }
    }

};
