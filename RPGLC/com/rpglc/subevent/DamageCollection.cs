using com.rpglc.core;
using com.rpglc.data.TO;
using com.rpglc.json;
using com.rpglc.math;

namespace com.rpglc.subevent;

/// <summary>
///   Collects damage for a damaging subevent.
///   
///   <br /><br />
///   <i>This subevent is unavailable to be used directly inside an RPGLEvent.</i>
///   
///   <br /><br />
///   <i>Note that all damaging subevents will create two DamageCollection subevents. The first will have the "base" tag, and will represent damage that is applied to all targets of the damaging subevent. The second will have the "target" tag, and will represent damage that is only applied to a specific target of the damaging subevent.</i>
///   
///   <br /><br />
///   <b>Special Conditions</b>
///   <list type="bullet">
///     <item>IncludesDamageType</item>
///   </list>
///   
///   <b>Special Functions</b>
///   <list type="bullet">
///     <item>AddDamage</item>
///     <item>RepeatDamageDice</item>
///   </list>
///   
/// </summary>
public class DamageCollection : Subevent, IDamageTypeSubevent {

    int damageIndex = 0;

    public DamageCollection() : base("damage_collection") {
        subeventSteps.AddRange([
            (context) => {
                json.PutJsonArray("damage_formulas", json.RemoveJsonArray("damage") ?? new());
                json.PutJsonArray("damage", new());

                return new() {
                    dependency = dependency,
                    nextPhase = null,
                    stepCompleted = true,
                };
            },
            (context) => {
                JsonArray damageArray = json.GetJsonArray("damage_formulas") ?? new();
                if (damageIndex < damageArray.Count()) {
                    JsonObject damageJson = damageArray.GetJsonObject(damageIndex);
                    damageIndex++;

                    string? formula = damageJson.GetString("formula");
                    if (formula == "number") {
                        AdvanceNumber(damageJson);
                    } else if (formula == "dice") {
                        AdvanceDice(damageJson);
                    } else if (formula == "modifier") {
                        AdvanceModifier(damageJson, context);
                    } else if (formula == "ability") {
                        AdvanceAbility(damageJson, context);
                    } else if (formula == "proficiency") {
                        AdvanceProficiency(damageJson, context);
                    } else if (formula == "level") {
                        AdvanceLevel(damageJson);
                    }
                    return new() {
                        dependency = this.dependency,
                        stepCompleted = this.dependency is null && damageIndex == damageArray.Count(),
                    };
                }

                return new() {
                    dependency = dependency,
                    nextPhase = null,
                    stepCompleted = true,
                };
            },
        ]);
    }

    public override Subevent Clone() {
        Subevent clone = new DamageCollection();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new DamageCollection();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public void AdvanceNumber(JsonObject bonusJson) {
        AddDamage(new JsonObject().LoadFromString($$"""
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

    public void AdvanceDice(JsonObject bonusJson) {
        AddDamage(new JsonObject().LoadFromString($$"""
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

    public void AdvanceModifier(JsonObject bonusJson, RPGLContext context) {
        RPGLObject rpglObject = RPGLEffect.GetObject(null, this, bonusJson.GetJsonObject("object"));
        if (this.dependency is null) {
            this.dependency = new(new CalculateAbilityScore()
                .JoinSubeventData(new JsonObject().LoadFromString($$"""
                    {
                        "tags": {{rpglObject.GetTags()}},
                        "ability": "{{bonusJson.GetString("ability")}}"
                    }
                    """))
                .SetSource(rpglObject)
                .SetTarget(rpglObject));
            damageIndex--;
        } else {
            AddDamage(new JsonObject().LoadFromString($$"""
                {
                    "bonus": {{RPGLObject.GetAbilityModifierFromAbilityScore((dependency.subevent as CalculationSubevent).Get())}},
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

    public void AdvanceAbility(JsonObject bonusJson, RPGLContext context) {
        RPGLObject rpglObject = RPGLEffect.GetObject(null, this, bonusJson.GetJsonObject("object"));
        if (this.dependency is null) {
            this.dependency = new(new CalculateAbilityScore()
                .JoinSubeventData(new JsonObject().LoadFromString($$"""
                    {
                        "tags": {{rpglObject.GetTags()}},
                        "ability": "{{bonusJson.GetString("ability")}}"
                    }
                    """))
                .SetSource(rpglObject)
                .SetTarget(rpglObject));
            damageIndex--;
        } else {
            AddDamage(new JsonObject().LoadFromString($$"""
                {
                    "bonus": {{(dependency.subevent as CalculationSubevent).Get()}},
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

    public void AdvanceProficiency(JsonObject bonusJson, RPGLContext context) {
        RPGLObject rpglObject = RPGLEffect.GetObject(null, this, bonusJson.GetJsonObject("object"));
        if (this.dependency is null) {
            this.dependency = new(new CalculateProficiencyBonus()
                .JoinSubeventData(new JsonObject().LoadFromString($$"""
                    {
                        "tags": {{rpglObject.GetTags()}}
                    }
                    """))
                .SetSource(rpglObject)
                .SetTarget(rpglObject));
            damageIndex--;
        } else {
            AddDamage(new JsonObject().LoadFromString($$"""
                {
                    "bonus": {{(dependency.subevent as CalculationSubevent).Get()}},
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

    public void AdvanceLevel(JsonObject bonusJson) {
        RPGLObject rpglObject = RPGLEffect.GetObject(null, this, bonusJson.GetJsonObject("object"));
        string classDatapackId = bonusJson.GetString("class") ?? "*";

        AddDamage(new JsonObject().LoadFromString($$"""
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

    public override DamageCollection? Invoke(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return (DamageCollection?) base.Invoke(context, originPoint, invokingEffect);
    }

    public override DamageCollection JoinSubeventData(JsonObject other) {
        return (DamageCollection) base.JoinSubeventData(other);
    }

    public override DamageCollection Prepare(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        json.PutIfAbsent("damage", new JsonArray());
        PrepareDamage(context);
        return this;
    }

    public override Subevent Run(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return this;
    }

    public override DamageCollection SetOriginItem(string? originItem) {
        return (DamageCollection) base.SetOriginItem(originItem);
    }

    public override DamageCollection SetSource(RPGLObject source) {
        return (DamageCollection) base.SetSource(source);
    }

    public override DamageCollection SetTarget(RPGLObject target) {
        return (DamageCollection) base.SetTarget(target);
    }

    public bool IncludesDamageType(string damageType) {
        JsonArray damageDiceArray = GetDamageCollection();
        for (int i = 0; i < damageDiceArray.Count(); i++) {
            if (Equals(damageDiceArray.GetJsonObject(i).GetString("damage_type"), damageType)) {
                return true;
            }
        }
        return false;
    }

    private void PrepareDamage(RPGLContext context) {
        JsonArray damageArray = json.RemoveJsonArray("damage");
        json.PutJsonArray("damage", new());

        RPGLEffect rpglEffect = new();
        rpglEffect.SetSource(GetSource().GetUuid());
        rpglEffect.SetTarget(null);
        for (int i = 0; i < damageArray.Count(); i++) {
            JsonObject damageJson = damageArray.GetJsonObject(i);
            JsonObject damage = CalculationSubevent.SimplifyCalculationFormula(rpglEffect, this, damageJson, context);
            damage.PutString("damage_type", damageJson.GetString("damage_type"));
            AddDamage(damage);
        }
    }

    public DamageCollection AddDamage(JsonObject damageJson) {
        GetDamageCollection().AddJsonObject(damageJson);
        return this;
    }

    public JsonArray GetDamageCollection() {
        return json.GetJsonArray("damage");
    }

};
