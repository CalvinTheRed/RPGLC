using com.rpglc.core;
using com.rpglc.function;
using com.rpglc.json;

namespace com.rpglc.subevent;

/// <summary>
///   Calculates the maximum hit points of an object.
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
public class CalculateMaximumHitPoints : CalculationSubevent {

    public CalculateMaximumHitPoints() : base("calculate_maximum_hit_points") {
        subeventSteps.AddRange([
            (context) => {
                RPGLObject rpglObject = GetTarget();
                
                SetBase(rpglObject.GetHealthBase());

                dependency = new(new CalculateAbilityScore()
                    .SetOriginItem(GetOriginItem())
                    .SetSource(rpglObject)
                    .SetTarget(rpglObject)
                    .JoinSubeventData(new JsonObject().LoadFromString("""
                        {
                            "object": {
                                "from": "subevent",
                                "object": "target"
                            },
                            "ability": "con"
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
                            "numerator": {{GetTarget().GetLevel()}},
                            "denominator": 1,
                            "round_up": false
                        }
                    }
                    """));

                dependency = null;

                return new() {
                    dependency = null,
                    nextPhase = null,
                    stepCompleted = true,
                };
            },
        ]);
    }

    public override Subevent Clone() {
        Subevent clone = new CalculateMaximumHitPoints();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new CalculateMaximumHitPoints();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override CalculateMaximumHitPoints? Invoke(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return (CalculateMaximumHitPoints?) base.Invoke(context, originPoint, invokingEffect);
    }

    public override CalculateMaximumHitPoints JoinSubeventData(JsonObject other) {
        return (CalculateMaximumHitPoints) base.JoinSubeventData(other);
    }

    public override CalculateMaximumHitPoints Prepare(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        base.Prepare(context, originPoint, invokingEffect);
        RPGLObject source = GetSource();

        SetBase(source.GetHealthBase());

        new AddBonus().Execute(
            null,
            this,
            new JsonObject().LoadFromString($$"""
                {
                    "function": "add_bonus",
                    "bonus": [
                        {
                            "formula": "number",
                            "number": {{source.GetAbilityModifierFromAbilityName("con", context) * source.GetLevel()}}
                        }
                    ]
                }
                """),
            context,
            originPoint
        );

        return this;
    }

    public override CalculateMaximumHitPoints Run(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return this;
    }

    public override CalculateMaximumHitPoints SetOriginItem(string? originItem) {
        return (CalculateMaximumHitPoints) base.SetOriginItem(originItem);
    }

    public override CalculateMaximumHitPoints SetSource(RPGLObject source) {
        return (CalculateMaximumHitPoints) base.SetSource(source);
    }

    public override CalculateMaximumHitPoints SetTarget(RPGLObject target) {
        return (CalculateMaximumHitPoints) base.SetTarget(target);
    }

};
