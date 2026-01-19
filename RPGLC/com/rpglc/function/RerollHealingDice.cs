using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.math;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Rerolls healing dice that rolled within a defined range in a healing roll.
///   
///   <code>
///   {
///     "function": "reroll_healing_dice",
///     "lower_bound": &lt;long = 0&gt;,
///     "upper_bound": &lt;long = long.MaxValue&gt;
///   }
///   </code>
///   
///   <i>Note that this function should be paired with a condition checking for the subevent to have either the "base" or "target" tag. Neglecting this check will cause the re-roll to be performed on a healing subevent more than once.</i>
///   
///   <list type="bullet">
///     <item>"lower_bound" is an optional field and will default to a value of 0 if not specified. This field indicates the minimum value (inclusive) a die must have rolled to be eligible for a reroll.</item>
///     <item>"upper_bound" is an optional field and will default to a value of long.MaxValue if not specified. This field indicates the maximum value (inclusive) a die must have rolled to be eligible for a reroll.</item>
///   </list>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>HealingRoll</item>
///   </list>
///   
/// </summary>
public class RerollHealingDice : Function {

    public RerollHealingDice() : base("reroll_healing_dice") {
        functionSteps.AddRange([
            (rpglEffect, subevent, functionJson, context) => {
                if (subevent is HealingRoll healingRoll) {
                    functionJson.PutIfAbsent("threshold", new JsonObject().LoadFromString($$"""
                        {
                            "formula": "number",
                            "number": {{long.MaxValue}}
                        }
                        """));

                    string formula = functionJson.SeekString("threshold.formula");
                    if (formula == "number") {
                        AdvanceNumber(healingRoll, functionJson);
                    }
                    return new() {
                        dependency = this.dependency,
                        stepCompleted = this.dependency == null,
                    };
                }
                return new() {
                    dependency = null,
                    stepCompleted = true,
                };
            },
        ]);
    }

    public override RerollHealingDice Clone() {
        return new();
    }

    public static void AdvanceNumber(HealingRoll healingRoll, JsonObject functionJson) {
        JsonArray healingArray = healingRoll.json.GetJsonArray("healing");
        for (int i = 0; i < healingArray.Count(); i++) {
            JsonObject healingJson = healingArray.GetJsonObject(i);
            JsonArray healingDieArray = healingJson.GetJsonArray("dice") ?? new();
            for (int j = 0; j < healingDieArray.Count(); j++) {
                JsonObject healingDie = healingDieArray.GetJsonObject(j);
                long roll = (long) healingDie.GetLong("roll");
                long overrideValue = (long) functionJson.SeekLong("threshold.number");
                if (roll <= overrideValue) {
                    Die.Roll(healingDie);
                }
            }
        }
    }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is HealingRoll healingRoll) {
            healingRoll.RerollHealingDice(
                functionJson.GetLong("lower_bound") ?? 0L,
                functionJson.GetLong("upper_bound") ?? long.MaxValue
            );
        }
    }

};
