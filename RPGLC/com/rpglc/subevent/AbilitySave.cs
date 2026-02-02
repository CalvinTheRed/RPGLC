using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;

namespace com.rpglc.subevent;

/// <summary>
///   Performs an ability check and compares it to a difficulty class. Different results may occur on a pass or fail.
///   
///   <code>
///   {
///     "subevent": "ability_save",
///     "tags": [
///       &lt;string&gt;
///     ],
///     "ability": &lt;string&gt;,
///     "skill": &lt;string = null&gt;,
///     "use_origin_difficulty_class_ability": &lt;bool = false&gt;,
///     "difficulty_class_ability": &lt;string = null&gt;,
///     "difficulty_class": &lt;long = null&gt;,
///     "pass": [
///       &lt;nested subevent&gt;
///     ],
///     "fail": [
///       &lt;nested subevent&gt;
///     ]
///   }
///   </code>
///   
///   <i>Note that "difficulty_class_ability" and "difficulty_class" are mutually exclusive, but that one of them is required for the subevent to work as intended.</i>
///   
///   <br /><br />
///   <list type="bullet">
///     <item>"tags" is an optional field and will default to a value of [ ] if left unspecified. Any tags provided will be inherited by any nested subevents.</item>
///     <item>"ability" indicates what ability is used to make the ability check.</item>
///     <item>"skill" is an optional field and will default to a value of null if left unspecified. This field indicates what skill, if any, is used to make the ability check. This typically goes to inform how proficiency bonuses will be applied to the subevent.</item>
///     <item>"use_origin_difficulty_class_ability" is an optional field and it will default to a value of false if left unspecified. If true, the ability score used for this subevent will be taken from the source's origin object, instead of from the source.</item>
///     <item>"difficulty_class_ability" is an optional field and it will default to a value of null if left unspecified. This field indicates what ability, if any, should be used to calculate the save's difficulty class. If null, the difficulty class will not be calculated in this way.</item>
///     <item>"difficulty_class" is an optional field and it will default to a value of null if left unspecified. This field indicates what value, if any, to use as the difficulty class. If null, the difficulty class will not be assigned in this way.</item>
///     <item>"pass" is an optional field and it will default to a vlaue of [ ] if left unspecified. This field contains a list of subevents that will be invoked if the target passes the save.</item>
///     <item>"fail" is an optional field and it will default to a value of [ ] if left unspecified. This field contains a list of subevents that will be invoked if the target fails the save.</item>
///   </list>
///   
///   <b>Special Functions</b>
///   <list type="bullet">
///     <item>AddBonus</item>
///     <item>SetBase</item>
///     <item>SetMinimum</item>
///     <item>GrantRollExpertise</item>
///     <item>GrantRollHalfProficiency</item>
///     <item>GrantRollProficiency</item>
///     <item>GrantAdvantage</item>
///     <item>GrantDisadvantage</item>
///     <item>AddVampirism</item>
///   </list>
///   
/// </summary>
public class AbilitySave : RollSubevent, IVampiricSubevent {

    int nestedSubeventIndex = 0;

