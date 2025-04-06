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
    
    public CalculateDifficultyClass() : base("calculate_difficulty_class") { }

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

    public override CalculateDifficultyClass? Invoke(RPGLContext context, JsonArray originPoint) {
        return (CalculateDifficultyClass?) base.Invoke(context, originPoint);
    }

    public override CalculateDifficultyClass JoinSubeventData(JsonObject other) {
        return (CalculateDifficultyClass) base.JoinSubeventData(other);
    }

    public override CalculateDifficultyClass Prepare(RPGLContext context, JsonArray originPoint) {
        base.Prepare(context, originPoint);
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
                                    "object": "source"
                                }
                            },
                            {
                                "formula": "modifier",
                                "ability": "{{json.GetString("difficulty_class_ability")}}",
                                "object": {
                                    "from": "subevent",
                                    "object": "source"
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

    public override CalculateDifficultyClass Run(RPGLContext context, JsonArray originPoint) {
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
