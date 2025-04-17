using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

/// <summary>
///   Calculates the value of an object's ability score.
///   
///   <br /><br />
///   <i>This subevent is unavailable to be used directly inside an RPGLEvent.</i>
///   
///   <br /><br />
///   <b>Special Conditions</b>
///   <list type="bullet">
///     <item>CheckAbility</item>
///   </list>
///   
///   <b>Special Functions</b>
///   <list type="bullet">
///     <item>AddBonus</item>
///     <item>SetBase</item>
///     <item>SetMinimum</item>
///   </list>
///   
/// </summary>
public class CalculateAbilityScore : CalculationSubevent, IAbilitySubevent {
    
    public CalculateAbilityScore() : base("calculate_ability_score") { }

    public override Subevent Clone() {
        Subevent clone = new CalculateAbilityScore();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new CalculateAbilityScore();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override CalculateAbilityScore? Invoke(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return (CalculateAbilityScore?) base.Invoke(context, originPoint, invokingEffect);
    }

    public override CalculateAbilityScore JoinSubeventData(JsonObject other) {
        return (CalculateAbilityScore) base.JoinSubeventData(other);
    }

    public override CalculateAbilityScore Prepare(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        base.Prepare(context, originPoint, invokingEffect);
        SetBase((long) GetSource().GetAbilityScores().GetLong(GetAbility(context)));
        return this;
    }

    public override CalculateAbilityScore Run(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return this;
    }

    public override CalculateAbilityScore SetOriginItem(string? originItem) {
        return (CalculateAbilityScore) base.SetOriginItem(originItem);
    }

    public override CalculateAbilityScore SetSource(RPGLObject source) {
        return (CalculateAbilityScore) base.SetSource(source);
    }

    public override CalculateAbilityScore SetTarget(RPGLObject target) {
        return (CalculateAbilityScore) base.SetTarget(target);
    }

    public string GetAbility(RPGLContext context) {
        return json.GetString("ability");
    }

};
