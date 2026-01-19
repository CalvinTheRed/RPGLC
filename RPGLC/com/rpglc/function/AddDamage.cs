using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.math;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Adds a list of bonuses to a damage collection according to defined formulae.
///   
///   <code>
///   {
///     "function": "add_damage",
///     "bonus": [
///       {
///         &lt;bonus formula details&gt;
///         "damage_type": &lt;string | "*"&gt;
///       }
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

    public int bonusIndex = 0;

    public AddDamage() : base("add_damage") {
        functionSteps.AddRange([
            (rpglEffect, subevent, functionJson, context) => {
                if (subevent is DamageCollection damageCollection) {
                    JsonArray bonusArray = functionJson.GetJsonArray("bonus");
                    if (bonusIndex < bonusArray.Count()) {
                        JsonObject bonusJson = bonusArray.GetJsonObject(bonusIndex);
                        bonusIndex++;

                        string formula = bonusJson.GetString("formula");
                        if (formula == "number") {
                            AdvanceNumber(damageCollection, bonusJson);
                        } else if (formula == "dice") {
                            AdvanceDice(damageCollection, bonusJson);
                        } else if (formula == "modifier") {
                            AdvanceModifier(rpglEffect, damageCollection, bonusJson, context);
                        } else if (formula == "ability") {
                            AdvanceAbility(rpglEffect, damageCollection, bonusJson, context);
                        } else if (formula == "proficiency") {
                            AdvanceProficiency(rpglEffect, damageCollection, bonusJson, context);
                        } else if (formula == "level") {
                            AdvanceLevel(rpglEffect, damageCollection, bonusJson);
                        }
                        return new() {
                            dependency = this.dependency,
                            stepCompleted = this.dependency == null && bonusIndex == bonusArray.Count(),
                        };
                    }
                } else if (subevent is CriticalHitDamageCollection criticalHitDamageCollection) {
                    JsonArray bonusArray = functionJson.GetJsonArray("bonus");
                    if (bonusIndex < bonusArray.Count()) {
                        JsonObject bonusJson = bonusArray.GetJsonObject(bonusIndex);
                        bonusIndex++;

                        string formula = bonusJson.GetString("formula");
                        if (formula == "number") {
                            AdvanceNumber(criticalHitDamageCollection, bonusJson);
                        } else if (formula == "dice") {
                            AdvanceDice(criticalHitDamageCollection, bonusJson);
                        } else if (formula == "modifier") {
                            AdvanceModifier(rpglEffect, criticalHitDamageCollection, bonusJson, context);
                        } else if (formula == "ability") {
                            AdvanceAbility(rpglEffect, criticalHitDamageCollection, bonusJson, context);
                        } else if (formula == "proficiency") {
                            AdvanceProficiency(rpglEffect, criticalHitDamageCollection, bonusJson, context);
                        } else if (formula == "level") {
                            AdvanceLevel(rpglEffect, criticalHitDamageCollection, bonusJson);
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

    public override AddDamage Clone() {
        return new();
    }

    public static void AdvanceNumber(Subevent subevent, JsonObject damageJson) {
        string? damageType = damageJson.GetString("damage_type");

        if (subevent is DamageCollection damageCollection) {
            damageType ??= damageCollection.GetDamageCollection().GetJsonObject(0).GetString("damage_type");
            damageCollection.AddDamage(new JsonObject().LoadFromString($$"""
                {
                    "bonus": {{damageJson.GetLong("number")}},
                    "dice": [ ],
                    "damage_type": "{{damageType}}",
                    "scale": {{damageJson.GetJsonObject("scale")?.ToString() ?? $$"""
                    {
                        "numerator": 1,
                        "denominator": 1,
                        "round_up": false
                    }
                    """}}
                }
                """));
        } else if (subevent is CriticalHitDamageCollection criticalHitDamageCollection) {
            damageType ??= criticalHitDamageCollection.GetDamageCollection().GetJsonObject(0).GetString("damage_type");
            criticalHitDamageCollection.AddDamage(new JsonObject().LoadFromString($$"""
            {
                "bonus": {{damageJson.GetLong("number")}},
                "dice": [ ],
                "damage_type": "{{damageType}}",
                "scale": {{damageJson.GetJsonObject("scale")?.ToString() ?? $$"""
                {
                    "numerator": 1,
                    "denominator": 1,
                    "round_up": false
                }
                """}}
            }
            """));
        }
        
    }

    public static void AdvanceDice(Subevent subevent, JsonObject damageJson) {
        string? damageType = damageJson.GetString("damage_type");

        if (subevent is DamageCollection damageCollection) {
            damageType ??= damageCollection.GetDamageCollection().GetJsonObject(0).GetString("damage_type");
            damageCollection.AddDamage(new JsonObject().LoadFromString($$"""
                {
                    "bonus": 0,
                    "dice": {{Die.Unpack(damageJson.GetJsonArray("dice"))}},
                    "damage_type": "{{damageType}}",
                    "scale": {{damageJson.GetJsonObject("scale")?.ToString() ?? $$"""
                    {
                        "numerator": 1,
                        "denominator": 1,
                        "round_up": false
                    }
                    """}}
                }
                """));
        } else if (subevent is CriticalHitDamageCollection criticalHitDamageCollection) {
            damageType ??= criticalHitDamageCollection.GetDamageCollection().GetJsonObject(0).GetString("damage_type");
            criticalHitDamageCollection.AddDamage(new JsonObject().LoadFromString($$"""
                {
                    "bonus": 0,
                    "dice": {{Die.Unpack(damageJson.GetJsonArray("dice"))}},
                    "damage_type": "{{damageType}}",
                    "scale": {{damageJson.GetJsonObject("scale")?.ToString() ?? $$"""
                    {
                        "numerator": 1,
                        "denominator": 1,
                        "round_up": false
                    }
                    """}}
                }
                """));
        }
    }

    public void AdvanceModifier(RPGLEffect rpglEffect, Subevent subevent, JsonObject damageJson, RPGLContext context) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, subevent, damageJson.GetJsonObject("object"));
        string? damageType = damageJson.GetString("damage_type");

        if (this.dependency is null) {
            this.dependency = new CalculateAbilityScore()
                .JoinSubeventData(new JsonObject().LoadFromString($$"""
                    {
                        "tags": {{rpglObject.GetTags()}},
                        "ability": "{{damageJson.GetString("ability")}}"
                    }
                    """))
                .SetSource(rpglObject)
                .SetTarget(rpglObject);
            bonusIndex--;
        } else if (subevent is DamageCollection damageCollection) {
            damageType ??= damageCollection.GetDamageCollection().GetJsonObject(0).GetString("damage_type");
            damageCollection.AddDamage(new JsonObject().LoadFromString($$"""
                {
                    "bonus": {{RPGLObject.GetAbilityModifierFromAbilityScore((dependency as CalculationSubevent).Get())}},
                    "dice": [ ],
                    "damage_type": "{{damageType}}",
                    "scale": {{damageJson.GetJsonObject("scale")?.ToString() ?? $$"""
                    {
                        "numerator": 1,
                        "denominator": 1,
                        "round_up": false
                    }
                    """}}
                }
                """));
            dependency = null;
        } else if (subevent is CriticalHitDamageCollection criticalHitDamageCollection) {
            damageType ??= criticalHitDamageCollection.GetDamageCollection().GetJsonObject(0).GetString("damage_type");
            criticalHitDamageCollection.AddDamage(new JsonObject().LoadFromString($$"""
                {
                    "bonus": {{RPGLObject.GetAbilityModifierFromAbilityScore((dependency as CalculationSubevent).Get())}},
                    "dice": [ ],
                    "damage_type": "{{damageType}}",
                    "scale": {{damageJson.GetJsonObject("scale")?.ToString() ?? $$"""
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

    public void AdvanceAbility(RPGLEffect rpglEffect, Subevent subevent, JsonObject damageJson, RPGLContext context) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, subevent, damageJson.GetJsonObject("object"));
        string? damageType = damageJson.GetString("damage_type");

        if (this.dependency is null) {
            this.dependency = new CalculateAbilityScore()
                .JoinSubeventData(new JsonObject().LoadFromString($$"""
                    {
                        "tags": {{rpglObject.GetTags()}},
                        "ability": "{{damageJson.GetString("ability")}}"
                    }
                    """))
                .SetSource(rpglObject)
                .SetTarget(rpglObject);
            bonusIndex--;
        } else if (subevent is DamageCollection damageCollection) {
            damageType ??= damageCollection.GetDamageCollection().GetJsonObject(0).GetString("damage_type");
            damageCollection.AddDamage(new JsonObject().LoadFromString($$"""
                {
                    "bonus": {{(dependency as CalculationSubevent).Get()}},
                    "dice": [ ],
                    "damage_type": "{{damageType}}",
                    "scale": {{damageJson.GetJsonObject("scale")?.ToString() ?? $$"""
                    {
                        "numerator": 1,
                        "denominator": 1,
                        "round_up": false
                    }
                    """}}
                }
                """));
            dependency = null;
        } else if (subevent is CriticalHitDamageCollection criticalHitDamageCollection) {
            damageType ??= criticalHitDamageCollection.GetDamageCollection().GetJsonObject(0).GetString("damage_type");
            criticalHitDamageCollection.AddDamage(new JsonObject().LoadFromString($$"""
                {
                    "bonus": {{(dependency as CalculationSubevent).Get()}},
                    "dice": [ ],
                    "damage_type": "{{damageType}}",
                    "scale": {{damageJson.GetJsonObject("scale")?.ToString() ?? $$"""
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

    public void AdvanceProficiency(RPGLEffect rpglEffect, Subevent subevent, JsonObject damageJson, RPGLContext context) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, subevent, damageJson.GetJsonObject("object"));
        string? damageType = damageJson.GetString("damage_type");

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
        } else if (subevent is DamageCollection damageCollection) {
            damageType ??= damageCollection.GetDamageCollection().GetJsonObject(0).GetString("damage_type");
            damageCollection.AddDamage(new JsonObject().LoadFromString($$"""
                {
                    "bonus": {{(dependency as CalculationSubevent).Get()}},
                    "dice": [ ],
                    "damage_type": "{{damageType}}",
                    "scale": {{damageJson.GetJsonObject("scale")?.ToString() ?? $$"""
                    {
                        "numerator": 1,
                        "denominator": 1,
                        "round_up": false
                    }
                    """}}
                }
                """));
            dependency = null;
        } else if (subevent is CriticalHitDamageCollection criticalHitDamageCollection) {
            damageType ??= criticalHitDamageCollection.GetDamageCollection().GetJsonObject(0).GetString("damage_type");
            criticalHitDamageCollection.AddDamage(new JsonObject().LoadFromString($$"""
                {
                    "bonus": {{(dependency as CalculationSubevent).Get()}},
                    "dice": [ ],
                    "damage_type": "{{damageType}}",
                    "scale": {{damageJson.GetJsonObject("scale")?.ToString() ?? $$"""
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

    public static void AdvanceLevel(RPGLEffect rpglEffect, Subevent subevent, JsonObject damageJson) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, subevent, damageJson.GetJsonObject("object"));
        string classDatapackId = damageJson.GetString("class") ?? "*";
        string? damageType = damageJson.GetString("damage_type");

        if (subevent is DamageCollection damageCollection) {
            damageType ??= damageCollection.GetDamageCollection().GetJsonObject(0).GetString("damage_type");
            damageCollection.AddDamage(new JsonObject().LoadFromString($$"""
                {
                    "bonus": {{(classDatapackId == "*" ? rpglObject.GetLevel() : rpglObject.GetLevel(classDatapackId))}},
                    "dice": [ ],
                    "damage_type": "{{damageType}}",
                    "scale": {{damageJson.GetJsonObject("scale")?.ToString() ?? $$"""
                    {
                        "numerator": 1,
                        "denominator": 1,
                        "round_up": false
                    }
                    """}}
                }
                """));
        } else if (subevent is CriticalHitDamageCollection criticalHitDamageCollection) {
            damageType ??= criticalHitDamageCollection.GetDamageCollection().GetJsonObject(0).GetString("damage_type");
            criticalHitDamageCollection.AddDamage(new JsonObject().LoadFromString($$"""
                {
                    "bonus": {{(classDatapackId == "*" ? rpglObject.GetLevel() : rpglObject.GetLevel(classDatapackId))}},
                    "dice": [ ],
                    "damage_type": "{{damageType}}",
                    "scale": {{damageJson.GetJsonObject("scale")?.ToString() ?? $$"""
                    {
                        "numerator": 1,
                        "denominator": 1,
                        "round_up": false
                    }
                    """}}
                }
                """));
        }
    }

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
