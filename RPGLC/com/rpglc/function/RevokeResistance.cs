using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Revokes resistance to a damage type.
///   
///   <code>
///   {
///     "function": "revoke_resistance",
///     "damage_type": &lt;string = "*"&gt;
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"damage_type" is an optional field and will default to a value of "*" if not specified. This value causes the function to revoke resistance to all damage types, rather than a singular damage type. This field represents a damage type.</item>
///   </list>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>DamageAffinity</item>
///   </list>
///   
/// </summary>
public class RevokeResistance : Function {

    public RevokeResistance() : base("revoke_resistance") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is DamageAffinity damageAffinity) {
            damageAffinity.RevokeResistance(functionJson.GetString("damage_type") ?? "*");
        }
    }

};
