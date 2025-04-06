using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Repeats the first damage die contained within a damage collection a specified number of times.
///   
///   <code>
///   {
///     "function": "repeat_damage_dice",
///     "count": &lt;long = 1&gt;
///   }
///   </code>
///   
///   <i>Note that this function should be paired with a condition checking for DamageCollection subevents to have either the "base" or "target" tag. Neglecting this check will cause the repetition to be performed to a damaging subevent more than once.</i>
///   
///   <list type="bullet">
///     <item>"count" is an optional field and will default to a value of 1 if not specified. This field represents a number of times to repeat the first damage die in a damage collection.</item>
///   </list>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>CriticalHitDamageCollection</item>
///     <item>DamageCollection</item>
///   </list>
///   
/// </summary>
public class RepeatDamageDice : Function {

    public RepeatDamageDice() : base("repeat_damage_dice") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is DamageCollection damageCollection) {
            RepeatDice(damageCollection.GetDamageCollection(), functionJson);
        } else if (subevent is CriticalHitDamageCollection criticalHitDamageCollection) {
            RepeatDice(criticalHitDamageCollection.GetDamageCollection(), functionJson);
        }
    }

    private void RepeatDice(JsonArray damageArray, JsonObject functionJson) {
        if (damageArray.Count() > 0) {
            JsonObject damageJson = damageArray.GetJsonObject(0);
            JsonArray dice = damageJson.GetJsonArray("dice");
            if (dice.Count() > 0) {
                JsonObject die = dice.GetJsonObject(0);
                for (int i = 0; i < (functionJson.GetLong("count") ?? 1L); i++) {
                    dice.AddJsonObject(die.DeepClone());
                }
            }
        }
    }

};
