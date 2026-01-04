using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.math;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Adds a list of bonuses to a temporary hit point collection according to defined formulae.
///   
///   <code>
///   {
///     "function": "add_temporary_hit_points",
///     "bonus": [
///       {
///         &lt;bonus formula details&gt;
///       }
///     ]
///   }
///   </code>
///   
///   <i>Note that this function should be paired with a condition checking for the subevent to have either the "base" or "target" tag. Neglecting this check will cause the bonus to be applied to a temporary hit point collection more than once.</i>
///   
///   <list type="bullet">
///     <item>"temporary_hit_points" is a list of calculation formulae to add to a temporary hit point collection.</item>
///   </list>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>TemporaryHitPointCollection</item>
///   </list>
///   
/// </summary>
public class AddTemporaryHitPoints : Function {

    public int bonusIndex = 0;

    public AddTemporaryHitPoints() : base("add_temporary_hit_points") {
        functionSteps.AddRange([
            (rpglEffect, subevent, functionJson, context) => {
                if (subevent is TemporaryHitPointCollection temporaryHitPointCollection) {
                    JsonArray bonusArray = functionJson.GetJsonArray("bonus") ?? new();
                    if (bonusIndex < bonusArray.Count()) {
                        JsonObject bonusJson = bonusArray.GetJsonObject(bonusIndex);
                        bonusIndex++;

                        string? formula = bonusJson.GetString("formula");
                        if (formula == "number") {
                            AdvanceNumber(temporaryHitPointCollection, bonusJson);
                        } else if (formula == "dice") {
                            AdvanceDice(temporaryHitPointCollection, bonusJson);
                        } else if (formula == "modifier") {
                            AdvanceModifier(rpglEffect, temporaryHitPointCollection, bonusJson, context);
                        } else if (formula == "ability") {
                            AdvanceAbility(rpglEffect, temporaryHitPointCollection, bonusJson, context);
                        } else if (formula == "proficiency") {
                            AdvanceProficiency(rpglEffect, temporaryHitPointCollection, bonusJson, context);
                        } else if (formula == "level") {
                            AdvanceLevel(rpglEffect, temporaryHitPointCollection, bonusJson);
                        }
                        return new() {
                            dependency = this.dependency,
                            stepCompleted = false,
                        };
                    }
                }
                return new() {
                    dependency = null,
                    stepCompleted = true,
                };
            },
        ]);
    }

    public static void AdvanceNumber(TemporaryHitPointCollection temporaryHitPointCollection, JsonObject bonusJson) {
        temporaryHitPointCollection.AddTemporaryHitPoints(new JsonObject().LoadFromString($$"""
            {
                "bonus": {{bonusJson.GetLong("number")}},
                "dice": [ ],
                "scale": {{bonusJson.GetJsonObject("scale")?.ToString() ?? $$"""
                {
                    "numerator": 1,
                    "denominator": 1,
                    "round_up": false
                }
                """}}
            }
            """));
    }

    public static void AdvanceDice(TemporaryHitPointCollection temporaryHitPointCollection, JsonObject bonusJson) {
        temporaryHitPointCollection.AddTemporaryHitPoints(new JsonObject().LoadFromString($$"""
            {
                "bonus": 0,
                "dice": {{Die.Unpack(bonusJson.GetJsonArray("dice"))}},
                "scale": {{bonusJson.GetJsonObject("scale")?.ToString() ?? $$"""
                {
                    "numerator": 1,
                    "denominator": 1,
                    "round_up": false
                }
                """}}
            }
            """));
    }

