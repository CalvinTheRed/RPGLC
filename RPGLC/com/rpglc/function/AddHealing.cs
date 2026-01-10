using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.math;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Adds a list of bonuses to a healing collection according to defined formulae.
///   
///   <code>
///   {
///     "function": "add_healing",
///     "bonus": [
///       {
///         &lt;bonus formula details&gt;
///       }
///     ]
///   }
///   </code>
///   
///   <i>Note that this function should be paired with a condition checking for the subevent to have either the "base" or "target" tag. Neglecting this check will cause the bonus to be applied to a healing subevent more than once.</i>
///   
///   <list type="bullet">
///     <item>"healing" is a list of calculation formulae to add to the healing collection.</item>
///   </list>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>HealingCollection</item>
///   </list>
///   
/// </summary>
public class AddHealing : Function {

    public int bonusIndex = 0;

    public AddHealing() : base("add_healing") {
        functionSteps.AddRange([
            (rpglEffect, subevent, functionJson, context) => {
                if (subevent is HealingCollection healingCollection) {
                    JsonArray bonusArray = functionJson.GetJsonArray("bonus") ?? new();
                    if (bonusIndex < bonusArray.Count()) {
                        JsonObject bonusJson = bonusArray.GetJsonObject(bonusIndex);
                        bonusIndex++;

                        string? formula = bonusJson.GetString("formula");
                        if (formula == "number") {
                            AdvanceNumber(healingCollection, bonusJson);
                        } else if (formula == "dice") {
                            AdvanceDice(healingCollection, bonusJson);
                        } else if (formula == "modifier") {
                            AdvanceModifier(rpglEffect, healingCollection, bonusJson, context);
                        } else if (formula == "ability") {
                            AdvanceAbility(rpglEffect, healingCollection, bonusJson, context);
                        } else if (formula == "proficiency") {
                            AdvanceProficiency(rpglEffect, healingCollection, bonusJson, context);
                        } else if (formula == "level") {
                            AdvanceLevel(rpglEffect, healingCollection, bonusJson);
                        }
                        return new() {
                            dependency = this.dependency,
                            stepCompleted = this.dependency == null && bonusIndex == bonusArray.Count(),
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

    public static void AdvanceNumber(HealingCollection healingCollection, JsonObject bonusJson) {
        healingCollection.AddHealing(new JsonObject().LoadFromString($$"""
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

    public static void AdvanceDice(HealingCollection healingCollection, JsonObject bonusJson) {
        healingCollection.AddHealing(new JsonObject().LoadFromString($$"""
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

    public void AdvanceModifier(RPGLEffect rpglEffect, HealingCollection healingCollection, JsonObject bonusJson, RPGLContext context) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, healingCollection, bonusJson.GetJsonObject("object"));
        if (this.dependency is null) {
            this.dependency = new CalculateAbilityScore()
                .JoinSubeventData(new JsonObject().LoadFromString($$"""
                    {
                        "tags": {{rpglObject.GetTags()}},
                        "ability": "{{bonusJson.GetString("ability")}}"
                    }
                    """))
                .SetSource(rpglObject)
                .SetTarget(rpglObject);
            bonusIndex--;
        } else {
            healingCollection.AddHealing(new JsonObject().LoadFromString($$"""
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

    public void AdvanceAbility(RPGLEffect rpglEffect, HealingCollection healingCollection, JsonObject bonusJson, RPGLContext context) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, healingCollection, bonusJson.GetJsonObject("object"));
        if (this.dependency is null) {
            this.dependency = new CalculateAbilityScore()
                .JoinSubeventData(new JsonObject().LoadFromString($$"""
                    {
                        "tags": {{rpglObject.GetTags()}},
                        "ability": "{{bonusJson.GetString("ability")}}"
                    }
                    """))
                .SetSource(rpglObject)
                .SetTarget(rpglObject);
            bonusIndex--;
        } else {
            healingCollection.AddHealing(new JsonObject().LoadFromString($$"""
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

    public void AdvanceProficiency(RPGLEffect rpglEffect, HealingCollection healingCollection, JsonObject bonusJson, RPGLContext context) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, healingCollection, bonusJson.GetJsonObject("object"));
        if (this.dependency is null) {
            this.dependency = new CalculateProficiencyBonus()
                .JoinSubeventData(new JsonObject().LoadFromString($$"""
                    {
                        "tags": {{rpglObject.GetTags()}}
                    }
                    """))
                .SetSource(rpglObject)
                .SetTarget(rpglObject);
            bonusIndex--;
        } else {
            healingCollection.AddHealing(new JsonObject().LoadFromString($$"""
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

    public static void AdvanceLevel(RPGLEffect rpglEffect, HealingCollection healingCollection, JsonObject bonusJson) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, healingCollection, bonusJson.GetJsonObject("object"));
        string classDatapackId = bonusJson.GetString("class") ?? "*";

        healingCollection.AddHealing(new JsonObject().LoadFromString($$"""
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
        if (subevent is HealingCollection healingCollection) {
            JsonArray healingArray = functionJson.GetJsonArray("healing");
            for (int i = 0; i < healingArray.Count(); i++) {
                healingCollection.AddHealing(CalculationSubevent.SimplifyCalculationFormula(rpglEffect, subevent, healingArray.GetJsonObject(i), context));
            }
        }
    }

};
