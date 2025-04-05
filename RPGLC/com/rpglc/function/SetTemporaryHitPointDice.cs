using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Overrides the value of temporary hit point dice that rolled within a defined range in a temporary hit point roll.
///   
///   <code>
///   {
///     "function": "set_temporary_hit_point_dice",
///     "set": &lt;set formula&gt;,
///     "lower_bound": &lt;long = 0&gt;,
///     "upper_bound": &lt;long = long.MaxValue&gt;
///   }
///   </code>
///   
///   <i>Note that this function should be paired with a condition checking for the subevent to have either the "base" or "target" tag. Neglecting this check will cause the override to be performed on a temporary hit point subevent at undesired times.</i>
///   
///   <list type="bullet">
///     <item>"set" is a set formula that defines the value for an eligible die's rolled value to be set to.</item>
///     <item>"lower_bound" is an optional field and will default to a value of 0 if not specified. This field indicates the minimum value (inclusive) a die must have rolled to be eligible for an override.</item>
///     <item>"upper_bound" is an optional field and will default to a value of long.MaxValue if not specified. This field indicates the maximum value (inclusive) a die must have rolled to be eligible for an override.</item>
///   </list>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>TemporaryHitPointRoll</item>
///   </list>
///   
/// </summary>
public class SetTemporaryHitPointDice : Function {

    public SetTemporaryHitPointDice() : base("set_temporary_hit_point_dice") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is TemporaryHitPointRoll temporaryHitPointRoll) {
            temporaryHitPointRoll.SetTemporaryHitPointDice(rpglEffect, functionJson, context);
        }
    }

};
