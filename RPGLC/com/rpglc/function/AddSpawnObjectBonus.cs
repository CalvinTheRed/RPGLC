using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.math;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Grants a bonus to a spawned object.
///   
///   <code>
///   {
///     "function": "add_spawn_object_bonus",
///     "bonus": [
///       {
///         "field": &lt;string&gt;,
///         &lt;bonus formula details&gt;
///       }
///     ]
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"bonus" is an object containing a specified field and a bonus to be applied to that field.</item>
///   </list>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>SpawnObject</item>
///   </list>
///   
/// </summary>
public class AddSpawnObjectBonus: Function {

    public int bonusIndex = 0;

    public AddSpawnObjectBonus() : base("add_spawn_object_bonus") {
        functionSteps.AddRange([
            (rpglEffect, subevent, functionJson, context) => {
                if (subevent is SpawnObject spawnObject) {
                    JsonArray bonusArray = functionJson.GetJsonArray("bonus") ?? new();
                    if (bonusIndex < bonusArray.Count()) {
                        JsonObject bonusJson = bonusArray.GetJsonObject(bonusIndex);
                        bonusIndex++;

                        string? formula = bonusJson.GetString("formula");
                        if (formula == "number") {
                            AdvanceNumber(spawnObject, bonusJson);
                        } else if (formula == "dice") {
                            AdvanceDice(spawnObject, bonusJson);
                        } else if (formula == "modifier") {
                            AdvanceModifier(rpglEffect, spawnObject, bonusJson, context);
                        } else if (formula == "ability") {
                            AdvanceAbility(rpglEffect, spawnObject, bonusJson, context);
                        } else if (formula == "proficiency") {
                            AdvanceProficiency(rpglEffect, spawnObject, bonusJson, context);
                        } else if (formula == "level") {
                            AdvanceLevel(rpglEffect, spawnObject, bonusJson);
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

    public static void AdvanceNumber(SpawnObject spawnObject, JsonObject bonusJson) {
        spawnObject.AddObjectBonus(new JsonObject().LoadFromString($$"""
            {
                "field": "{{bonusJson.GetString("field")}}",
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

    public static void AdvanceDice(SpawnObject spawnObject, JsonObject bonusJson) {
        spawnObject.AddObjectBonus(new JsonObject().LoadFromString($$"""
            {
                "field": "{{bonusJson.GetString("field")}}",
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

    public void AdvanceModifier(RPGLEffect rpglEffect, SpawnObject spawnObject, JsonObject bonusJson, RPGLContext context) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, spawnObject, bonusJson.GetJsonObject("object"));
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
            spawnObject.AddObjectBonus(new JsonObject().LoadFromString($$"""
            {
                "field": "{{bonusJson.GetString("field")}}",
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

    public void AdvanceAbility(RPGLEffect rpglEffect, SpawnObject spawnObject, JsonObject bonusJson, RPGLContext context) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, spawnObject, bonusJson.GetJsonObject("object"));
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
            spawnObject.AddObjectBonus(new JsonObject().LoadFromString($$"""
            {
                "field": "{{bonusJson.GetString("field")}}",
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

    public void AdvanceProficiency(RPGLEffect rpglEffect, SpawnObject spawnObject, JsonObject bonusJson, RPGLContext context) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, spawnObject, bonusJson.GetJsonObject("object"));
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
            spawnObject.AddObjectBonus(new JsonObject().LoadFromString($$"""
            {
                "field": "{{bonusJson.GetString("field")}}",
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

    public static void AdvanceLevel(RPGLEffect rpglEffect, SpawnObject spawnObject, JsonObject bonusJson) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, spawnObject, bonusJson.GetJsonObject("object"));
        string classDatapackId = bonusJson.GetString("class") ?? "*";

        spawnObject.AddObjectBonus(new JsonObject().LoadFromString($$"""
            {
                "field": "{{bonusJson.GetString("field")}}",
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
        if (subevent is SpawnObject spawnObject) {
            spawnObject.json.GetJsonArray("object_bonuses").AddJsonObject(functionJson.GetJsonObject("bonus"));
        }
    }

};
