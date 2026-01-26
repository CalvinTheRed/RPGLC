using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.math;

namespace com.rpglc.subevent;

/// <summary>
///   Collects healing for a healing subevent.
///   
///   <br /><br />
///   <i>This subevent is unavailable to be used directly inside an RPGLEvent.</i>
///   
///   <br /><br />
///   <i>Note that all healing subevents will create two HealingCollection subevents. The first will have the "base" tag, and will represent healing that is applied to all targets of the healing subevent. The second will have the "target" tag, and will represent healing that is only applied to a specific target of the healing subevent.</i>
///   
///   <br /><br />
///   <b>Special Functions</b>
///   <list type="bullet">
///     <item>AddHealing</item>
///   </list>
///   
/// </summary>
public class HealingCollection : Subevent {

    private int healingIndex = 0;

    public HealingCollection() : base("healing_collection") {
        subeventSteps.AddRange([
            (context) => {
                json.PutJsonArray("healing_formulas", json.RemoveJsonArray("healing") ?? new());
                json.PutJsonArray("healing", new());

                return new() {
                    dependency = dependency,
                    nextPhase = null,
                    stepCompleted = true,
                };
            },
            (context) => {
                JsonArray healingArray = json.GetJsonArray("healing_formulas") ?? new();
                if (healingIndex < healingArray.Count()) {
                    JsonObject healingJson = healingArray.GetJsonObject(healingIndex);
                    healingIndex++;

                    string? formula = healingJson.GetString("formula");
                    if (formula == "number") {
                        AdvanceNumber(healingJson);
                    } else if (formula == "dice") {
                        AdvanceDice(healingJson);
                    } else if (formula == "modifier") {
                        AdvanceModifier(healingJson, context);
                    } else if (formula == "ability") {
                        AdvanceAbility(healingJson, context);
                    } else if (formula == "proficiency") {
                        AdvanceProficiency(healingJson, context);
                    } else if (formula == "level") {
                        AdvanceLevel(healingJson);
                    }
                    return new() {
                        dependency = this.dependency,
                        stepCompleted = this.dependency is null && healingIndex == healingArray.Count(),
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
        Subevent clone = new HealingCollection();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new HealingCollection();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public void AdvanceNumber(JsonObject bonusJson) {
        AddHealing(new JsonObject().LoadFromString($$"""
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
        AddHealing(new JsonObject().LoadFromString($$"""
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
                .SetOriginItem(GetOriginItem())
                .SetSource(rpglObject)
                .SetTarget(rpglObject)
                .JoinSubeventData(new JsonObject().LoadFromString($$"""
                    {
                        "tags": {{rpglObject.GetTags()}},
                        "ability": "{{bonusJson.GetString("ability")}}"
                    }
                    """)));
            healingIndex--;
        } else {
            AddHealing(new JsonObject().LoadFromString($$"""
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
                .SetOriginItem(GetOriginItem())
                .SetSource(rpglObject)
                .SetTarget(rpglObject)
                .JoinSubeventData(new JsonObject().LoadFromString($$"""
                    {
                        "tags": {{rpglObject.GetTags()}},
                        "ability": "{{bonusJson.GetString("ability")}}"
                    }
                    """)));
            healingIndex--;
        } else {
            AddHealing(new JsonObject().LoadFromString($$"""
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
                .SetOriginItem(GetOriginItem())
                .SetSource(rpglObject)
                .SetTarget(rpglObject)
                .JoinSubeventData(new JsonObject().LoadFromString($$"""
                    {
                        "tags": {{rpglObject.GetTags()}}
                    }
                    """)));
            healingIndex--;
        } else {
            AddHealing(new JsonObject().LoadFromString($$"""
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

        AddHealing(new JsonObject().LoadFromString($$"""
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

    public override HealingCollection? Invoke(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return (HealingCollection?) base.Invoke(context, originPoint, invokingEffect);
    }

    public override HealingCollection JoinSubeventData(JsonObject other) {
        return (HealingCollection) base.JoinSubeventData(other);
    }

    public override HealingCollection Prepare(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        json.PutIfAbsent("healing", new JsonArray());
        PrepareHealing(context);
        return this;
    }

    public override HealingCollection Run(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return this;
    }

    public override HealingCollection SetOriginItem(string? originItem) {
        return (HealingCollection) base.SetOriginItem(originItem);
    }

    public override HealingCollection SetSource(RPGLObject source) {
        return (HealingCollection) base.SetSource(source);
    }

    public override HealingCollection SetTarget(RPGLObject target) {
        return (HealingCollection) base.SetTarget(target);
    }

    private void PrepareHealing(RPGLContext context) {
        JsonArray healingArray = json.RemoveJsonArray("healing");
        json.PutJsonArray("healing", new());

        RPGLEffect rpglEffect = new();
        rpglEffect.SetSource(GetSource().GetUuid());
        rpglEffect.SetTarget(null);
        for (int i = 0; i < healingArray.Count(); i++) {
            AddHealing(CalculationSubevent.SimplifyCalculationFormula(
                rpglEffect,
                this,
                healingArray.GetJsonObject(i),
                context
            ));
        }
    }

    public HealingCollection AddHealing(JsonObject healingJson) {
        GetHealingCollection().AddJsonObject(healingJson);
        return this;
    }

    public JsonArray GetHealingCollection() {
        return json.GetJsonArray("healing");
    }

};
