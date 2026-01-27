using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.math;

namespace com.rpglc.subevent;

/// <summary>
///   Collects temporary hit points for a temporary hit point subevent.
///   
///   <br /><br />
///   <i>This subevent is unavailable to be used directly inside an RPGLEvent.</i>
///   
///   <br /><br />
///   <i>Note that all temporary hit point subevents will create two TemporaryHitPointCollection subevents. The first will have the "base" tag, and will represent temporary hit points that are applied to all targets of the temporary hit point subevent. The second will have the "target" tag, and will represent temporary hit points that are only applied to a specific target of the temporary hit point subevent.</i>
///   
///   <br /><br />
///   <b>Special Functions</b>
///   <list type="bullet">
///     <item>AddTemporaryHitPoints</item>
///   </list>
///   
/// </summary>
public class TemporaryHitPointCollection : Subevent {

    private int temporaryHitPointIndex = 0;

    public TemporaryHitPointCollection() : base("temporary_hit_point_collection") {
        subeventSteps.AddRange([
            (context) => {
                json.PutJsonArray("temporary_hit_point_formulas", json.RemoveJsonArray("temporary_hit_points") ?? new());
                json.PutJsonArray("temporary_hit_points", new());

                return new() {
                    dependency = dependency,
                    nextPhase = null,
                    stepCompleted = true,
                };
            },
            (context) => {
                JsonArray temporaryHitPointArray = json.GetJsonArray("temporary_hit_point_formulas") ?? new();
                if (temporaryHitPointIndex < temporaryHitPointArray.Count()) {
                    JsonObject temporaryHitPointJson = temporaryHitPointArray.GetJsonObject(temporaryHitPointIndex);
                    temporaryHitPointIndex++;

                    string? formula = temporaryHitPointJson.GetString("formula");
                    if (formula == "number") {
                        AdvanceNumber(temporaryHitPointJson);
                    } else if (formula == "dice") {
                        AdvanceDice(temporaryHitPointJson);
                    } else if (formula == "modifier") {
                        AdvanceModifier(temporaryHitPointJson, context);
                    } else if (formula == "ability") {
                        AdvanceAbility(temporaryHitPointJson, context);
                    } else if (formula == "proficiency") {
                        AdvanceProficiency(temporaryHitPointJson, context);
                    } else if (formula == "level") {
                        AdvanceLevel(temporaryHitPointJson);
                    }
                    return new() {
                        dependency = this.dependency,
                        stepCompleted = this.dependency is null && temporaryHitPointIndex == temporaryHitPointArray.Count(),
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
        Subevent clone = new TemporaryHitPointCollection();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new TemporaryHitPointCollection();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public void AdvanceNumber(JsonObject bonusJson) {
        AddTemporaryHitPoints(new JsonObject().LoadFromString($$"""
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
        AddTemporaryHitPoints(new JsonObject().LoadFromString($$"""
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
            temporaryHitPointIndex--;
        } else {
            AddTemporaryHitPoints(new JsonObject().LoadFromString($$"""
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
            temporaryHitPointIndex--;
        } else {
            AddTemporaryHitPoints(new JsonObject().LoadFromString($$"""
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
            temporaryHitPointIndex--;
        } else {
            AddTemporaryHitPoints(new JsonObject().LoadFromString($$"""
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

        AddTemporaryHitPoints(new JsonObject().LoadFromString($$"""
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

    public override TemporaryHitPointCollection? Invoke(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return (TemporaryHitPointCollection?) base.Invoke(context, originPoint, invokingEffect);
    }

    public override TemporaryHitPointCollection JoinSubeventData(JsonObject other) {
        return (TemporaryHitPointCollection) base.JoinSubeventData(other);
    }

    public override TemporaryHitPointCollection Prepare(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        json.PutIfAbsent("temporary_hit_points", new JsonArray());
        PrepareTemporaryHitPoints(context);
        return this;
    }

    public override TemporaryHitPointCollection Run(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return this;
    }

    public override TemporaryHitPointCollection SetOriginItem(string? originItem) {
        return (TemporaryHitPointCollection) base.SetOriginItem(originItem);
    }

    public override TemporaryHitPointCollection SetSource(RPGLObject source) {
        return (TemporaryHitPointCollection) base.SetSource(source);
    }

    public override TemporaryHitPointCollection SetTarget(RPGLObject target) {
        return (TemporaryHitPointCollection) base.SetTarget(target);
    }

    private void PrepareTemporaryHitPoints(RPGLContext context) {
        JsonArray temporaryHitPointArray = json.RemoveJsonArray("temporary_hit_points");
        json.PutJsonArray("temporary_hit_points", new());

        RPGLEffect rpglEffect = new();
        rpglEffect.SetSource(GetSource().GetUuid());
        rpglEffect.SetTarget(null);
        for (int i = 0; i < temporaryHitPointArray.Count(); i++) {
            AddTemporaryHitPoints(CalculationSubevent.SimplifyCalculationFormula(
                rpglEffect,
                this,
                temporaryHitPointArray.GetJsonObject(i),
                context
            ));
        }
    }

    public TemporaryHitPointCollection AddTemporaryHitPoints(JsonObject temporaryHitPointJson) {
        GetTemporaryHitPointCollection().AddJsonObject(temporaryHitPointJson);
        return this;
    }

    public JsonArray GetTemporaryHitPointCollection() {
        return json.GetJsonArray("temporary_hit_points");
    }

};
