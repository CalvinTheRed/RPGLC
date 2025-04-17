using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

/// <summary>
///   Calculates the proficiency bonus of an object.
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
public class CalculateProficiencyBonus : CalculationSubevent {
    
    public CalculateProficiencyBonus() : base("calculate_proficiency_bonus") { }

    public override Subevent Clone() {
        Subevent clone = new CalculateProficiencyBonus();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new CalculateProficiencyBonus();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override CalculateProficiencyBonus? Invoke(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return (CalculateProficiencyBonus?) base.Invoke(context, originPoint, invokingEffect);
    }

    public override CalculateProficiencyBonus JoinSubeventData(JsonObject other) {
        return (CalculateProficiencyBonus) base.JoinSubeventData(other);
    }

    public override CalculateProficiencyBonus Prepare(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        base.Prepare(context, originPoint, invokingEffect)
            .SetBase(GetSource().GetProficiencyBonus() ?? GetSource().GetProficiencyBonusByLevel());
        return this;
    }

    public override CalculateProficiencyBonus Run(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return this;
    }

    public override CalculateProficiencyBonus SetOriginItem(string? originItem) {
        return (CalculateProficiencyBonus) base.SetOriginItem(originItem);
    }

    public override CalculateProficiencyBonus SetSource(RPGLObject source) {
        return (CalculateProficiencyBonus) base.SetSource(source);
    }

    public override CalculateProficiencyBonus SetTarget(RPGLObject target) {
        return (CalculateProficiencyBonus) base.SetTarget(target);
    }

};
