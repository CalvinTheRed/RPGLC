using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Revokes immunity to a damage type.
///   
///   <code>
///   {
///     "function": "revoke_immunity",
///     "damage_type": &lt;string = "*"&gt;
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"damage_type" is an optional field and will default to a value of "*" if not specified. This value causes the function to revoke immunity to all damage types, rather than a singular damage type. This field represents a damage type.</item>
///   </list>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>DamageAffinity</item>
///   </list>
///   
/// </summary>
public class RevokeImmunity : Function {

    public RevokeImmunity() : base("revoke_immunity") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is DamageAffinity damageAffinity) {
            damageAffinity.RevokeImmunity(functionJson.GetString("damage_type") ?? "*");
        }
    }

};
