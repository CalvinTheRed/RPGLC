using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.math;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Adds a list of bonuses to a calculation subevent according to defined formulae.
///   
///   <code>
///   {
///     "function": "add_bonus",
///     "bonus": [
///       {
///         &lt;bonus formula details&gt;
///       }
///     ]
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"bonus" is a list of calculation formulae to add to a calculation-type subevent.</item>
///   </list>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>CalculateAbilityScore</item>
///     <item>CalculateArmorClass</item>
///     <item>CalculateCriticalHitThreshold</item>
///     <item>CalculateDifficultyClass</item>
///     <item>CalculateMaximumHitPoints</item>
///     <item>CalculateProficiencyBonus</item>
///   </list>
///   
/// </summary>
public class AddBonus : Function {

    public int bonusIndex = 0;

    public AddBonus() : base("add_bonus") {
        functionSteps.AddRange([
            (rpglEffect, subevent, functionJson, context) => {
                if (subevent is CalculationSubevent calculationSubevent) {
                    JsonArray bonusArray = functionJson.GetJsonArray("bonus") ?? new();
                    if (bonusIndex < bonusArray.Count()) {
                        JsonObject bonusJson = bonusArray.GetJsonObject(bonusIndex);
                        bonusIndex++;

                        string? formula = bonusJson.GetString("formula");
                        if (formula == "number") {
                            AdvanceNumber(calculationSubevent, bonusJson);
                        } else if (formula == "dice") {
                            AdvanceDice(calculationSubevent, bonusJson);
                        } else if (formula == "modifier") {
                            AdvanceModifier(rpglEffect, calculationSubevent, bonusJson, context);
                        } else if (formula == "ability") {
                            AdvanceAbility(rpglEffect, calculationSubevent, bonusJson, context);
                        } else if (formula == "proficiency") {
                            AdvanceProficiency(rpglEffect, calculationSubevent, bonusJson, context);
                        } else if (formula == "level") {
                            AdvanceLevel(rpglEffect, calculationSubevent, bonusJson);
                        }
                        return new() {
                            dependency = this.dependency,
                            completed = false,
                        };
                    }
                }
                return new() {
                    dependency = null,
                    completed = true,
                };
            },
        ]);
    }

    public static void AdvanceNumber(CalculationSubevent calculationSubevent, JsonObject bonusJson) {
        calculationSubevent.AddBonus(new JsonObject().LoadFromString($$"""
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

    public static void AdvanceDice(CalculationSubevent calculationSubevent, JsonObject bonusJson) {
        calculationSubevent.AddBonus(new JsonObject().LoadFromString($$"""
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

    public void AdvanceModifier(RPGLEffect rpglEffect, CalculationSubevent calculationSubevent, JsonObject bonusJson, RPGLContext context) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, calculationSubevent, bonusJson.GetJsonObject("object"));
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
            calculationSubevent.AddBonus(new JsonObject().LoadFromString($$"""
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

    public void AdvanceAbility(RPGLEffect rpglEffect, CalculationSubevent calculationSubevent, JsonObject bonusJson, RPGLContext context) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, calculationSubevent, bonusJson.GetJsonObject("object"));
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
            calculationSubevent.AddBonus(new JsonObject().LoadFromString($$"""
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

    public void AdvanceProficiency(RPGLEffect rpglEffect, CalculationSubevent calculationSubevent, JsonObject bonusJson, RPGLContext context) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, calculationSubevent, bonusJson.GetJsonObject("object"));
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
            calculationSubevent.AddBonus(new JsonObject().LoadFromString($$"""
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

    public static void AdvanceLevel(RPGLEffect rpglEffect, CalculationSubevent calculationSubevent, JsonObject bonusJson) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, calculationSubevent, bonusJson.GetJsonObject("object"));
        string classDatapackId = bonusJson.GetString("class") ?? "*";

        calculationSubevent.AddBonus(new JsonObject().LoadFromString($$"""
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
        if (subevent is CalculationSubevent calculationSubevent) {
            JsonArray bonusArray = functionJson.GetJsonArray("bonus");
            for (int i = 0; i < bonusArray.Count(); i++) {
                calculationSubevent.AddBonus(CalculationSubevent.SimplifyCalculationFormula(rpglEffect, subevent, bonusArray.GetJsonObject(i), context));
            }
        }
    }

};
