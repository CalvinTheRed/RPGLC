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

    public int damageIndex = 0;

    public AddDamage() : base("add_damage") {
        functionSteps.AddRange([
            (RPGLEffect rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context) => {
                if (subevent is DamageCollection damageCollection) {
                    JsonArray damageArray = functionJson.GetJsonArray("bonus");
                    if (damageIndex < damageArray.Count()) {
                        JsonObject damageJson = damageArray.GetJsonObject(damageIndex);
                        damageIndex++;

                        string formula = damageJson.GetString("formula");
                        if (formula == "number") {
                            AdvanceNumber(damageCollection, damageJson);
                        } else if (formula == "dice") {
                            AdvanceDice(damageCollection, damageJson);
                        } else if (formula == "modifier") {
                            AdvanceModifier(rpglEffect, damageCollection, damageJson, context);
                        } else if (formula == "ability") {
                            AdvanceAbility(rpglEffect, damageCollection, damageJson, context);
                        } else if (formula == "proficiency") {
                            AdvanceProficiency(rpglEffect, damageCollection, damageJson, context);
                        } else if (formula == "level") {
                            AdvanceLevel(rpglEffect, damageCollection, damageJson);
                        }
                        return new() {
                            dependency = this.dependency,
                            completed = false,
                        };
                    }
                } else if (subevent is CriticalHitDamageCollection criticalHitDamageCollection) {
                    JsonArray damageArray = functionJson.GetJsonArray("bonus");
                    if (damageIndex < damageArray.Count()) {
                        JsonObject damageJson = damageArray.GetJsonObject(damageIndex);
                        damageIndex++;

                        string formula = damageJson.GetString("formula");
                        if (formula == "number") {
                            AdvanceNumber(criticalHitDamageCollection, damageJson);
                        } else if (formula == "dice") {
                            AdvanceDice(criticalHitDamageCollection, damageJson);
                        } else if (formula == "modifier") {
                            AdvanceModifier(rpglEffect, criticalHitDamageCollection, damageJson, context);
                        } else if (formula == "ability") {
                            AdvanceAbility(rpglEffect, criticalHitDamageCollection, damageJson, context);
                        } else if (formula == "proficiency") {
                            AdvanceProficiency(rpglEffect, criticalHitDamageCollection, damageJson, context);
                        } else if (formula == "level") {
                            AdvanceLevel(rpglEffect, criticalHitDamageCollection, damageJson);
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
                .JoinSubeventData(new JsonObject()
                    .PutJsonArray("tags", rpglObject.GetTags().DeepClone())
                    .PutString("ability", damageJson.GetString("ability"))
                )
                .SetSource(rpglObject)
                .SetTarget(rpglObject);
            damageIndex--;
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
                .JoinSubeventData(new JsonObject()
                    .PutJsonArray("tags", rpglObject.GetTags().DeepClone())
                    .PutString("ability", damageJson.GetString("ability"))
                )
                .SetSource(rpglObject)
                .SetTarget(rpglObject);
            damageIndex--;
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
                .JoinSubeventData(new JsonObject()
                    .PutJsonArray("tags", rpglObject.GetTags().DeepClone())
                )
                .SetSource(rpglObject)
                .SetTarget(rpglObject);
            damageIndex--;
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
