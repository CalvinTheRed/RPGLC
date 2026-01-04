using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.math;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Sets the base value of a calculation subevent according to a defined formula.
///   
///   <code>
///   {
///     "function": "set_base",
///     "base": &lt;calculation formula&gt;
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"base" is a calculation formula that sets the base of a calculation-type subevent.</item>
///   </list>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>CalculateAbilityScore</item>
///     <item>CalculateArmorClass</item>
///     <item>CalculateCriticalHitThreshold</item>
///     <item>CalculateDifficultyClass</item>
///     <item>CalculateMaximumHitPoints</item>
///     <item>CalculateProficiencyBonus</item>
///   </list>
///   
/// </summary>
public class SetBase : Function {

    public SetBase() : base("set_base") {
        functionSteps.AddRange([
            (rpglEffect, subevent, functionJson, context) => {
                if (subevent is CalculationSubevent calculationSubevent) {
                    JsonObject baseJson = functionJson.GetJsonObject("base");
                    string? formula = baseJson.GetString("formula");
                    if (formula == "number") {
                        AdvanceNumber(calculationSubevent, baseJson);
                    } else if (formula == "modifier") {
                        AdvanceModifier(rpglEffect, calculationSubevent, baseJson, context);
                    } else if (formula == "ability") {
                        AdvanceAbility(rpglEffect, calculationSubevent, baseJson, context);
                    } else if (formula == "proficiency") {
                        AdvanceProficiency(rpglEffect, calculationSubevent, baseJson, context);
                    } else if (formula == "level") {
                        AdvanceLevel(rpglEffect, calculationSubevent, baseJson);
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

    public static void AdvanceNumber(CalculationSubevent calculationSubevent, JsonObject baseJson) {
        calculationSubevent.SetBase(CalculationSubevent.Scale(
                (long) baseJson.GetLong("number"),
                baseJson.GetJsonObject("scale") ?? new JsonObject().LoadFromString($$"""
                    {
                        "numerator": 1,
                        "denominator": 1,
                        "round_up": false
                    }
                    """
                )
            ));
    }

    public void AdvanceModifier(RPGLEffect rpglEffect, CalculationSubevent calculationSubevent, JsonObject baseJson, RPGLContext context) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, calculationSubevent, baseJson.GetJsonObject("object"));
        if (this.dependency is null) {
            this.dependency = new CalculateAbilityScore()
                .JoinSubeventData(new JsonObject().LoadFromString($$"""
                    {
                        "tags": {{rpglObject.GetTags().DeepClone()}},
                        "ability": "{{baseJson.GetString("ability")}}"
                    }
                    """)
                )
                .SetSource(rpglObject)
                .SetTarget(rpglObject);
        } else {
            calculationSubevent.SetBase(CalculationSubevent.Scale(
                RPGLObject.GetAbilityModifierFromAbilityScore((dependency as CalculationSubevent).Get()),
                baseJson.GetJsonObject("scale") ?? new JsonObject().LoadFromString($$"""
                    {
                        "numerator": 1,
                        "denominator": 1,
                        "round_up": false
                    }
                    """
                )
            ));
            dependency = null;
        }
    }

    public void AdvanceAbility(RPGLEffect rpglEffect, CalculationSubevent calculationSubevent, JsonObject bonusJson, RPGLContext context) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, calculationSubevent, bonusJson.GetJsonObject("object"));
        if (this.dependency is null) {
            this.dependency = new CalculateAbilityScore()
                .JoinSubeventData(new JsonObject().LoadFromString($$"""
                    {
                        "tags": {{rpglObject.GetTags().DeepClone()}},
                        "ability": "{{bonusJson.GetString("ability")}}"
                    }
                    """)
                )
                .SetSource(rpglObject)
                .SetTarget(rpglObject);
        } else {
            calculationSubevent.SetBase(CalculationSubevent.Scale(
                (dependency as CalculationSubevent).Get(),
                bonusJson.GetJsonObject("scale") ?? new JsonObject().LoadFromString($$"""
                    {
                        "numerator": 1,
                        "denominator": 1,
                        "round_up": false
                    }
                    """
                )
            ));
            dependency = null;
        }
    }

    public void AdvanceProficiency(RPGLEffect rpglEffect, CalculationSubevent calculationSubevent, JsonObject bonusJson, RPGLContext context) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, calculationSubevent, bonusJson.GetJsonObject("object"));
        if (this.dependency is null) {
            this.dependency = new CalculateProficiencyBonus()
                .JoinSubeventData(new JsonObject().LoadFromString($$"""
                    {
                        "tags": {{rpglObject.GetTags().DeepClone()}}
                    }
                    """)
                )
                .SetSource(rpglObject)
                .SetTarget(rpglObject);
        } else {
            calculationSubevent.SetBase(CalculationSubevent.Scale(
                (dependency as CalculationSubevent).Get(),
                bonusJson.GetJsonObject("scale") ?? new JsonObject().LoadFromString($$"""
                    {
                        "numerator": 1,
                        "denominator": 1,
                        "round_up": false
                    }
                    """
                )
            ));
            dependency = null;
        }
    }

    public static void AdvanceLevel(RPGLEffect rpglEffect, CalculationSubevent calculationSubevent, JsonObject bonusJson) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, calculationSubevent, bonusJson.GetJsonObject("object"));
        string classDatapackId = bonusJson.GetString("class") ?? "*";

        calculationSubevent.SetBase(CalculationSubevent.Scale(
            classDatapackId == "*" ? rpglObject.GetLevel() : rpglObject.GetLevel(classDatapackId),
            bonusJson.GetJsonObject("scale") ?? new JsonObject().LoadFromString($$"""
                {
                    "numerator": 1,
                    "denominator": 1,
                    "round_up": false
                }
                """
            )
        ));
    }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is CalculationSubevent calculationSubevent) {
            calculationSubevent.SetBase(CalculationSubevent.ProcessFormulaJson(
                CalculationSubevent.SimplifyCalculationFormula(rpglEffect, subevent, functionJson.GetJsonObject("base"), context))
            );
        }
    }

};
