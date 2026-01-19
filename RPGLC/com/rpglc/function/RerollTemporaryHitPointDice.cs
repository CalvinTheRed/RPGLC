using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.math;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Rerolls temporary hit point dice that rolled within a defined range in a temporary hit point roll.
///   
///   <code>
///   {
///     "function": "reroll_temporary_hit_point_dice",
///     "lower_bound": &lt;long = 0&gt;,
///     "upper_bound": &lt;long = long.MaxValue&gt;
///   }
///   </code>
///   
///   <i>Note that this function should be paired with a condition checking for the subevent to have either the "base" or "target" tag. Neglecting this check will cause the re-roll to be performed on a temporary hit point subevent more than once.</i>
///   
///   <list type="bullet">
///     <item>"lower_bound" is an optional field and will default to a value of 0 if not specified. This field indicates the minimum value (inclusive) a die must have rolled to be eligible for a reroll.</item>
///     <item>"upper_bound" is an optional field and will default to a value of long.MaxValue if not specified. This field indicates the maximum value (inclusive) a die must have rolled to be eligible for a reroll.</item>
///   </list>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>TemporaryHitPointRoll</item>
///   </list>
///   
/// </summary>
public class RerollTemporaryHitPointDice : Function {

    public RerollTemporaryHitPointDice() : base("reroll_temporary_hit_point_dice") {
        functionSteps.AddRange([
            (rpglEffect, subevent, functionJson, context) => {
                if (subevent is TemporaryHitPointRoll temporaryHitPointRoll) {
                    functionJson.PutIfAbsent("threshold", new JsonObject().LoadFromString($$"""
                        {
                            "formula": "number",
                            "number": {{long.MaxValue}}
                        }
                        """));

                    string formula = functionJson.SeekString("threshold.formula");
                    if (formula == "number") {
                        AdvanceNumber(temporaryHitPointRoll, functionJson);
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

    public override RerollTemporaryHitPointDice Clone() {
        return new();
    }

    public static void AdvanceNumber(TemporaryHitPointRoll temporaryHitPointRoll, JsonObject functionJson) {
        JsonArray temporaryHitPointArray = temporaryHitPointRoll.json.GetJsonArray("temporary_hit_points");
        for (int i = 0; i < temporaryHitPointArray.Count(); i++) {
            JsonObject temporaryHitPointJson = temporaryHitPointArray.GetJsonObject(i);
            JsonArray temporaryHitPointDieArray = temporaryHitPointJson.GetJsonArray("dice") ?? new();
            for (int j = 0; j < temporaryHitPointDieArray.Count(); j++) {
                JsonObject temporaryHitPointDie = temporaryHitPointDieArray.GetJsonObject(j);
                long roll = (long) temporaryHitPointDie.GetLong("roll");
                long overrideValue = (long) functionJson.SeekLong("threshold.number");
                if (roll <= overrideValue) {
                    Die.Roll(temporaryHitPointDie);
                }
            }
        }
    }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is TemporaryHitPointRoll temporaryHitPointRoll) {
            temporaryHitPointRoll.RerollTemporaryHitPointDice(
                functionJson.GetLong("lower_bound") ?? 0L,
                functionJson.GetLong("upper_bound") ?? long.MaxValue
            );
        }
    }

};
