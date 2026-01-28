using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

/// <summary>
///   Calculates the threshold at which point an attack will become a critical hit.
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
public class CalculateCriticalHitThreshold : CalculationSubevent {

    public CalculateCriticalHitThreshold() : base("calculate_critical_hit_threshold") {
        subeventSteps.Insert(0, (context) => {
            json.PutIfAbsent("base", new JsonObject().LoadFromString("""
                {
                    "formula": "number",
                    "number": 20
                }
                """));
            
            return new() {
                dependency = null,
                nextPhase = null,
                stepCompleted = true,
            };
        });
    }

    public override Subevent Clone() {
        Subevent clone = new CalculateCriticalHitThreshold();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new CalculateCriticalHitThreshold();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override CalculateCriticalHitThreshold? Invoke(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return (CalculateCriticalHitThreshold?) base.Invoke(context, originPoint, invokingEffect);
    }

    public override CalculateCriticalHitThreshold JoinSubeventData(JsonObject other) {
        return (CalculateCriticalHitThreshold) base.JoinSubeventData(other);
    }

    public override CalculateCriticalHitThreshold Prepare(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        base.Prepare(context, originPoint, invokingEffect);
        json.PutIfAbsent("critical_hit_threshold", 20L);
        return this;
    }

    public override CalculateCriticalHitThreshold Run(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        SetBase((long) json.GetLong("critical_hit_threshold"));
        return this;
    }

    public override CalculateCriticalHitThreshold SetOriginItem(string? originItem) {
        return (CalculateCriticalHitThreshold) base.SetOriginItem(originItem);
    }

    public override CalculateCriticalHitThreshold SetSource(RPGLObject source) {
        return (CalculateCriticalHitThreshold) base.SetSource(source);
    }

    public override CalculateCriticalHitThreshold SetTarget(RPGLObject target) {
        return (CalculateCriticalHitThreshold) base.SetTarget(target);
    }

};
