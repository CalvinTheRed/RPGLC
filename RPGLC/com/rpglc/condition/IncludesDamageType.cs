using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.condition;

/// <summary>
///   Returns true if the subevent's damage type matches the condition's subevent.
///   
///   <code>
///   {
///     "condition": "check_level",
///     "damage_type": &lt;string&gt;
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"damage_type" indicates which damage type the subevent is expected to include.</item>
///   </list>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>CriticalHitDamageCollection</item>
///     <item>DamageAffinity</item>
///     <item>DamageCollection</item>
///     <item>DamageDelivery</item>
///     <item>DamageRoll</item>
///   </list>
///   
/// </summary>
public class IncludesDamageType : Condition {

    public IncludesDamageType() : base("includes_damage_type") { }

    public override bool Run(RPGLEffect rpglEffect, Subevent subevent, JsonObject conditionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is IDamageTypeSubevent damageTypeSubevent) {
            return damageTypeSubevent.IncludesDamageType(conditionJson.GetString("damage_type"));
        }
        return false;
    }

};