    public AbilitySave() : base("ability_save") {
        subeventSteps.AddRange([
            //
            // pre-targeting steps
            //
            (context) => {
                json.PutIfAbsent("damage", new JsonArray());
                json.PutIfAbsent("use_origin_difficulty_class_ability", false);
                json.PutIfAbsent("pass", new JsonArray());
                json.PutIfAbsent("fail", new JsonArray());

                RPGLObject rpglObject = (bool) json.GetBool("use_origin_difficulty_class_ability")
                        ? RPGL.GetRPGLObject(GetSource().GetOriginObject())
                        : GetSource();
                long? difficultyClass = GetDifficultyClass();
                dependency = new(new CalculateDifficultyClass()
                    .SetOriginItem(GetOriginItem())
                    .SetSource(rpglObject)
                    .SetTarget(rpglObject)
                    .JoinSubeventData(new JsonObject().LoadFromString(difficultyClass is null
                        ? $$"""
                        {
                            "difficulty_class_ability": "{{json.GetString("difficulty_class_ability")}}",
                            "tags": {{GetTags()}}
                        }
                        """
                        : $$"""
                        {
                            "difficulty_class": {{difficultyClass}},
                            "tags": {{GetTags()}}
                        }
                        """)));

                return new() {
                    dependency = dependency,
                    nextPhase = null,
                    stepCompleted = true,
                };
            },
            (context) => {
                json.PutLong("difficulty_class", (dependency.subevent as CalculateDifficultyClass).Get());

                RPGLObject rpglObject = GetSource();
                dependency = new(new DamageCollection()
                    .SetOriginItem(GetOriginItem())
                    .SetSource(rpglObject)
                    .SetTarget(rpglObject)
                    .JoinSubeventData(new JsonObject().LoadFromString($$"""
                        {
                            "damage": {{json.GetJsonArray("damage")}},
                            "tags": {{GetTags()}}
                        }
                        """))
                    .AddTag("base_damage_collection"));

                return new() {
                    dependency = dependency,
                    nextPhase = null,
                    stepCompleted = true,
                };
            },
            (context) => {
                RPGLObject rpglObject = GetSource();
                dependency = new(new DamageRoll()
                    .SetOriginItem(GetOriginItem())
                    .SetSource(rpglObject)
                    .SetTarget(rpglObject)
                    .JoinSubeventData(new JsonObject().LoadFromString($$"""
                        {
                            "damage": {{(dependency.subevent as DamageCollection).GetDamageCollection()}},
                            "tags": {{GetTags()}}
                        }
                        """))
                    .AddTag("base_damage_roll"));

                return new() {
                    dependency = dependency,
                    nextPhase = null,
                    stepCompleted = true,
                };
            },
            (context) => {
                json.PutJsonArray("damage", (dependency.subevent as DamageRoll).GetDamage());

                dependency = null;

                return new() {
                    dependency = null,
                    nextPhase = SubeventState.Phase.Targeting,
                    stepCompleted = true,
                };
            },
            //
            // post-targeting steps
            //
            AddProficiencyStep,
            (context) => {
                RPGLObject rpglObject = GetTarget();
                dependency = new(new CalculateAbilityScore()
                    .SetOriginItem (GetOriginItem())
                    .SetSource(rpglObject)
                    .SetTarget(rpglObject)
                    .JoinSubeventData (new JsonObject().LoadFromString($$"""
                        {
                            "ability": "{{GetAbility(context)}}",
                            "tags": {{GetTags()}}
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
                            "numerator": 1,
                            "denominator": 1,
                            "round_up": false
                        }
                    }
                    """));

                RPGLObject rpglObject = GetSource();
                dependency = new(new DamageCollection()
                    .SetOriginItem(GetOriginItem())
                    .SetSource(rpglObject)
                    .SetTarget(rpglObject)
                    .JoinSubeventData(new JsonObject().LoadFromString($$"""
                        {
                            "tags": {{GetTags()}}
                        }
                        """))
                    .AddTag("target_damage_collection"));

                return new() {
                    dependency = dependency,
                    nextPhase = null,
                    stepCompleted = true,
                };
            },
            (context) => {
                RPGLObject rpglObject = GetSource();
                dependency = new(new DamageRoll()
                    .SetOriginItem(GetOriginItem())
                    .SetSource(rpglObject)
                    .SetTarget(rpglObject)
                    .JoinSubeventData(new JsonObject().LoadFromString($$"""
                        {
                            "damage": {{(dependency.subevent as DamageCollection).GetDamageCollection()}},
                            "tags": {{GetTags()}}
                        }
                        """))
                    .AddTag("target_damage_roll"));

                return new() {
                    dependency = dependency,
                    nextPhase = null,
                    stepCompleted = true,
                };
            },
            (context) => {
                Roll();

                if (Get() >= (long) json.GetLong("difficulty_class")) {
                    AddPassSteps();
                } else {
                    AddFailSteps();
                }

                json.GetJsonArray("damage").AsList().AddRange((dependency.subevent as DamageRoll).GetDamage().AsList());
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
        Subevent clone = new AttackRoll();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new AbilitySave();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public void AddPassSteps() {
        subeventSteps.AddRange([
            (context) => {
                string damageProportion = json.GetString("damage_on_pass");
                bool dealsVampiricDamage = false;
                if (!Equals(damageProportion, "none")) {
                    dependency = new(new DamageDelivery()
                        .SetOriginItem(GetOriginItem())
                        .SetSource(GetSource())
                        .SetTarget(GetTarget())
                        .JoinSubeventData(new JsonObject().LoadFromString($$"""
                            {
                                "damage": {{json.GetJsonArray("damage")}},
                                "damage_proportion": "{{damageProportion}}",
                                "tags": {{GetTags()}}
                            }
                            """)));

                    dealsVampiricDamage = json.AsDict().ContainsKey("vampirism");
                    if (dealsVampiricDamage) {
                        IVampiricSubevent.AddVampirismSteps(this);
                    }
                }

                AddNestedSubeventSteps("pass");

                return new() {
                    dependency = dependency,
                    nextPhase = dealsVampiricDamage || !json.GetJsonArray("pass").IsEmpty() ? null : SubeventState.Phase.Completed,
                    stepCompleted = true,
                };
            },
        ]);
    }

    public void AddFailSteps() {
        subeventSteps.AddRange([
            (context) => {
                dependency = new(new DamageDelivery()
                    .SetOriginItem(GetOriginItem())
                    .SetSource(GetSource())
                    .SetTarget(GetTarget())
                    .JoinSubeventData(new JsonObject().LoadFromString($$"""
                        {
                            "damage": {{json.GetJsonArray("damage")}},
                            "tags": {{GetTags()}}
                        }
                        """)));

                bool isVampiric = json.AsDict().ContainsKey("vampirism");
                if (isVampiric) {
                    IVampiricSubevent.AddVampirismSteps(this);
                }

                AddNestedSubeventSteps("fail");

                return new() {
                    dependency = dependency,
                    nextPhase = isVampiric || !json.GetJsonArray("fail").IsEmpty() ? null : SubeventState.Phase.Completed,
                    stepCompleted = true,
                };
            },
        ]);
    }

    public void AddNestedSubeventSteps(string resolution) {
        if (!json.GetJsonArray(resolution).IsEmpty()) {
            subeventSteps.AddRange([
                (context) => {
                    JsonArray nestedSubeventArray = json.GetJsonArray(resolution) ?? new();
                    bool completed = nestedSubeventIndex == nestedSubeventArray.Count();

                    if (!completed) {
                        JsonObject nestedSubeventJson = nestedSubeventArray.GetJsonObject(nestedSubeventIndex);
                        nestedSubeventIndex++;

                        dependency = new(Subevent.Subevents[nestedSubeventJson.GetString("subevent")]
                            .Clone(nestedSubeventJson)
                            .SetOriginItem(GetOriginItem())
                            .SetSource(GetSource())
                            .SetTarget(GetTarget()));
                    }

                    return new() {
                        dependency = dependency,
                        nextPhase = nestedSubeventIndex == nestedSubeventArray.Count() ? SubeventState.Phase.Completed : null,
                        stepCompleted = nestedSubeventIndex == nestedSubeventArray.Count(),
                    };
                }
            ]);
        }
    }

    public override AbilitySave? Invoke(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return (AbilitySave?) base.Invoke(context, originPoint, invokingEffect);
    }

    public override AbilitySave JoinSubeventData(JsonObject other) {
        return (AbilitySave) base.JoinSubeventData(other);
    }

    public override AbilitySave Prepare(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        json.PutIfAbsent("determined", new JsonArray());
        json.PutIfAbsent("use_origin_difficulty_class_ability", false);
        CalculateDifficultyClass(context, invokingEffect);
        return this;
    }

    public override AbilitySave Run(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return this;
    }

    public override AbilitySave SetOriginItem(string? originItem) {
        return (AbilitySave) base.SetOriginItem(originItem);
    }

    public override AbilitySave SetSource(RPGLObject source) {
        return (AbilitySave) base.SetSource(source);
    }

    public override AbilitySave SetTarget(RPGLObject target) {
        return (AbilitySave) base.SetTarget(target);
    }

    private void CalculateDifficultyClass(RPGLContext context, RPGLEffect? invokingEffect = null) {
        long? difficultyClass = GetDifficultyClass();

        CalculateDifficultyClass calculateDifficultyClass = new CalculateDifficultyClass()
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    {{(difficultyClass.HasValue
                    ? $"\"difficulty_class\": {difficultyClass}"
                    : $"\"difficulty_class_ability\": \"{json.GetString("difficulty_class_ability")}\"")}},
                    "tags": {{GetTags()}}
                }
            """))
            .SetOriginItem(GetOriginItem())
            .SetSource((bool) json.GetBool("use_origin_difficulty_class_ability")
                ? RPGL.GetRPGLObject(GetSource().GetOriginObject())
                : GetSource()
            )
            .Prepare(context, GetSource().GetPosition(), invokingEffect)
            .SetTarget(GetSource())
            .Invoke(context, GetSource().GetPosition(), invokingEffect);

        json.PutLong("difficulty_class", calculateDifficultyClass.Get());
    }

    private long? GetDifficultyClass() {
        return json.GetLong("difficulty_class");
    }

    public string? GetSkill() {
        return json.GetString("skill");
    }

}
