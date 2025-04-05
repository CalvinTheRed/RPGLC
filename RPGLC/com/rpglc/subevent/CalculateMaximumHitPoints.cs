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
///   <b>Compatible Conditions</b>
///   <list type="bullet">
///     <item></item>
///   </list>
///   
///   <b>Compatible Functions</b>
///   <list type="bullet">
///     <item></item>
///   </list>
///   
/// </summary>
public class CalculateMaximumHitPoints : CalculationSubevent {

    public CalculateMaximumHitPoints() : base("calculate_maximum_hit_points") { }

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

    public override CalculateMaximumHitPoints? Invoke(RPGLContext context, JsonArray originPoint) {
        return (CalculateMaximumHitPoints?) base.Invoke(context, originPoint);
    }

    public override CalculateMaximumHitPoints JoinSubeventData(JsonObject other) {
        return (CalculateMaximumHitPoints) base.JoinSubeventData(other);
    }

    public override CalculateMaximumHitPoints Prepare(RPGLContext context, JsonArray originPoint) {
        base.Prepare(context, originPoint);
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
                            "formula": "range",
                            "dice": [ ],
                            "bonus": {{source.GetAbilityModifierFromAbilityName("con", context) * source.GetLevel()}}
                        }
                    ]
                }
                """),
            context,
            originPoint
        );

        return this;
    }

    public override CalculateMaximumHitPoints Run(RPGLContext context, JsonArray originPoint) {
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
