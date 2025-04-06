using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Rerolls damage dice of a specified damage type that rolled within a defined range in a damage roll.
///   
///   <code>
///   {
///     "function": "reroll_damage_dice",
///     "damage_type": &lt;string = "*"&gt;,
///     "lower_bound": &lt;long = 0&gt;,
///     "upper_bound": &lt;long = long.MaxValue&gt;
///   }
///   </code>
///   
///   <i>Note that this function should be paired with a condition checking for the subevent to have either the "base" or "target" tag. Neglecting this check will cause the re-roll to be performed on a damaging subevent more than once.</i>
///   
///   <list type="bullet">
///     <item>"damage_type" is an optional field and will default to a value of "*" if not specified. This value causes the function to perform a reroll on eligible dice of all damage types, rather than on eligible dice of a single damage type.</item>
///     <item>"lower_bound" is an optional field and will default to a value of 0 if not specified. This field indicates the minimum value (inclusive) a die must have rolled to be eligible for a reroll.</item>
///     <item>"upper_bound" is an optional field and will default to a value of long.MaxValue if not specified. This field indicates the maximum value (inclusive) a die must have rolled to be eligible for a reroll.</item>
///   </list>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>DamageRoll</item>
///   </list>
///   
/// </summary>
public class RerollDamageDice : Function {

    public RerollDamageDice() : base("reroll_damage_dice") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is DamageRoll damageRoll) {
            damageRoll.RerollDamageDice(
                functionJson.GetString("damage_type") ?? "*",
                functionJson.GetLong("lower_bound") ?? 0L,
                functionJson.GetLong("upper_bound") ?? long.MaxValue
            );
        }
    }

};
