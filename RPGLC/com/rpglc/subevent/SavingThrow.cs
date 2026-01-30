using com.rpglc.core;
using com.rpglc.function;
using com.rpglc.json;
using com.rpglc.runtime;

namespace com.rpglc.subevent;

/// <summary>
///   Performs an attack roll against a target. Different results may occur on a hit or miss. Damage defined in "damage" is only dealt on a hit.
///   
///   <code>
///   {
///     "subevent": "attack_roll",
///     "tags": [
///       &lt;string&gt;
///     ],
///     "save_ability": &lt;string&gt;,
///     "difficulty_class": &lt;long = null&gt;,
///     "difficulty_class_ability": &lt;string = null&gt;,
///     "use_origin_difficulty_class_ability": &lt;bool = false&gt;,
///     "damage": [
///       &lt;bonus formula&gt;
///     ],
///     "damage_on_pass": &lt;"all" | "half" | "none" = "half"&gt;,
///     "vampirism": [
///       &lt;vampirism formula&gt;
///     ],
///     "pass": [
///       &lt;nested subevent&gt;
///     ],
///     "fail": [
///       &lt;nested subevent&gt;
///     ]
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"tags" is an optional field and will default to a value of [ ] if left unspecified. Any tags provided will be inherited by any nested subevents.</item>
///     <item>"save_ability" indicates what ability is used to make the saving throw.</item>
///     <item>"difficulty_class" is an optional field and it will default to a value of null if left unspecified. This field indicates what value, if any, to use as the difficulty class. If null, the difficulty class will not be assigned in this way.</item>
///     <item>"difficulty_class_ability" is an optional field and it will default to a value of null if left unspecified. This field indicates what ability, if any, should be used to calculate the save's difficulty class. If null, the difficulty class will not be calculated in this way.</item>
///     <item>"use_origin_difficulty_class_ability" is an optional field and it will default to a value of false if left unspecified. If true, the save_ability score used for this subevent will be taken from the target's origin object, instead of from the target.</item>
///     <item>"damage" is an optional field and it will default to a value of [ ] if left unspecified. This field indicates how much damage is dealt by the saving throw on a failed save, if any.</item>
///     <item>"damage_on_pass" is an optional field and it will default to a value of "half" if left unspecified. This field incidates what proportion of the subevent's damage will be dealt on a successful save.</item>
///     <item>"vampirism" is an optional field and it will default to a value of [ ] if left unspecified. This field indicates whether and to what extent the damage dealt by this subevent restores hit points to the source.</item>
///     <item>"pass" is an optional field and it will default to a value of [ ] if left unspecified. This field contains a list of subevents that will be invoked if the target passes its save. The damage defined by "damage" will be dealt in accordance with the value of "damage_on_pass".</item>
///     <item>"fail" is an optional field and it will default to a vlaue of [ ] if left unspecified. This field contains a list of subevents that will be invoked if the source fails its save. The damage defined by "damage" will be dealt in full.</item>
///   </list>
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
///     <item>FailSave</item>
///     <item>PassSave</item>
///     <item>SetBase</item>
///     <item>SetMinimum</item>
///     <item>GrantAdvantage</item>
///     <item>GrantDisadvantage</item>
///     <item>AddVampirism</item>
///   </list>
///   
/// </summary>
public class SavingThrow : RollSubevent, IAbilitySubevent, IVampiricSubevent {

    int nestedSubeventIndex = 0;

