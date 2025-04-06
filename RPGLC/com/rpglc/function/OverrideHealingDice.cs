using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Overrides the value of healing dice that rolled within a defined range in a healing roll.
///   
///   <code>
///   {
///     "function": "override_damage_dice",
///     "override": &lt;calculation formula&gt;,
///     "lower_bound": &lt;long = 0&gt;,
///     "upper_bound": &lt;long = long.MaxValue&gt;
///   }
///   </code>
///   
///   <i>Note that this function should be paired with a condition checking for the subevent to have either the "base" or "target" tag. Neglecting this check will cause the override to be performed on a healing subevent at undesired times.</i>
///   
///   <list type="bullet">
///     <item>"override" is a calculation formula that defines the value for an eligible die's rolled value to be overridden with.</item>
///     <item>"lower_bound" is an optional field and will default to a value of 0 if not specified. This field indicates the minimum value (inclusive) a die must have rolled to be eligible for an override.</item>
///     <item>"upper_bound" is an optional field and will default to a value of long.MaxValue if not specified. This field indicates the maximum value (inclusive) a die must have rolled to be eligible for an override.</item>
///   </list>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>HealingRoll</item>
///   </list>
///   
/// </summary>
public class OverrideHealingDice : Function {

    public OverrideHealingDice() : base("override_healing_dice") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is HealingRoll healingRoll) {
            healingRoll.OverrideHealingDice(rpglEffect, functionJson, context);
        }
    }

};
