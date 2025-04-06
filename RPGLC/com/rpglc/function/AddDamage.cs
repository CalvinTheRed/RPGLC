using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Adds a list of bonuses to a damage collection according to defined formulae.
///   
///   <code>
///   {
///     "function": "add_damage",
///     "damage": [
///       &lt;calculation formula&gt;
///     ]
///   }
///   </code>
///   
///   <i>Note that this function should be paired with a condition checking for the subevent to have either the "base" or "target" tag. Neglecting this check will cause the bonus to be applied to a damaging subevent more than once.</i>
///   
///   <list type="bullet">
///     <item>"bonus" is a list of calculation formulae to add to a damage collection. Note that these formulas must include a "damage_type" field to work as intended.</item>
///   </list>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>DamageCollection</item>
///     <item>CriticalHitDamageCollection</item>
///   </list>
///   
/// </summary>
public class AddDamage : Function {

    public AddDamage() : base("add_damage") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is DamageCollection damageCollection) {
            JsonArray damageArray = functionJson.GetJsonArray("damage");
            for (int i = 0; i < damageArray.Count(); i++) {
                JsonObject damageElement = damageArray.GetJsonObject(i);
                JsonObject damage = CalculationSubevent.SimplifyCalculationFormula(rpglEffect, subevent, damageElement, context);
                string? damageType = damageElement.GetString("damage_type");
                if (damageType is null) {
                    damage.PutString("damage_type", damageCollection.GetDamageCollection().GetJsonObject(0).GetString("damage_type"));
                } else {
                    damage.PutString("damage_type", damageType);
                }
                damageCollection.AddDamage(damage);
            }
        } else if (subevent is CriticalHitDamageCollection criticalHitDamageCollection) {
            JsonArray damageArray = functionJson.GetJsonArray("damage");
            for (int i = 0; i < damageArray.Count(); i++) {
                JsonObject damageElement = damageArray.GetJsonObject(i);
                JsonObject damage = CalculationSubevent.SimplifyCalculationFormula(rpglEffect, subevent, damageElement, context);
                string? damageType = damageElement.GetString("damage_type");
                if (damageType is null) {
                    damage.PutString("damage_type", criticalHitDamageCollection.GetDamageCollection().GetJsonObject(0).GetString("damage_type"));
                } else {
                    damage.PutString("damage_type", damageType);
                }
                criticalHitDamageCollection.AddDamage(damage);
            }
        }
    }

};
