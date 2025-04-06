using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

/// <summary>
///   Calculates the threshhold at which point an attack will become a critical hit.
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
public class CalculateCriticalHitThreshhold : CalculationSubevent {

    public CalculateCriticalHitThreshhold() : base("calculate_critical_hit_threshhold") { }

    public override Subevent Clone() {
        Subevent clone = new CalculateCriticalHitThreshhold();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new CalculateCriticalHitThreshhold();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override CalculateCriticalHitThreshhold? Invoke(RPGLContext context, JsonArray originPoint) {
        return (CalculateCriticalHitThreshhold?) base.Invoke(context, originPoint);
    }

    public override CalculateCriticalHitThreshhold JoinSubeventData(JsonObject other) {
        return (CalculateCriticalHitThreshhold) base.JoinSubeventData(other);
    }

    public override CalculateCriticalHitThreshhold Prepare(RPGLContext context, JsonArray originPoint) {
        base.Prepare(context, originPoint);
        SetBase(20L);
        return this;
    }

    public override CalculateCriticalHitThreshhold Run(RPGLContext context, JsonArray originPoint) {
        return this;
    }

    public override CalculateCriticalHitThreshhold SetOriginItem(string? originItem) {
        return (CalculateCriticalHitThreshhold) base.SetOriginItem(originItem);
    }

    public override CalculateCriticalHitThreshhold SetSource(RPGLObject source) {
        return (CalculateCriticalHitThreshhold) base.SetSource(source);
    }

    public override CalculateCriticalHitThreshhold SetTarget(RPGLObject target) {
        return (CalculateCriticalHitThreshhold) base.SetTarget(target);
    }

};
