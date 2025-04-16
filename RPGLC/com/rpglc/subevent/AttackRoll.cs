using com.rpglc.core;
using com.rpglc.function;
using com.rpglc.json;

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
///     "attack_type": &lt;string&gt;,
///     "ability": &lt;string&gt;,
///     "use_origin_ability": &lt;bool = false&gt;,
///     "damage": [
///       &lt;bonus formula&gt;
///     ],
///     "withhold_damage_modifier": &lt;bool = false&gt;,
///     "vampirism": [
///       &lt;vampirism formula&gt;
///     ],
///     "hit": [
///       &lt;nested subevent&gt;
///     ],
///     "miss": [
///       &lt;nested_subevent&gt;
///     ],
///     "crtical_hit_threshhold": &lt;long = 20&gt;
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"tags" is an optional field and will default to a value of [ ] if left unspecified. Any tags provided will be inherited by any nested subevents. The subevent's "attack_type" value is added to its tags upon subevent preparation.</item>
///     <item>"attack_type" indicates what kind of attack is being made (normally "melee", "ranged", or "thrown"). This value will be promoted to the subevent's tags.</item>
///     <item>"ability" indicates what ability is used to make the ability check.</item>
///     <item>"use_origin_ability" is an optional field and it will default to a value of false if left unspecified. If true, the ability score used for this subevent will be taken from the source's origin object, instead of from the source.</item>
///     <item>"damage" is an optional field and it will default to a value of [ ] if left unspecified. This field indicates how much damage the attack deals on a hit, if any.</item>
///     <item>"withhold_damage_modifier" is an optional field and it will default to a value of false if left unspecified. If true, the subevent will not add the modifier of the attack's ability score to the subevent's base damage roll.</item>
///     <item>"vampirism" is an optional field and it will default to a value of [ ] if left unspecified. This field indicates whether and to what extent the damage dealt by this subevent restores hit points to the source.</item>
///     <item>"hit" is an optional field and it will default to a value of [ ] if left unspecified. This field contains a list of subevents that will be invoked if the source hits the target. The damage defined by "damage" will be dealt on a hit.</item>
///     <item>"miss" is an optional field and it will default to a value of [ ] if left unspecified. This field contains a list of subevents that will be invoked if the source misses the target. The damage defined by "damage" will not be dealt on a miss.</item>
///     <item>"critical_hit_threshhold" is an optional field and it will default to a value of 20 if left unspecified. This field indicates the default minimum number which must be rolled on the d20 to qualify the attack as a critical hit.</item>
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
///     <item>SetBase</item>
///     <item>SetMinimum</item>
///     <item>GrantAdvantage</item>
///     <item>GrantDisadvantage</item>
///     <item>AddVampirism</item>
///   </list>
///   
/// </summary>
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
        json.PutIfAbsent("use_origin_ability", false);
        json.PutIfAbsent("damage", new JsonArray());
        json.PutIfAbsent("vampirism", new JsonArray());
        json.PutIfAbsent("critical_hit_threshhold", 20L);

        // Add tag so nested subevents such as DamageCollection can know they
        // hail from an attack roll made using a particular attack ability.
        AddTag(GetAbility(context));

        // Add tag so nested subevents such as DamageCollection can know they
        // hail from an attack roll of a particular attack type.
        AddTag(json.GetString("attack_type"));

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
                                "as_origin": {{json.GetBool("use_origin_ability").ToString().ToLower()}}
                            }
                        }
                    ]    
                }
                """),
            context,
            originPoint
        );

        return this;
    }

    public override AttackRoll Run(RPGLContext context, JsonArray originPoint) {
        Roll();

        json.PutLong("target_armor_class", GetTarget().CalculateArmorClass(context, this));
        CalculateCriticalHitThreshhold(context, originPoint);

        if (GetBase() >= GetCriticalHitThreshhold() && ConfirmCriticalDamage(context, originPoint)) {
            if (json.GetJsonArray("damage").Count() > 0) {
                GetBaseDamage(context, originPoint);
                GetTargetDamage(context, originPoint);
                GetCriticalHitDamage(context, originPoint);
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
        return json.GetString("ability");
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
                                    "as_origin": {{json.GetBool("use_origin_ability").ToString().ToLower()}}
                                }
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
                .PutLong("critical_hit_threshhold", json.GetLong("critical_hit_threshhold"))
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

    private bool ConfirmCriticalDamage(RPGLContext context, JsonArray originPoint) {
        return new CriticalDamageConfirmation()
            .JoinSubeventData(new JsonObject()
                .PutJsonArray("tags", GetTags().DeepClone())
            )
            .SetOriginItem(GetOriginItem())
            .SetSource(GetSource())
            .Prepare(context, originPoint)
            .SetTarget(GetTarget())
            .Invoke(context, originPoint)
            .DealsCriticalDamage();
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

};
