using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Overrides the value of damage dice of a specified damage type that rolled within a defined range in a damage roll.
///   
///   <code>
///   {
///     "function": "override_damage_dice",
///     "override": &lt;calculation formula&gt;,
///     "damage_type": &lt;string = "*"&gt;,
///     "lower_bound": &lt;long = 0&gt;,
///     "upper_bound": &lt;long = long.MaxValue&gt;
///   }
///   </code>
///   
///   <i>Note that this function should be paired with a condition checking for the subevent to have either the "base" or "target" tag. Neglecting this check will cause the override to be performed on a damaging subevent at undesired times.</i>
///   
///   <list type="bullet">
///     <item>"override" is a calculation formula that defines the value for an eligible die's rolled value to be overridden with.</item>
///     <item>"damage_type" is an optional field and will default to a value of "*" if not specified. This value causes the function to override the rolled value of eligible dice of all damage types, rather than on eligible dice of a single damage type.</item>
///     <item>"lower_bound" is an optional field and will default to a value of 0 if not specified. This field indicates the minimum value (inclusive) a die must have rolled to be eligible for an override.</item>
///     <item>"upper_bound" is an optional field and will default to a value of long.MaxValue if not specified. This field indicates the maximum value (inclusive) a die must have rolled to be eligible for an override.</item>
///   </list>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>DamageRoll</item>
///   </list>
///   
/// </summary>
public class OverrideDamageDice : Function {

    public OverrideDamageDice() : base("override_damage_dice") {
        functionSteps.AddRange([
            (rpglEffect, subevent, functionJson, context) => {
                if (subevent is DamageRoll damageRoll) {
                    JsonObject overrideJson = functionJson.GetJsonObject("override");

                    string formula = overrideJson.GetString("formula");
                    if (formula == "number") {
                        AdvanceNumber(damageRoll, functionJson);
                    } else if (formula == "modifier") {
                        AdvanceModifier(rpglEffect, damageRoll, functionJson, context);
                    }
                    return new() {
                        dependency = this.dependency,
                        stepCompleted = this.dependency == null,
                    };
                }
                return new() {
                    dependency = null,
                    stepCompleted = true,
                };
            },
        ]);
    }

    public static void AdvanceNumber(DamageRoll damageRoll, JsonObject functionJson) {
        string damageType = functionJson.GetString("damage_type") ?? "*";

        JsonArray typedDamageArray = damageRoll.json.GetJsonArray("damage");
        for (int i = 0; i < typedDamageArray.Count(); i++) {
            JsonObject typedDamage = typedDamageArray.GetJsonObject(i);
            if (damageType == "*" || damageType == typedDamage.GetString("damage_type")) {
                JsonArray typedDamageDieArray = typedDamage.GetJsonArray("dice") ?? new();
                for (int j = 0; j < typedDamageDieArray.Count(); j++) {
                    JsonObject typedDamageDie = typedDamageDieArray.GetJsonObject(j);
                    long roll = (long) typedDamageDie.GetLong("roll");
                    long overrideValue = (long) functionJson.SeekLong("override.number");
                    if (roll < overrideValue) {
                        typedDamageDie.PutLong("roll", overrideValue);
                    }
                }
            }
        }
    }

    public void AdvanceModifier(RPGLEffect rpglEffect, DamageRoll damageRoll, JsonObject functionJson, RPGLContext context) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, damageRoll, functionJson.SeekJsonObject("override.object"));
        if (this.dependency is null) {
            this.dependency = new CalculateAbilityScore()
                .JoinSubeventData(new JsonObject().LoadFromString($$"""
                    {
                        "tags": {{rpglObject.GetTags()}},
                        "ability": "{{functionJson.SeekString("override.ability")}}"
                    }
                    """))
                .SetSource(rpglObject)
                .SetTarget(rpglObject);
        } else {
            string damageType = functionJson.SeekString("override.damage_type") ?? "*";

            JsonArray typedDamageArray = damageRoll.json.GetJsonArray("damage");
            for (int i = 0; i < typedDamageArray.Count(); i++) {
                JsonObject typedDamage = typedDamageArray.GetJsonObject(i);
                if (damageType == "*" || damageType == typedDamage.GetString("damage_type")) {
                    JsonArray typedDamageDieArray = typedDamage.GetJsonArray("dice") ?? new();
                    for (int j = 0; j < typedDamageDieArray.Count(); j++) {
                        JsonObject typedDamageDie = typedDamageDieArray.GetJsonObject(j);
                        long roll = (long) typedDamageDie.GetLong("roll");
                        long overrideValue = RPGLObject.GetAbilityModifierFromAbilityScore((dependency as CalculationSubevent).Get());
                        if (roll < overrideValue) {
                            typedDamageDie.PutLong("roll", overrideValue);
                        }
                    }
                }
            }
            dependency = null;
        }
    }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is DamageRoll damageRoll) {
            damageRoll.OverrideDamageDice(rpglEffect, functionJson, context);
        }
    }

};