    public SavingThrow() : base("saving_throw") {
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
                string? resolution = GetDeterminedResolution();
                long roll = Get();
                long dc = (long) json.GetLong("difficulty_class");

                bool pass;
                if (resolution == "pass") {
                    pass = true;
                } else if (resolution == "fail") {
                    pass = false;
                } else {
                    pass = roll >= dc;
                }

                if (pass) {
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
        Subevent clone = new SavingThrow();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new SavingThrow();
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

    public override SavingThrow? Invoke(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        VerifySubevent(this.GetSubeventId());

        // Override base Invoke() code to insert additional post-preparatory logic
        new AddBonus().Execute(
            null,
            this,
            new JsonObject().LoadFromString($$"""
                {
                    "function": "add_bonus",
                    "bonus": [
                        {
                            "formula": "modifier",
                            "ability": "{{GetAbility(context)}}",
                            "object": {
                                "from": "subevent",
                                "object": "target",
                                "as_origin": false
                            }
                        }
                    ]
                }
                """),
            context,
            originPoint
        );

        context.ProcessSubevent(this, originPoint);
        Run(context, originPoint, invokingEffect);
        context.ViewCompletedSubevent(this);
        return this;
    }

    public override SavingThrow JoinSubeventData(JsonObject other) {
        return (SavingThrow) base.JoinSubeventData(other);
    }

    public override SavingThrow Prepare(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        base.Prepare(context, originPoint, invokingEffect);
        json.PutIfAbsent("damage", new JsonArray());
        json.PutIfAbsent("use_origin_difficulty_class_ability", false);
        CalculateDifficultyClass(context, invokingEffect);
        GetBaseDamage(context, originPoint, invokingEffect);
        return this;
    }

    public override SavingThrow Run(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        Roll();
        string? determinedResolution = GetDeterminedResolution();
        if (determinedResolution == "fail" || (Get() < GetDifficultyClass() && determinedResolution != "pass")) {
            GetTargetDamage(context, originPoint, invokingEffect);
            DeliverDamage("all", context, originPoint, invokingEffect);
            ResolveNestedSubevents("fail", context, originPoint, invokingEffect);
        } else if (determinedResolution == "pass" || Get() >= GetDifficultyClass()) {
            GetTargetDamage(context, originPoint, invokingEffect);
            DeliverDamage(json.GetString("damage_on_pass"), context, originPoint, invokingEffect);
            ResolveNestedSubevents("pass", context, originPoint, invokingEffect);
        }
        return this;
    }

    public override SavingThrow SetOriginItem(string? originItem) {
        return (SavingThrow) base.SetOriginItem(originItem);
    }

    public override SavingThrow SetSource(RPGLObject source) {
        return (SavingThrow) base.SetSource(source);
    }

    public override SavingThrow SetTarget(RPGLObject target) {
        return (SavingThrow) base.SetTarget(target);
    }

    public override SavingThrow GrantAdvantage() {
        return (SavingThrow) base.GrantAdvantage();
    }

    public override SavingThrow GrantDisadvantage() {
        return (SavingThrow) base.GrantDisadvantage();
    }

    public string GetAbility(RPGLContext context) {
        return json.GetString("save_ability");
    }

    // TODO function deprecated
    public long? GetDifficultyClass() {
        return json.GetLong("difficulty_class");
    }

    public SavingThrow Fail() {
        json.PutString("determined_resolution", "fail");
        return this;
    }

    public SavingThrow Pass() {
        json.PutString("determined_resolution", "pass");
        return this;
    }

    public string? GetDeterminedResolution() {
        return json.GetString("determined_resolution");
    }

    private void CalculateDifficultyClass(RPGLContext context, RPGLEffect? invokingEffect = null) {
        long? difficultyClass = GetDifficultyClass();

        RPGLObject source = GetSource();
        CalculateDifficultyClass calculateDifficultyClass = new CalculateDifficultyClass()
            .JoinSubeventData(new JsonObject().LoadFromString(difficultyClass is null
                ? $$"""
                {
                    "difficulty_class_ability": "{{json.GetString("difficulty_class_ability")}}",
                    "tags": {{json.GetJsonArray("tags").DeepClone()}}
                }
                """
                : $$"""
                {
                    "difficulty_class": {{difficultyClass}},
                    "tags": {{json.GetJsonArray("tags").DeepClone()}}
                }
                """))
            .SetOriginItem(GetOriginItem())
            .SetSource((bool) json.GetBool("use_origin_difficulty_class_ability")
                ? RPGL.GetRPGLObject(source.GetOriginObject())
                : source
            )
            .Prepare(context, source.GetPosition(), invokingEffect)
            .SetTarget(source)
            .Invoke(context, source.GetPosition(), invokingEffect);

        json.PutLong("difficulty_class", calculateDifficultyClass.Get());
    }

    private void GetBaseDamage(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        RPGLObject source = GetSource();

        DamageCollection baseDamageCollection = new DamageCollection()
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "damage": {{json.GetJsonArray("damage")}},
                    "tags": {{GetTags().DeepClone().AddString("base_damage_collection")}}
                }
                """))
            .SetOriginItem(GetOriginItem())
            .SetSource(source)
            .Prepare(context, originPoint, invokingEffect)
            .SetTarget(source)
            .Invoke(context, originPoint, invokingEffect);

        DamageRoll baseDamageRoll = new DamageRoll()
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "damage": {{baseDamageCollection.GetDamageCollection()}},
                    "tags": {{GetTags().DeepClone().AddString("base_damage_roll")}}
                }
                """))
            .SetOriginItem(GetOriginItem())
            .SetSource(source)
            .Prepare(context, originPoint, invokingEffect)
            .SetTarget(source)
            .Invoke(context, originPoint, invokingEffect);

