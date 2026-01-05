using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Overrides the value of temporary hit point dice that rolled within a defined range in a temporary hit point roll.
///   
///   <code>
///   {
///     "function": "override_temporary_hit_point_dice",
///     "override": &lt;calculation formula&gt;,
///     "lower_bound": &lt;long = 0&gt;,
///     "upper_bound": &lt;long = long.MaxValue&gt;
///   }
///   </code>
///   
///   <i>Note that this function should be paired with a condition checking for the subevent to have either the "base" or "target" tag. Neglecting this check will cause the override to be performed on a temporary hit point subevent at undesired times.</i>
///   
///   <list type="bullet">
///     <item>"override" is a calculation formula that defines the value for an eligible die's rolled value to be overridden with.</item>
///     <item>"lower_bound" is an optional field and will default to a value of 0 if not specified. This field indicates the minimum value (inclusive) a die must have rolled to be eligible for an override.</item>
///     <item>"upper_bound" is an optional field and will default to a value of long.MaxValue if not specified. This field indicates the maximum value (inclusive) a die must have rolled to be eligible for an override.</item>
///   </list>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>TemporaryHitPointRoll</item>
///   </list>
///   
/// </summary>
public class OverrideTemporaryHitPointDice : Function {

    public OverrideTemporaryHitPointDice() : base("override_temporary_hit_point_dice") {
        functionSteps.AddRange([
            (rpglEffect, subevent, functionJson, context) => {
                if (subevent is TemporaryHitPointRoll temporaryHitPointRoll) {
                    JsonObject overrideJson = functionJson.GetJsonObject("override");

                    string formula = overrideJson.GetString("formula");
                    if (formula == "number") {
                        AdvanceNumber(temporaryHitPointRoll, functionJson);
                    } else if (formula == "modifier") {
                        AdvanceModifier(rpglEffect, temporaryHitPointRoll, functionJson, context);
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

    public static void AdvanceNumber(TemporaryHitPointRoll temporaryHitPointRoll, JsonObject functionJson) {
        JsonArray temporaryHitPointArray = temporaryHitPointRoll.json.GetJsonArray("temporary_hit_points");
        for (int i = 0; i < temporaryHitPointArray.Count(); i++) {
            JsonObject temporaryHitPointJson = temporaryHitPointArray.GetJsonObject(i);
            JsonArray temporaryHitPointDieArray = temporaryHitPointJson.GetJsonArray("dice") ?? new();
            for (int j = 0; j < temporaryHitPointDieArray.Count(); j++) {
                JsonObject temporaryHitPointDie = temporaryHitPointDieArray.GetJsonObject(j);
                long roll = (long) temporaryHitPointDie.GetLong("roll");
                long overrideValue = (long) functionJson.SeekLong("override.number");
                if (roll < overrideValue) {
                    temporaryHitPointDie.PutLong("roll", overrideValue);
                }
            }
        }
    }

    public void AdvanceModifier(RPGLEffect rpglEffect, TemporaryHitPointRoll temporaryHitPointRoll, JsonObject functionJson, RPGLContext context) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, temporaryHitPointRoll, functionJson.SeekJsonObject("override.object"));
        if (this.dependency is null) {
            this.dependency = new CalculateAbilityScore()
                .JoinSubeventData(new JsonObject().LoadFromString($$"""
                    {
                        "tags": {{rpglObject.GetTags()}},
                        "ability": "{{functionJson.SeekString("override.ability")}}"
                    }
                    """)
                )
                .SetSource(rpglObject)
                .SetTarget(rpglObject);
        } else {
            JsonArray temporaryHitPointArray = temporaryHitPointRoll.json.GetJsonArray("temporary_hit_points");
            for (int i = 0; i < temporaryHitPointArray.Count(); i++) {
                JsonObject temporaryHitPointJson = temporaryHitPointArray.GetJsonObject(i);
                JsonArray temporaryHitPointDieArray = temporaryHitPointJson.GetJsonArray("dice") ?? new();
                for (int j = 0; j < temporaryHitPointDieArray.Count(); j++) {
                    JsonObject temporaryHitPointDie = temporaryHitPointDieArray.GetJsonObject(j);
                    long roll = (long) temporaryHitPointDie.GetLong("roll");
                    long overrideValue = RPGLObject.GetAbilityModifierFromAbilityScore((dependency as CalculationSubevent).Get());
                    if (roll < overrideValue) {
                        temporaryHitPointDie.PutLong("roll", overrideValue);
                    }
                }
            }
            dependency = null;
        }
    }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is TemporaryHitPointRoll temporaryHitPointRoll) {
            temporaryHitPointRoll.OverrideTemporaryHitPointDice(rpglEffect, functionJson, context);
        }
    }

};
