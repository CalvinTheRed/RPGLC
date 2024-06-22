using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

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

    public override CalculateAbilityScore? Invoke(RPGLContext context, JsonArray originPoint) {
        return (CalculateAbilityScore?) base.Invoke(context, originPoint);
    }

    public override CalculateAbilityScore JoinSubeventData(JsonObject other) {
        return (CalculateAbilityScore) base.JoinSubeventData(other);
    }

    public override CalculateAbilityScore Prepare(RPGLContext context, JsonArray originPoint) {
        base.Prepare(context, originPoint);
        SetBase((long) GetSource().GetAbilityScores().GetLong(GetAbility(context)));
        return this;
    }

    public override CalculateAbilityScore Run(RPGLContext context, JsonArray originPoint) {
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