        json.PutJsonArray("damage", baseDamageRoll.GetDamage());
    }

    private void GetTargetDamage(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        RPGLObject source = GetSource();
        RPGLObject target = GetTarget();

        DamageCollection targetDamageCollection = new DamageCollection()
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "tags": {{GetTags().DeepClone().AddString("target_damage_collection")}}
                }
                """))
            .SetOriginItem(GetOriginItem())
            .SetSource(source)
            .Prepare(context, originPoint, invokingEffect)
            .SetTarget(target)
            .Invoke(context, originPoint, invokingEffect);

        DamageRoll targetDamageRoll = new DamageRoll()
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "damage": {{targetDamageCollection.GetDamageCollection()}},
                    "tags": {{GetTags().DeepClone().AddString("target_damage_roll")}}
                }
                """))
            .SetOriginItem(GetOriginItem())
            .SetSource(source)
            .Prepare(context, originPoint, invokingEffect)
            .SetTarget(target)
            .Invoke(context, originPoint, invokingEffect);

        json.GetJsonArray("damage").AsList().AddRange(targetDamageRoll.GetDamage().AsList());
    }

    private void ResolveNestedSubevents(string resolution, RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        JsonArray subeventJsonArray = json.GetJsonArray(resolution) ?? new();
        for (int i = 0; i < subeventJsonArray.Count(); i++) {
            JsonObject nestedSubeventJson = subeventJsonArray.GetJsonObject(i);
            Subevents[nestedSubeventJson.GetString("subevent")]
                .Clone(nestedSubeventJson)
                .SetOriginItem(GetOriginItem())
                .SetSource(GetSource())
                .Prepare(context, originPoint, invokingEffect)
                .SetTarget(GetTarget())
                .Invoke(context, originPoint, invokingEffect);
        }
    }

    private void DeliverDamage(string damageProportion, RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        if (!Equals(damageProportion, "none")) {
            DamageDelivery damageDelivery = new DamageDelivery()
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "damage": {{json.GetJsonArray("damage")}},
                    "damage_proportion": "{{damageProportion}}",
                    "tags": {{GetTags()}}
                }
                """))
            .SetOriginItem(GetOriginItem())
            .SetSource(GetSource())
            .Prepare(context, originPoint, invokingEffect)
            .SetTarget(GetTarget())
            .Invoke(context, originPoint, invokingEffect);

            JsonObject damageByType = damageDelivery.GetDamage();
            if (json.AsDict().ContainsKey("vampirism")) {
                // TODO excess damage shouldn't contribute to vampirism... should this be handled as a part of DamageDelivery?
                IVampiricSubevent.HandleVampirism(this, damageByType, context, originPoint);
            }
        }
    }

};
