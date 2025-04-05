using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Grants immunity to a damage type.
///   
///   <code>
///   {
///     "function": "grant_immunity",
///     "damage_type": &lt;string = "*"&gt;
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"damage_type" is an optional field and will default to a value of "*" if not specified. This value causes the function to grant immunity to all damage types, rather than a singular damage type. This field represents a damage type.</item>
///   </list>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>DamageAffinity</item>
///   </list>
///   
/// </summary>
public class GrantImmunity : Function {

    public GrantImmunity() : base("grant_immunity") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is DamageAffinity damageAffinity) {
            damageAffinity.GrantImmunity(functionJson.GetString("damage_type") ?? "*");
        }
    }

};