    public void AdvanceModifier(RPGLEffect rpglEffect, TemporaryHitPointCollection temporaryHitPointCollection, JsonObject bonusJson, RPGLContext context) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, temporaryHitPointCollection, bonusJson.GetJsonObject("object"));
        if (this.dependency is null) {
            this.dependency = new CalculateAbilityScore()
                .JoinSubeventData(new JsonObject().LoadFromString($$"""
                    {
                        "tags": {{rpglObject.GetTags().DeepClone()}},
                        "ability": "{{bonusJson.GetString("ability")}}"
                    }
                    """)
                )
                .SetSource(rpglObject)
                .SetTarget(rpglObject);
            bonusIndex--;
        } else {
            temporaryHitPointCollection.AddTemporaryHitPoints(new JsonObject().LoadFromString($$"""
            {
                "bonus": {{RPGLObject.GetAbilityModifierFromAbilityScore((dependency as CalculationSubevent).Get())}},
                "dice": [ ],
                "scale": {{bonusJson.GetJsonObject("scale")?.ToString() ?? $$"""
                {
                    "numerator": 1,
                    "denominator": 1,
                    "round_up": false
                }
                """}}
            }
            """));
            dependency = null;
        }
    }

    public void AdvanceAbility(RPGLEffect rpglEffect, TemporaryHitPointCollection temporaryHitPointCollection, JsonObject bonusJson, RPGLContext context) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, temporaryHitPointCollection, bonusJson.GetJsonObject("object"));
        if (this.dependency is null) {
            this.dependency = new CalculateAbilityScore()
                .JoinSubeventData(new JsonObject().LoadFromString($$"""
                    {
                        "tags": {{rpglObject.GetTags().DeepClone()}},
                        "ability": "{{bonusJson.GetString("ability")}}"
                    }
                    """)
                )
                .SetSource(rpglObject)
                .SetTarget(rpglObject);
            bonusIndex--;
        } else {
            temporaryHitPointCollection.AddTemporaryHitPoints(new JsonObject().LoadFromString($$"""
            {
                "bonus": {{(dependency as CalculationSubevent).Get()}},
                "dice": [ ],
                "scale": {{bonusJson.GetJsonObject("scale")?.ToString() ?? $$"""
                {
                    "numerator": 1,
                    "denominator": 1,
                    "round_up": false
                }
                """}}
            }
            """));
            dependency = null;
        }
    }

    public void AdvanceProficiency(RPGLEffect rpglEffect, TemporaryHitPointCollection temporaryHitPointCollection, JsonObject bonusJson, RPGLContext context) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, temporaryHitPointCollection, bonusJson.GetJsonObject("object"));
        if (this.dependency is null) {
            this.dependency = new CalculateProficiencyBonus()
                .JoinSubeventData(new JsonObject().LoadFromString($$"""
                    {
                        "tags": {{rpglObject.GetTags().DeepClone()}}
                    }
                    """)
                )
                .SetSource(rpglObject)
                .SetTarget(rpglObject);
            bonusIndex--;
        } else {
            temporaryHitPointCollection.AddTemporaryHitPoints(new JsonObject().LoadFromString($$"""
            {
                "bonus": {{(dependency as CalculationSubevent).Get()}},
                "dice": [ ],
                "scale": {{bonusJson.GetJsonObject("scale")?.ToString() ?? $$"""
                {
                    "numerator": 1,
                    "denominator": 1,
                    "round_up": false
                }
                """}}
            }
            """));
            dependency = null;
        }
    }

    public static void AdvanceLevel(RPGLEffect rpglEffect, TemporaryHitPointCollection temporaryHitPointCollection, JsonObject bonusJson) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, temporaryHitPointCollection, bonusJson.GetJsonObject("object"));
        string classDatapackId = bonusJson.GetString("class") ?? "*";

        temporaryHitPointCollection.AddTemporaryHitPoints(new JsonObject().LoadFromString($$"""
            {
                "bonus": {{(classDatapackId == "*" ? rpglObject.GetLevel() : rpglObject.GetLevel(classDatapackId))}},
                "dice": [ ],
                "scale": {{bonusJson.GetJsonObject("scale")?.ToString() ?? $$"""
                {
                    "numerator": 1,
                    "denominator": 1,
                    "round_up": false
                }
                """}}
            }
            """));
    }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is TemporaryHitPointCollection temporaryHitPointCollection) {
            JsonArray temporaryHitPointArray = functionJson.GetJsonArray("temporary_hit_points");
            for (int i = 0; i < temporaryHitPointArray.Count(); i++) {
                temporaryHitPointCollection.AddTemporaryHitPoints(CalculationSubevent.SimplifyCalculationFormula(
                    rpglEffect,
                    subevent,
                    temporaryHitPointArray.GetJsonObject(i),
                    context
                ));
            }
        }
    }

};
