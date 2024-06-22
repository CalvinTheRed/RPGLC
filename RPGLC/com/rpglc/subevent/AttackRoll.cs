using com.rpglc.core;
using com.rpglc.function;
using com.rpglc.json;

namespace com.rpglc.subevent;

public class AttackRoll : RollSubevent, IAbilitySubevent, IVampiricSubevent {

    public AttackRoll() : base("attack_roll") { }

    public override Subevent Clone() {
        Subevent clone = new AttackRoll();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new AttackRoll();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override AttackRoll? Invoke(RPGLContext context, JsonArray originPoint) {
        return (AttackRoll?) base.Invoke(context, originPoint);
    }

    public override AttackRoll JoinSubeventData(JsonObject other) {
        return (AttackRoll) base.JoinSubeventData(other);
    }

    public override AttackRoll Prepare(RPGLContext context, JsonArray originPoint) {
        base.Prepare(context, originPoint);
        json.PutIfAbsent("withhold_damage_modifier", false);
        json.PutIfAbsent("use_origin_attack_ability", false);
        json.PutIfAbsent("damage", new JsonArray());
        json.PutIfAbsent("vampirism", new JsonArray());

        // Add tag so nested subevents such as DamageCollection can know they
        // hail from an attack roll made using a particular attack ability.
        AddTag(GetAbility(context));

        // Add tag so nested subevents such as DamageCollection can know they
        // hail from an attack roll of a particular attack type.
        AddTag(json.GetString("attack_type"));

        return this;
    }

    public override AttackRoll Run(RPGLContext context, JsonArray originPoint) {
        Roll();

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
                                "object": "source",
                                "as_origin": {{json.GetBool("use_origin_attack_ability").ToString().ToLower()}}
                            }
                        }
                    ]    
                }
                """),
            context,
            originPoint
        );

        json.PutLong("target_armor_class", GetTarget().CalculateArmorClass(context, this));
        CalculateCriticalHitThreshhold(context, originPoint);

        if (GetBase() >= GetCriticalHitThreshhold()) {
            if (json.GetJsonArray("damage").Count() > 0) {
                GetBaseDamage(context, originPoint);
                GetTargetDamage(context, originPoint);
                if (ConfirmCriticalDamage(context)) {
                    GetCriticalHitDamage(context, originPoint);
                }
                ResolveDamage(context, originPoint);
            }
            ResolveNestedSubevents("hit", context, originPoint);
        } else if (IsCriticalMiss() || Get() < GetTargetArmorClass()) {
            ResolveNestedSubevents("miss", context, originPoint);
        } else {
            if (json.GetJsonArray("damage").Count() > 0) {
                GetBaseDamage(context, originPoint);
                GetTargetDamage(context, originPoint);
                ResolveDamage(context, originPoint);
            }
            ResolveNestedSubevents("hit", context, originPoint);
        }

        return this;
    }

    public override AttackRoll SetOriginItem(string? originItem) {
        return (AttackRoll) base.SetOriginItem(originItem);
    }

    public override AttackRoll SetSource(RPGLObject source) {
        return (AttackRoll) base.SetSource(source);
    }

    public override AttackRoll SetTarget(RPGLObject target) {
        return (AttackRoll) base.SetTarget(target);
    }

    public override AttackRoll GrantAdvantage() {
        return (AttackRoll) base.GrantAdvantage();
    }

    public override AttackRoll GrantDisadvantage() {
        return (AttackRoll) base.GrantDisadvantage();
    }

    public string GetAbility(RPGLContext context) {
        return json.GetString("attack_ability");
    }

    private void GetBaseDamage(RPGLContext context, JsonArray originPoint) {
        // Collect base typed damage dice and bonuses
        DamageCollection baseDamageCollection = new DamageCollection()
            .JoinSubeventData(new JsonObject()
                .PutJsonArray("damage", json.GetJsonArray("damage").DeepClone())
                .PutJsonArray("tags", GetTags().DeepClone()
                    .AddString("base_damage_collection")
                )
            )
            .SetOriginItem(GetOriginItem())
            .SetSource(GetSource())
            .Prepare(context, originPoint)
            .SetTarget(GetTarget())
            .Invoke(context, originPoint);

        // Add damage modifier from attack ability, if applicable
        // TODO make a function for this stuff...
        if (!(bool) json.GetBool("withhold_damage_modifier")) {
            new AddDamage().Execute(
                null,
                baseDamageCollection,
                new JsonObject().LoadFromString($$"""
                    {
                        "function": "add_damage",
                        "damage": [
                            {
                                "formula": "modifier",
                                "damage_type": "{{json.GetJsonArray("damage").GetJsonObject(0).GetString("damage_type")}}",
                                "ability": "{{GetAbility(context)}}",
                                "object": {
                                    "from": "subevent",
                                    "object": "source",
                                    "as_origin": {{json.GetBool("use_origin_attack_ability").ToString().ToLower()}}}
                            }
                        ]
                    }
                    """),
                context,
                originPoint
            );
        }

        // Replace damage key with base damage collection
        json.PutJsonArray("damage", baseDamageCollection.GetDamageCollection());
    }

    private void GetTargetDamage(RPGLContext context, JsonArray originPoint) {
        // Collect target typed damage dice and bonuses
        DamageCollection targetDamageCollection = new DamageCollection()
            .JoinSubeventData(new JsonObject()
                .PutJsonArray("tags", GetTags().DeepClone()
                    .AddString("target_damage_collection")
                )
            )
            .SetOriginItem(GetOriginItem())
            .SetSource(GetSource())
            .Prepare(context, originPoint)
            .SetTarget(GetTarget())
            .Invoke(context, originPoint);

        // Add target damage collection to base damage collection
        json.GetJsonArray("damage").AsList().AddRange(targetDamageCollection.GetDamageCollection().AsList());
    }

    private void CalculateCriticalHitThreshhold(RPGLContext context, JsonArray originPoint) {
        CalculateCriticalHitThreshhold calculateCriticalHitThreshhold = new CalculateCriticalHitThreshhold()
            .JoinSubeventData(new JsonObject()
                .PutJsonArray("tags", GetTags().DeepClone())
            )
            .SetOriginItem(GetOriginItem())
            .SetSource(GetSource())
            .Prepare(context, originPoint)
            .SetTarget(GetTarget())
            .Invoke(context, originPoint);
        json.PutLong("critical_hit_threshhold", calculateCriticalHitThreshhold.Get());
    }

    public long GetTargetArmorClass() {
        return (long) json.GetLong("target_armor_class");
    }

    public long GetCriticalHitThreshhold() {
        return (long) json.GetLong("critical_hit_threshhold");
    }

    private bool ConfirmCriticalDamage(RPGLContext context) {
        // TODO come back to this later
        return true;
    }

    public bool IsCriticalMiss() {
        return GetBase() == 1L;
    }

    private void GetCriticalHitDamage(RPGLContext context, JsonArray originPoint) {
        // Get a copy of the attack damage with twice the number of dice
        JsonArray damageArray = json.GetJsonArray("damage").DeepClone();
        for (int i = 0; i < damageArray.Count(); i++) {
            JsonObject damageJson = damageArray.GetJsonObject(i);
            JsonArray dice = damageJson.GetJsonArray("dice");
            dice?.AsList().AddRange(dice.DeepClone().AsList());
        }

        // Collect any extra damage bonuses which aren't subject to dice doubling
        CriticalHitDamageCollection criticalHitDamageCollection = new CriticalHitDamageCollection()
            .JoinSubeventData(new JsonObject()
                .PutJsonArray("tags", GetTags().DeepClone())
                .PutJsonArray("damage", damageArray)
            )
            .SetOriginItem(GetOriginItem())
            .SetSource(GetSource())
            .Prepare(context, originPoint)
            .SetTarget(GetTarget())
            .Invoke(context, originPoint);

        // Set the attack damage to the critical hit damage collection
        json.PutJsonArray("damage", criticalHitDamageCollection.GetDamageCollection());
    }

    private void ResolveDamage(RPGLContext context, JsonArray originPoint) {
        DamageRoll damageRoll = new DamageRoll()
            .JoinSubeventData (new JsonObject()
                .PutJsonArray("damage", json.GetJsonArray("damage"))
                .PutJsonArray("tags", GetTags().DeepClone()
                    .AddString("attack_damage_roll")
                )
            )
            .SetOriginItem(GetOriginItem())
            .SetSource(GetSource())
            .Prepare(context, originPoint)
            .SetTarget(GetTarget())
            .Invoke(context, originPoint);

        // Store final damage by type to damage key
        json.PutJsonArray("damage", damageRoll.GetDamage());

        DeliverDamage(context, originPoint);
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

    private void DeliverDamage(RPGLContext context, JsonArray originPoint) {
        DamageDelivery damageDelivery = new DamageDelivery()
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "damage": {{json.GetJsonArray("damage")}},
                    "tags": {{json.GetJsonArray("tags").ToString()}}
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

};
