using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Causes all healing dice to roll their largest value.
///   
///   <code>
///   {
///     "function": "maximize_healing"
///   }
///   </code>
///   
///   <i>Note that this function should be paired with a condition checking for the subevent to have either the "base" or "target" tag. Neglecting this check may cause the maximization to be applied to a healing subevent at undesired times.</i>
///   
///   <br /><br />
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>HealingRoll</item>
///     <item>HealingDelivery</item>
///   </list>
///   
/// </summary>
public class MaximizeHealing : Function {

    public MaximizeHealing() : base("maximize_healing") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is HealingRoll healingRoll) {
            healingRoll.MaximizeHealingDice();
        } else if (subevent is HealingDelivery healingDelivery) {
            healingDelivery.MaximizeHealingDice();
        }
    }

};
