using com.rpglc.core;
using com.rpglc.function;
using com.rpglc.json;

namespace com.rpglc.subevent;

public class SavingThrow : RollSubevent, IAbilitySubevent, IVampiricSubevent {

    public SavingThrow() : base("saving_throw") { }

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

    public override SavingThrow? Invoke(RPGLContext context, JsonArray originPoint) {
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
                                "object": "target"
                            }
                        }
                    ]
                }
                """),
            context,
            originPoint
        );

        context.ProcessSubevent(this, originPoint);
        Run(context, originPoint);
        context.ViewCompletedSubevent(this);
        return this;
    }

    public override SavingThrow JoinSubeventData(JsonObject other) {
        return (SavingThrow) base.JoinSubeventData(other);
    }

    public override SavingThrow Prepare(RPGLContext context, JsonArray originPoint) {
        base.Prepare(context, originPoint);
        json.PutIfAbsent("damage", new JsonArray());
        json.PutIfAbsent("use_origin_difficulty_class_ability", false);
        CalculateDifficultyClass(context);
        GetBaseDamage(context, originPoint);
        return this;
    }

    public override SavingThrow Run(RPGLContext context, JsonArray originPoint) {
        Roll();
        if (Get() < GetDifficultyClass()) {
            GetTargetDamage(context, originPoint);
            DeliverDamage("all", context, originPoint);
            ResolveNestedSubevents("fail", context, originPoint);
        } else {
            GetTargetDamage(context, originPoint);
            DeliverDamage(json.GetString("damage_on_pass"), context, originPoint);
            ResolveNestedSubevents("pass", context, originPoint);
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

    public long? GetDifficultyClass() {
        return json.GetLong("difficulty_class");
    }

    private void CalculateDifficultyClass(RPGLContext context) {
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
                ? RPGLObject.GetRPGLObjects().Find(x => x.GetUuid() == source.GetOriginObject())
                : source
            )
            .Prepare(context, source.GetPosition())
            .SetTarget(source)
            .Invoke(context, source.GetPosition());

        json.PutLong("difficulty_class", calculateDifficultyClass.Get());
    }

    private void GetBaseDamage(RPGLContext context, JsonArray originPoint) {
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
            .Prepare(context, originPoint)
            .SetTarget(source)
            .Invoke(context, originPoint);

        DamageRoll baseDamageRoll = new DamageRoll()
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "damage": {{baseDamageCollection.GetDamageCollection()}},
                    "tags": {{GetTags().DeepClone().AddString("base_damage_roll")}}
                }
                """))
            .SetOriginItem(GetOriginItem())
            .SetSource(source)
            .Prepare(context, originPoint)
            .SetTarget(source)
            .Invoke(context, originPoint);

        json.PutJsonArray("damage", baseDamageRoll.GetDamage());
    }

    private void GetTargetDamage(RPGLContext context, JsonArray originPoint) {
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
            .Prepare(context, originPoint)
            .SetTarget(target)
            .Invoke(context, originPoint);

        DamageRoll targetDamageRoll = new DamageRoll()
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "damage": {{targetDamageCollection.GetDamageCollection()}},
                    "tags": {{GetTags().DeepClone().AddString("target_damage_roll")}}
                }
                """))
            .SetOriginItem(GetOriginItem())
            .SetSource(source)
            .Prepare(context, originPoint)
            .SetTarget(target)
            .Invoke(context, originPoint);

        json.GetJsonArray("damage").AsList().AddRange(targetDamageRoll.GetDamage().AsList());
    }

    private void ResolveNestedSubevents(string resolution, RPGLContext context, JsonArray originPoint) {
        JsonArray subeventJsonArray = json.GetJsonArray(resolution) ?? new();
        for (int i = 0; i < subeventJsonArray.Count(); i++) {
            JsonObject nestedSubeventJson = subeventJsonArray.GetJsonObject(i);
            Subevents[nestedSubeventJson.GetString("subevent")]
                .Clone(nestedSubeventJson)
                .SetOriginItem(GetOriginItem())
                .SetSource(GetSource())
                .Prepare(context, originPoint)
                .SetTarget(GetTarget())
                .Invoke(context, originPoint);
        }
    }

    private void DeliverDamage(string damageProportion, RPGLContext context, JsonArray originPoint) {
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
            .Prepare(context, originPoint)
            .SetTarget(GetTarget())
            .Invoke(context, originPoint);

            JsonObject damageByType = damageDelivery.GetDamage();
            if (json.AsDict().ContainsKey("vampirism")) {
                // TODO excess damage shouldn't contribute to vampirism... should this be handled as a part of DamageDelivery?
                IVampiricSubevent.HandleVampirism(this, damageByType, context, originPoint);
            }
        }
    }

};
