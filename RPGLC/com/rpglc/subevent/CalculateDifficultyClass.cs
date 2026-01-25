using com.rpglc.core;
using com.rpglc.function;
using com.rpglc.json;

namespace com.rpglc.subevent;

/// <summary>
///   Calculates the difficulty class for a save-type subevent.
///   
///   <br /><br />
///   <i>This subevent is unavailable to be used directly inside an RPGLEvent.</i>
///   
///   <br /><br />
///   <b>Special Functions</b>
///   <list type="bullet">
///     <item>AddBonus</item>
///     <item>SetBase</item>
///     <item>SetMinimum</item>
///   </list>
///   
/// </summary>
public class CalculateDifficultyClass : CalculationSubevent {

    public CalculateDifficultyClass() : base("calculate_difficulty_class") {
        subeventSteps.AddRange([
            (context) => {
                if (json.AsDict().ContainsKey("difficulty_class")) {
                    AddAssignedDifficultyClassSteps();
                } else {
                    AddCalculatedDifficultyClassSteps();
                }
                return new() {
                    dependency = null,
                    nextPhase = null,
                    stepCompleted = true,
                };
            },
        ]);
    }

    private void AddAssignedDifficultyClassSteps() {
        subeventSteps.AddRange([
            (context) => {
                SetBase((long) json.GetLong("difficulty_class"));
                return new() {
                    dependency = null,
                    nextPhase = null,
                    stepCompleted = true,
                };
            },
        ]);
    }

    private void AddCalculatedDifficultyClassSteps() {
        subeventSteps.AddRange([
            (context) => {
                SetBase(8L);

                RPGLObject rpglObject = GetTarget();
                dependency = new(new CalculateAbilityScore()
                    .SetSource(rpglObject)
                    .SetTarget(rpglObject)
                    .JoinSubeventData(new JsonObject().LoadFromString($$"""
                        {
                            "subevent": "calculate_ability_score",
                            "object": {
                                "from": "subevent",
                                "object": "target"
                            },
                            "ability": "{{json.GetString("difficulty_class_ability")}}"
                        }
                        """)));

                return new() {
                    dependency = dependency,
                    nextPhase = null,
                    stepCompleted = true,
                };
            },
            (context) => {
                AddBonus(new JsonObject().LoadFromString($$"""
                    {
                        "bonus": {{RPGLObject.GetAbilityModifierFromAbilityScore((dependency.subevent as CalculateAbilityScore).Get())}},
                        "dice": [ ],
                        "scale": {
                            "numerator": 1,
                            "denominator": 1,
                            "round_up": false
                        }
                    }
                    """));

                RPGLObject rpglObject = GetTarget();
                dependency = new(new CalculateProficiencyBonus()
                    .SetSource(rpglObject)
                    .SetTarget(rpglObject)
                    .JoinSubeventData(new JsonObject().LoadFromString("""
                        {
                            "subevent": "calculate_proficiency_bonus",
                            "object": {
                                "from": "subevent",
                                "object": "target"
                            }
                        }
                        """)));

                return new() {
                    dependency = dependency,
                    nextPhase = null,
                    stepCompleted = true,
                };
            },
            (context) => {
                AddBonus(new JsonObject().LoadFromString($$"""
                    {
                        "bonus": {{(dependency.subevent as CalculateProficiencyBonus).Get()}},
                        "dice": [ ],
                        "scale": {
                            "numerator": 1,
                            "denominator": 1,
                            "round_up": false
                        }
                    }
                    """));

                dependency = null;

                return new() {
                    dependency = dependency,
                    nextPhase = null,
                    stepCompleted = true,
                };
            },
        ]);
    }
    
    public override Subevent Clone() {
        Subevent clone = new CalculateDifficultyClass();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new CalculateDifficultyClass();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override CalculateDifficultyClass? Invoke(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return (CalculateDifficultyClass?) base.Invoke(context, originPoint, invokingEffect);
    }

    public override CalculateDifficultyClass JoinSubeventData(JsonObject other) {
        return (CalculateDifficultyClass) base.JoinSubeventData(other);
    }

    public override CalculateDifficultyClass Prepare(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        base.Prepare(context, originPoint, invokingEffect);
        long? difficultyClass = json.GetLong("difficulty_class");
        if (difficultyClass is null) {
            SetBase(8L);
            new AddBonus().Execute(
                null,
                this,
                new JsonObject().LoadFromString($$"""
                    {
                        "function": "add_bonus",
                        "bonus": [
                            {
                                "formula": "proficiency",
                                "object": {
                                    "from": "subevent",
                                    "object": "source",
                                    "as_origin": false
                                }
                            },
                            {
                                "formula": "modifier",
                                "ability": "{{json.GetString("difficulty_class_ability")}}",
                                "object": {
                                    "from": "subevent",
                                    "object": "source",
                                    "as_origin": false
                                }
                            }
                        ]
                    }
                    """),
                context,
                originPoint
            );
        } else {
            SetBase((long) difficultyClass);
        }
        return this;
    }

    public override CalculateDifficultyClass Run(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return this;
    }

    public override CalculateDifficultyClass SetOriginItem(string? originItem) {
        return (CalculateDifficultyClass) base.SetOriginItem(originItem);
    }

    public override CalculateDifficultyClass SetSource(RPGLObject source) {
        return (CalculateDifficultyClass) base.SetSource(source);
    }

    public override CalculateDifficultyClass SetTarget(RPGLObject target) {
        return (CalculateDifficultyClass) base.SetTarget(target);
    }

};
