using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Sets the minimum value of a calculation subevent according to a defined formula.
///   
///   <code>
///   {
///     "function": "set_minimum",
///     "minimum": &lt;calculation formula&gt;
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"minimum" is a calculation formula that defines the minimum value for a calculation-type subevent. If the function's minimum formula provides a number less than the subevent's current minimum, nothing changes.</item>
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
public class SetMinimum : Function {

    public SetMinimum() : base("set_minimum") {
        functionSteps.AddRange([
            (rpglEffect, subevent, functionJson, context) => {
                if (subevent is CalculationSubevent calculationSubevent) {
                    JsonObject minimumJson = functionJson.GetJsonObject("minimum");
                    string? formula = minimumJson.GetString("formula");
                    if (formula == "number") {
                        AdvanceNumber(calculationSubevent, minimumJson);
                    } else if (formula == "modifier") {
                        AdvanceModifier(rpglEffect, calculationSubevent, minimumJson, context);
                    } else if (formula == "ability") {
                        AdvanceAbility(rpglEffect, calculationSubevent, minimumJson, context);
                    } else if (formula == "proficiency") {
                        AdvanceProficiency(rpglEffect, calculationSubevent, minimumJson, context);
                    } else if (formula == "level") {
                        AdvanceLevel(rpglEffect, calculationSubevent, minimumJson);
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

    public static void AdvanceNumber(CalculationSubevent calculationSubevent, JsonObject minimumJson) {
        calculationSubevent.SetMinimum(CalculationSubevent.Scale(
                (long) minimumJson.GetLong("number"),
                minimumJson.GetJsonObject("scale") ?? new JsonObject().LoadFromString($$"""
                    {
                        "numerator": 1,
                        "denominator": 1,
                        "round_up": false
                    }
                    """
                )
            ));
    }

    public void AdvanceModifier(RPGLEffect rpglEffect, CalculationSubevent calculationSubevent, JsonObject minimumJson, RPGLContext context) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, calculationSubevent, minimumJson.GetJsonObject("object"));
        if (this.dependency is null) {
            this.dependency = new CalculateAbilityScore()
                .JoinSubeventData(new JsonObject().LoadFromString($$"""
                    {
                        "tags": {{rpglObject.GetTags()}},
                        "ability": "{{minimumJson.GetString("ability")}}"
                    }
                    """
                ))
                .SetSource(rpglObject)
                .SetTarget(rpglObject);
        } else {
            calculationSubevent.SetMinimum(CalculationSubevent.Scale(
                RPGLObject.GetAbilityModifierFromAbilityScore((dependency as CalculationSubevent).Get()),
                minimumJson.GetJsonObject("scale") ?? new JsonObject().LoadFromString($$"""
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

    public void AdvanceAbility(RPGLEffect rpglEffect, CalculationSubevent calculationSubevent, JsonObject minimumJson, RPGLContext context) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, calculationSubevent, minimumJson.GetJsonObject("object"));
        if (this.dependency is null) {
            this.dependency = new CalculateAbilityScore()
                .JoinSubeventData(new JsonObject().LoadFromString($$"""
                    {
                        "tags": {{rpglObject.GetTags()}},
                        "ability": "{{minimumJson.GetString("ability")}}"
                    }
                    """
                ))
                .SetSource(rpglObject)
                .SetTarget(rpglObject);
        } else {
            calculationSubevent.SetMinimum(CalculationSubevent.Scale(
                (dependency as CalculationSubevent).Get(),
                minimumJson.GetJsonObject("scale") ?? new JsonObject().LoadFromString($$"""
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

    public void AdvanceProficiency(RPGLEffect rpglEffect, CalculationSubevent calculationSubevent, JsonObject minimumJson, RPGLContext context) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, calculationSubevent, minimumJson.GetJsonObject("object"));
        if (this.dependency is null) {
            this.dependency = new CalculateProficiencyBonus()
                .JoinSubeventData(new JsonObject().LoadFromString($$"""
                    {
                        "tags": {{rpglObject.GetTags()}}
                    }
                    """
                ))
                .SetSource(rpglObject)
                .SetTarget(rpglObject);
        } else {
            calculationSubevent.SetMinimum(CalculationSubevent.Scale(
                (dependency as CalculationSubevent).Get(),
                minimumJson.GetJsonObject("scale") ?? new JsonObject().LoadFromString($$"""
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

    public static void AdvanceLevel(RPGLEffect rpglEffect, CalculationSubevent calculationSubevent, JsonObject minimumJson) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, calculationSubevent, minimumJson.GetJsonObject("object"));
        string classDatapackId = minimumJson.GetString("class") ?? "*";

        calculationSubevent.SetMinimum(CalculationSubevent.Scale(
            classDatapackId == "*" ? rpglObject.GetLevel() : rpglObject.GetLevel(classDatapackId),
            minimumJson.GetJsonObject("scale") ?? new JsonObject().LoadFromString($$"""
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
            calculationSubevent.SetMinimum(CalculationSubevent.ProcessFormulaJson(
                CalculationSubevent.SimplifyCalculationFormula(rpglEffect, subevent, functionJson.GetJsonObject("minimum"), context))
            );
        }
    }

};
