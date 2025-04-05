using com.rpglc.core;
using com.rpglc.function;
using com.rpglc.json;

namespace com.rpglc.subevent;

/// <summary>
///   Performs an ability check and stores the result. This subevent does not precipitate any special behavior on its own.
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
public class AbilityCheck : RollSubevent, IAbilitySubevent {

    public AbilityCheck() : base("ability_check") { }

    public override Subevent Clone() {
        Subevent clone = new AttackRoll();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new AbilityCheck();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override AbilityCheck? Invoke(RPGLContext context, JsonArray originPoint) {
        return (AbilityCheck?) base.Invoke(context, originPoint);
    }

    public override AbilityCheck JoinSubeventData(JsonObject other) {
        return (AbilityCheck) base.JoinSubeventData(other);
    }

    public override AbilityCheck Prepare(RPGLContext context, JsonArray originPoint) {
        base.Prepare(context, originPoint);
        json.PutIfAbsent("has_expertise", false);
        json.PutIfAbsent("has_proficiency", false);
        json.PutIfAbsent("has_half_proficiency", false);
        return this;
    }

    public override AbilityCheck Run(RPGLContext context, JsonArray originPoint) {
        Roll();
        long proficiencyNumerator = 0L;
        long proficiencyDenominator = 1L;
        if (GetExpertise()) {
            proficiencyNumerator = 2L;
        } else if (GetProficiency()) {
            proficiencyNumerator = 1L;
        } else if (GetHalfProficiency()) {
            proficiencyNumerator = 1L;
            proficiencyDenominator = 2L;
        }
        new AddBonus().Execute(null, this, new JsonObject().LoadFromString($$"""
            {
                "function": "add_bonus",
                "bonus": [
                    {
                        "formula": "modifier",
                        "ability": "{{GetAbility(context)}}",
                        "object": {
                            "from": "subevent",
                            "object": "source"
                        }
                    },
                    {
                        "formula": "proficiency",
                        "object": {
                            "from": "subevent",
                            "object": "source"
                        },
                        "scale": {
                            "numerator": {{proficiencyNumerator}},
                            "denominator": {{proficiencyDenominator}},
                            "round_up": false
                        }
                    }
                ]
            }
            """), context, originPoint);
        return this;
    }

    public override AbilityCheck SetOriginItem(string? originItem) {
        return (AbilityCheck) base.SetOriginItem(originItem);
    }

    public override AbilityCheck SetSource(RPGLObject source) {
        return (AbilityCheck) base.SetSource(source);
    }

    public override AbilityCheck SetTarget(RPGLObject target) {
        return (AbilityCheck) base.SetTarget(target);
    }

    public override AbilityCheck GrantAdvantage() {
        return (AbilityCheck) base.GrantAdvantage();
    }

    public override AbilityCheck GrantDisadvantage() {
        return (AbilityCheck) base.GrantDisadvantage();
    }

    private bool GetExpertise() {
        return (bool) json.GetBool("has_expertise");
    }

    private bool GetProficiency() {
        return (bool) json.GetBool("has_proficiency");
    }

    private bool GetHalfProficiency() {
        return (bool) json.GetBool("has_half_proficiency");
    }

    public AbilityCheck GrantExpertise() {
        json.PutBool("has_expertise", true);
        return this;
    }

    public AbilityCheck GrantProficiency() {
        json.PutBool("has_proficiency", true);
        return this;
    }

    public AbilityCheck GrantHalfProficiency() {
        json.PutBool("has_half_proficiency", true);
        return this;
    }

    public bool HasExpertise() {
        return GetExpertise();
    }

    public bool HasProficiency() {
        return !GetExpertise() && GetProficiency();
    }

    public bool HasHalfProficiency() {
        return !GetExpertise() && !GetProficiency() && GetHalfProficiency();
    }

    public string GetAbility(RPGLContext context) {
        return json.GetString("ability");
    }

    public string? GetSkill() {
        return json.GetString("skill");
    }
}
