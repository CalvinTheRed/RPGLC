using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.math;
using com.rpglc.runtime;

namespace com.rpglc.subevent;

public abstract class RollSubevent : CalculationSubevent, IAbilitySubevent {

    public RollSubevent(string subeventId) : base(subeventId) {
        this.subeventSteps.AddRange([
            (context) => {
                json.PutIfAbsent("determined", new JsonArray());
                json.PutIfAbsent("has_advantage", false);
                json.PutIfAbsent("has_disadvantage", false);
                json.PutIfAbsent("has_expertise", false);
                json.PutIfAbsent("has_half_proficiency", false);
                json.PutIfAbsent("has_proficiency", false);

                AddTag(GetAbility(context));

                return new() {
                    dependency = null,
                    nextPhase = null,
                    stepCompleted = true,
                };
            },
        ]);
    }

    public SubeventState.StateData AddProficiencyStep(RPGLContext context) {
        if (json.GetBool("has_expertise") == true || json.GetBool("has_proficiency") == true || json.GetBool("has_half_proficiency") == true) {
            if (dependency is null) {
                dependency = new(new CalculateProficiencyBonus()
                    .SetOriginItem(GetOriginItem())
                    .SetSource(GetSource())
                    .SetTarget(GetTarget()));
            } else if (json.GetBool("has_expertise") == true) {
                AddBonus(new JsonObject().LoadFromString($$"""
                    {
                        "bonus": {{(dependency.subevent as CalculateProficiencyBonus).Get()}},
                        "dice": [ ],
                        "scale": {
                            "numerator": 2,
                            "denominator": 1,
                            "round_up": false
                        }
                    }
                    """));
                dependency = null;
            } else if (json.GetBool("has_proficiency") == true) {
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
            } else if (json.GetBool("has_half_proficiency") == true) {
                AddBonus(new JsonObject().LoadFromString($$"""
                    {
                        "bonus": {{(dependency.subevent as CalculateProficiencyBonus).Get()}},
                        "dice": [ ],
                        "scale": {
                            "numerator": 1,
                            "denominator": 2,
                            "round_up": false
                        }
                    }
                    """));
                dependency = null;
            }
        }

        return new() {
            dependency = dependency,
            nextPhase = null,
            stepCompleted = dependency is null,
        };
    }

    public SubeventState.StateData AddProficiencyStepInverted(RPGLContext context) {
        if (json.GetBool("has_expertise") == true || json.GetBool("has_proficiency") == true || json.GetBool("has_half_proficiency") == true) {
            if (dependency is null) {
                dependency = new(new CalculateProficiencyBonus()
                    .SetOriginItem(GetOriginItem())
                    .SetSource(GetTarget())
                    .SetTarget(GetSource()));
            } else if (json.GetBool("has_expertise") == true) {
                AddBonus(new JsonObject().LoadFromString($$"""
                    {
                        "bonus": {{(dependency.subevent as CalculateProficiencyBonus).Get()}},
                        "dice": [ ],
                        "scale": {
                            "numerator": 2,
                            "denominator": 1,
                            "round_up": false
                        }
                    }
                    """));
                dependency = null;
            } else if (json.GetBool("has_proficiency") == true) {
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
            } else if (json.GetBool("has_half_proficiency") == true) {
                AddBonus(new JsonObject().LoadFromString($$"""
                    {
                        "bonus": {{(dependency.subevent as CalculateProficiencyBonus).Get()}},
                        "dice": [ ],
                        "scale": {
                            "numerator": 1,
                            "denominator": 2,
                            "round_up": false
                        }
                    }
                    """));
                dependency = null;
            }
        }

        return new() {
            dependency = dependency,
            nextPhase = null,
            stepCompleted = dependency is null,
        };
    }

    public override RollSubevent Prepare(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        base.Prepare(context, originPoint, invokingEffect);
        json.PutIfAbsent("determined", new JsonArray());
        json.PutBool("has_advantage", false);
        json.PutBool("has_disadvantage", false);
        return this;
    }

    public RollSubevent GrantExpertise() {
        json.PutBool("has_expertise", true);
        return this;
    }

    public RollSubevent GrantProficiency() {
        json.PutBool("has_proficiency", true);
        return this;
    }

    public RollSubevent GrantHalfProficiency() {
        json.PutBool("has_half_proficiency", true);
        return this;
    }

    public RollSubevent GrantAdvantage() {
        json.PutBool("has_advantage", true);
        return this;
    }

    public RollSubevent GrantDisadvantage() {
        json.PutBool("has_disadvantage", true);
        return this;
    }

    public bool IsAdvantageRoll() {
        return (bool) json.GetBool("has_advantage") && !(bool) json.GetBool("has_disadvantage");
    }

    public bool IsDisadvantageRoll() {
        return !(bool) json.GetBool("has_advantage") && (bool) json.GetBool("has_disadvantage");
    }

    public bool IsNormalRoll() {
        return Equals(json.GetBool("has_advantage"), json.GetBool("has_disadvantage"));
    }

    public RollSubevent Roll() {
        JsonArray determined = json.GetJsonArray("determined");
        long baseDieRoll = Die.Roll(20L, determined);
        if (IsAdvantageRoll()) {
            long advantageRoll = Die.Roll(20L, determined);
            if (advantageRoll > baseDieRoll) {
                baseDieRoll = advantageRoll;
            }
        } else if (IsDisadvantageRoll()) {
            long disadvantageRoll = Die.Roll(20L, determined);
            if (disadvantageRoll < baseDieRoll) {
                baseDieRoll = disadvantageRoll;
            }
        }
        return (RollSubevent) SetBase(baseDieRoll);
    }

    public string GetAbility(RPGLContext context) {
        return json.GetString("ability");
    }

};
