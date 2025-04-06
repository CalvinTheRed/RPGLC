using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Causes all damage dice of a specified damage type to roll their largest value.
///   
///   <code>
///   {
///     "function": "maximize_damage",
///     "damage_type": &lt;string = "*"&gt;
///   }
///   </code>
///   
///   <i>Note that this function should be paired with a condition checking for the subevent to have either the "base" or "target" tag. Neglecting this check may cause the maximization to be applied to a damaging subevent at undesired times.</i>
///   
///   <list type="bullet">
///     <item>"damage_type" is an optional field and will default to a value of "*" if not specified. This value causes the function to maximize damage of all types, rather than a singular type. This field represents a damage type.</item>
///   </list>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>DamageRoll</item>
///     <item>DamageDelivery</item>
///   </list>
///   
/// </summary>
public class MaximizeDamage : Function {

    public MaximizeDamage() : base("maximize_damage") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is DamageRoll damageRoll) {
            damageRoll.MaximizeDamageDice(functionJson.GetString("damage_type") ?? "*");
        } else if (subevent is DamageDelivery damageDelivery) {
            damageDelivery.MaximizeDamageDice(functionJson.GetString("damage_type") ?? "*");
        }
    }

};
