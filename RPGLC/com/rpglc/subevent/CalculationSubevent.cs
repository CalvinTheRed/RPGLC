using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.math;
using com.rpglc.runtime;

namespace com.rpglc.subevent;

public abstract class CalculationSubevent : Subevent {

    private int bonusIndex = 0;

    public CalculationSubevent(string subeventId) : base(subeventId) {
        json.PutIfAbsent("bonuses", new JsonArray());
        this.subeventSteps.AddRange([
            (context) => {
                json.PutIfAbsent("base", new JsonObject().LoadFromString("""
                    {
                        "formula": "number",
                        "number": 0
                    }
                    """));

                json.PutJsonArray(
                    "bonus_formulas",
                    json.AsDict().ContainsKey("bonuses")
                        ? json.RemoveJsonArray("bonuses")
                        : new()
                );
                json.PutJsonArray("bonuses", new());

                json.PutIfAbsent("minimum", new JsonObject().LoadFromString($$"""
                    {
                        "formula": "number",
                        "number": {{long.MinValue}}
                    }
                    """));
                return new() {
                    dependency = null,
                    nextPhase = null,
                    stepCompleted = true,
                };
            },
            AdvanceBase,
            AdvanceBonuses,
            AdvanceMinimum,
        ]);
    }

    public SubeventState.StateData AdvanceBase(RPGLContext context) {
        string? formula = json.GetJsonObject("base").GetString("formula");
        if (formula == "number") {
            AdvanceBaseNumber();
        } else if (formula == "modifier") {
            AdvanceBaseModifier(context);
        } else if (formula == "ability") {
            AdvanceBaseAbility(context);
        } else if (formula == "proficiency") {
            AdvanceBaseProficiency(context);
        } else if (formula == "level") {
            AdvanceBaseLevel();
        }
        return new() {
            dependency = dependency,
            nextPhase = null,
            stepCompleted = dependency is null,
        };
    }

    public void AdvanceBaseNumber() {
        SetBase((long) json.GetJsonObject("base").GetLong("number"));
    }

    public void AdvanceBaseModifier(RPGLContext context) {
        JsonObject baseJson = json.GetJsonObject("base");
        RPGLObject rpglObject = RPGLEffect.GetObject(null, this, baseJson.GetJsonObject("object"));
        if (dependency is null) {
            dependency = new(new CalculateAbilityScore()
                .SetOriginItem(GetOriginItem())
                .SetSource(rpglObject)
                .SetTarget(rpglObject)
                .JoinSubeventData(new JsonObject().LoadFromString($$"""
                    {
                        "tags": {{rpglObject.GetTags()}},
                        "ability": "{{baseJson.GetString("ability")}}"
                    }
                    """)));
        } else {
            SetBase(Scale(
                RPGLObject.GetAbilityModifierFromAbilityScore((dependency.subevent as CalculationSubevent).Get()),
                baseJson.GetJsonObject("scale") ?? new JsonObject().LoadFromString($$"""
                    {
                        "numerator": 1,
                        "denominator": 1,
                        "round_up": false
                    }
                    """)
            ));
            dependency = null;
        }
    }

    public void AdvanceBaseAbility(RPGLContext context) {
        JsonObject baseJson = json.GetJsonObject("base");
        RPGLObject rpglObject = RPGLEffect.GetObject(null, this, baseJson.GetJsonObject("object"));
        if (dependency is null) {
            dependency = new(new CalculateAbilityScore()
                .SetOriginItem(GetOriginItem())
                .SetSource(rpglObject)
                .SetTarget(rpglObject)
                .JoinSubeventData(new JsonObject().LoadFromString($$"""
                    {
                        "tags": {{rpglObject.GetTags()}},
                        "ability": "{{baseJson.GetString("ability")}}"
                    }
                    """)));
        } else {
            SetBase(Scale(
                (dependency.subevent as CalculationSubevent).Get(),
                baseJson.GetJsonObject("scale") ?? new JsonObject().LoadFromString($$"""
                    {
                        "numerator": 1,
                        "denominator": 1,
                        "round_up": false
                    }
                    """)
            ));
            dependency = null;
        }
    }

    public void AdvanceBaseProficiency(RPGLContext context) {
        JsonObject baseJson = json.GetJsonObject("base");
        RPGLObject rpglObject = RPGLEffect.GetObject(null, this, baseJson.GetJsonObject("object"));
        if (dependency is null) {
            dependency = new(new CalculateProficiencyBonus()
                .SetOriginItem(GetOriginItem())
                .SetSource(rpglObject)
                .SetTarget(rpglObject)
                .JoinSubeventData(new JsonObject().LoadFromString($$"""
                    {
                        "tags": {{rpglObject.GetTags()}}
                    }
                    """)));
        } else {
            SetBase(Scale(
                (dependency.subevent as CalculateProficiencyBonus).Get(),
                baseJson.GetJsonObject("scale") ?? new JsonObject().LoadFromString($$"""
                    {
                        "numerator": 1,
                        "denominator": 1,
                        "round_up": false
                    }
                    """)
            ));
            dependency = null;
        }
    }

    public void AdvanceBaseLevel() {
        JsonObject baseJson = json.GetJsonObject("base");
        RPGLObject rpglObject = RPGLEffect.GetObject(null, this, baseJson.GetJsonObject("object"));
        string classDatapackId = baseJson.GetString("class") ?? "*";
        SetBase(Scale(
            classDatapackId == "*" ? rpglObject.GetLevel() : rpglObject.GetLevel(classDatapackId),
            baseJson.GetJsonObject("scale") ?? new JsonObject().LoadFromString($$"""
                {
                    "numerator": 1,
                    "denominator": 1,
                    "round_up": false
                }
                """)
        ));
    }

    public SubeventState.StateData AdvanceBonuses(RPGLContext context) {
        JsonArray bonusArray = json.GetJsonArray("bonus_formulas");
        if (bonusIndex < bonusArray.Count()) {
            JsonObject bonusJson = bonusArray.GetJsonObject(bonusIndex);
            bonusIndex++;

            string? formula = bonusJson.GetString("formula");
            if (formula == "number") {
                AdvanceBonusesNumber(bonusJson);
            } else if (formula == "dice") {
                AdvanceBonusesDice(bonusJson);
            } else if (formula == "modifier") {
                AdvanceBonusesModifier(bonusJson, context);
            } else if (formula == "ability") {
                AdvanceBonusesAbility(bonusJson, context);
            } else if (formula == "proficiency") {
                AdvanceBonusesProficiency(bonusJson, context);
            } else if (formula == "level") {
                AdvanceBonusesLevel(bonusJson);
            }
            return new() {
                dependency = dependency,
                nextPhase = null,
                stepCompleted = dependency is null && bonusIndex == bonusArray.Count(),
            };
        }
        return new() {
            dependency = null,
            nextPhase = null,
            stepCompleted = true,
        };
    }

    public void AdvanceBonusesNumber(JsonObject bonusJson) {
        AddBonus(new JsonObject().LoadFromString($$"""
            {
                "bonus": {{bonusJson.GetLong("number")}},
                "dice": [ ],
                "scale": {{bonusJson.GetJsonObject("scale")?.ToString() ?? $$"""
                {
                    "numerator": 1,
                    "denominator": 1,
                    "round_up": false
                }
                """}}
            }
            """));
    }

    public void AdvanceBonusesDice(JsonObject bonusJson) {
        AddBonus(new JsonObject().LoadFromString($$"""
            {
                "bonus": 0,
                "dice": {{Die.Unpack(bonusJson.GetJsonArray("dice"))}},
                "scale": {{bonusJson.GetJsonObject("scale")?.ToString() ?? $$"""
                {
                    "numerator": 1,
                    "denominator": 1,
                    "round_up": false
                }
                """}}
            }
            """));
    }

    public void AdvanceBonusesModifier(JsonObject bonusJson, RPGLContext context) {
        RPGLObject rpglObject = RPGLEffect.GetObject(null, this, bonusJson.GetJsonObject("object"));
        if (dependency is null) {
            dependency = new(new CalculateAbilityScore()
                .SetOriginItem(GetOriginItem())
                .SetSource(rpglObject)
                .SetTarget(rpglObject)
                .JoinSubeventData(new JsonObject().LoadFromString($$"""
                    {
                        "tags": {{rpglObject.GetTags()}},
                        "ability": "{{bonusJson.GetString("ability")}}"
                    }
                    """)));
            bonusIndex--;
        } else {
            AddBonus(new JsonObject().LoadFromString($$"""
                {
                    "bonus": {{RPGLObject.GetAbilityModifierFromAbilityScore((dependency.subevent as CalculationSubevent).Get())}},
                    "dice": [ ],
                    "scale": {{bonusJson.GetJsonObject("scale")?.ToString() ?? $$"""
                    {
                        "numerator": 1,
                        "denominator": 1,
                        "round_up": false
                    }
                    """}}
                }
                """));
            dependency = null;
        }
    }

    public void AdvanceBonusesAbility(JsonObject bonusJson, RPGLContext context) {
        RPGLObject rpglObject = RPGLEffect.GetObject(null, this, bonusJson.GetJsonObject("object"));
        if (dependency is null) {
            dependency = new(new CalculateAbilityScore()
                .SetOriginItem(GetOriginItem())
                .SetSource(rpglObject)
                .SetTarget(rpglObject)
                .JoinSubeventData(new JsonObject().LoadFromString($$"""
                    {
                        "tags": {{rpglObject.GetTags()}},
                        "ability": "{{bonusJson.GetString("ability")}}"
                    }
                    """)));
            bonusIndex--;
        } else {
            AddBonus(new JsonObject().LoadFromString($$"""
                {
                    "bonus": {{(dependency.subevent as CalculationSubevent).Get()}},
                    "dice": [ ],
                    "scale": {{bonusJson.GetJsonObject("scale")?.ToString() ?? $$"""
                    {
                        "numerator": 1,
                        "denominator": 1,
                        "round_up": false
                    }
                    """}}
                }
                """));
            dependency = null;
        }
    }

    public void AdvanceBonusesProficiency(JsonObject bonusJson, RPGLContext context) {
        RPGLObject rpglObject = RPGLEffect.GetObject(null, this, bonusJson.GetJsonObject("object"));
        if (dependency is null) {
            dependency = new(new CalculateProficiencyBonus()
                .SetOriginItem(GetOriginItem())
                .SetSource(rpglObject)
                .SetTarget(rpglObject)
                .JoinSubeventData(new JsonObject().LoadFromString($$"""
                    {
                        "tags": {{rpglObject.GetTags()}}
                    }
                    """)));
            bonusIndex--;
        } else {
            AddBonus(new JsonObject().LoadFromString($$"""
                {
                    "bonus": {{(dependency.subevent as CalculationSubevent).Get()}},
                    "dice": [ ],
                    "scale": {{bonusJson.GetJsonObject("scale")?.ToString() ?? $$"""
                    {
                        "numerator": 1,
                        "denominator": 1,
                        "round_up": false
                    }
                    """}}
                }
                """));
            dependency = null;
        }
    }

    public void AdvanceBonusesLevel(JsonObject bonusJson) {
        RPGLObject rpglObject = RPGLEffect.GetObject(null, this, bonusJson.GetJsonObject("object"));
        string classDatapackId = bonusJson.GetString("class") ?? "*";

        AddBonus(new JsonObject().LoadFromString($$"""
            {
                "bonus": {{(classDatapackId == "*" ? rpglObject.GetLevel() : rpglObject.GetLevel(classDatapackId))}},
                "dice": [ ],
                "scale": {{bonusJson.GetJsonObject("scale")?.ToString() ?? $$"""
                {
                    "numerator": 1,
                    "denominator": 1,
                    "round_up": false
                }
                """}}
            }
            """));
    }

    public SubeventState.StateData AdvanceMinimum(RPGLContext context) {
        string? formula = json.GetJsonObject("minimum").GetString("formula");
        if (formula == "number") {
            AdvanceMinimumNumber();
        } else if (formula == "modifier") {
            AdvanceMinimumModifier(context);
        } else if (formula == "ability") {
            AdvanceMinimumAbility(context);
        } else if (formula == "proficiency") {
            AdvanceMinimumProficiency(context);
        } else if (formula == "level") {
            AdvanceMinimumLevel();
        }
        return new() {
            dependency = dependency,
            nextPhase = null,
            stepCompleted = dependency is null,
        };
    }

    public void AdvanceMinimumNumber() {
        SetMinimum((long) json.GetJsonObject("minimum").GetLong("number"));
    }

    public void AdvanceMinimumModifier(RPGLContext context) {
        JsonObject minimumJson = json.GetJsonObject("minimum");
        RPGLObject rpglObject = RPGLEffect.GetObject(null, this, minimumJson.GetJsonObject("object"));
        if (dependency is null) {
            dependency = new(new CalculateAbilityScore()
                .SetOriginItem(GetOriginItem())
                .SetSource(rpglObject)
                .SetTarget(rpglObject)
                .JoinSubeventData(new JsonObject().LoadFromString($$"""
                    {
                        "tags": {{rpglObject.GetTags()}},
                        "ability": "{{minimumJson.GetString("ability")}}"
                    }
                    """)));
        } else {
            SetMinimum(Scale(
                RPGLObject.GetAbilityModifierFromAbilityScore((dependency.subevent as CalculationSubevent).Get()),
                minimumJson.GetJsonObject("scale") ?? new JsonObject().LoadFromString($$"""
                    {
                        "numerator": 1,
                        "denominator": 1,
                        "round_up": false
                    }
                    """)
            ));
            dependency = null;
        }
    }

    public void AdvanceMinimumAbility(RPGLContext context) {
        JsonObject minimumJson = json.GetJsonObject("minimum");
        RPGLObject rpglObject = RPGLEffect.GetObject(null, this, minimumJson.GetJsonObject("object"));
        if (dependency is null) {
            dependency = new(new CalculateAbilityScore()
                .SetOriginItem(GetOriginItem())
                .SetSource(rpglObject)
                .SetTarget(rpglObject)
                .JoinSubeventData(new JsonObject().LoadFromString($$"""
                    {
                        "tags": {{rpglObject.GetTags()}},
                        "ability": "{{minimumJson.GetString("ability")}}"
                    }
                    """)));
        } else {
            SetMinimum(Scale(
                (dependency.subevent as CalculationSubevent).Get(),
                minimumJson.GetJsonObject("scale") ?? new JsonObject().LoadFromString($$"""
                    {
                        "numerator": 1,
                        "denominator": 1,
                        "round_up": false
                    }
                    """)
            ));
            dependency = null;
        }
    }

    public void AdvanceMinimumProficiency(RPGLContext context) {
        JsonObject minimumJson = json.GetJsonObject("minimum");
        RPGLObject rpglObject = RPGLEffect.GetObject(null, this, minimumJson.GetJsonObject("object"));
        if (dependency is null) {
            dependency = new(new CalculateProficiencyBonus()
                .SetOriginItem(GetOriginItem())
                .SetSource(rpglObject)
                .SetTarget(rpglObject)
                .JoinSubeventData(new JsonObject().LoadFromString($$"""
                    {
                        "tags": {{rpglObject.GetTags()}}
                    }
                    """)));
        } else {
            SetMinimum(Scale(
                (dependency.subevent as CalculateProficiencyBonus).Get(),
                minimumJson.GetJsonObject("scale") ?? new JsonObject().LoadFromString($$"""
                    {
                        "numerator": 1,
                        "denominator": 1,
                        "round_up": false
                    }
                    """)
            ));
            dependency = null;
        }
    }

    public void AdvanceMinimumLevel() {
        JsonObject minimumJson = json.GetJsonObject("minimum");
        RPGLObject rpglObject = RPGLEffect.GetObject(null, this, minimumJson.GetJsonObject("object"));
        string classDatapackId = minimumJson.GetString("class") ?? "*";
        SetMinimum(Scale(
            classDatapackId == "*" ? rpglObject.GetLevel() : rpglObject.GetLevel(classDatapackId),
            minimumJson.GetJsonObject("scale") ?? new JsonObject().LoadFromString($$"""
                {
                    "numerator": 1,
                    "denominator": 1,
                    "round_up": false
                }
                """)
        ));
    }

    public override CalculationSubevent Prepare(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return this
            .PrepareBase(context)
            .PrepareBonuses(context)
            .PrepareMinimum(context);
    }

    public long Get() {
        return Math.Max(GetBase() + GetBonus(), GetMinimum());
    }

    public long GetBase() {
        return json.GetLong("base") ?? 0L;
    }

    public CalculationSubevent SetBase(long baseValue) {
        json.PutLong("base", baseValue);
        return this;
    }

    public JsonArray GetBonuses() {
        return json.GetJsonArray("bonuses");
    }

    public CalculationSubevent AddBonus(JsonObject bonusJson) {
        GetBonuses().AddJsonObject(bonusJson);
        return this;
    }

    public long GetBonus() {
        long totalBonus = 0L;
        JsonArray bonuses = GetBonuses();
        for (int i = 0; i < bonuses.Count(); i++) {
            JsonObject bonusJson = bonuses.GetJsonObject(i);
            long bonusValue = (long) bonusJson.GetLong("bonus");
            JsonArray dice = bonusJson.GetJsonArray("dice");
            for (int j = 0; j < dice.Count(); j++) {
                JsonObject die = dice.GetJsonObject(j);
                bonusValue += die.GetLong("roll") ?? Die.Roll(die);
            }
            totalBonus += Scale(bonusValue, bonusJson.GetJsonObject("scale"));
        }
        return totalBonus;
    }

    public long GetMinimum() {
        return json.GetLong("minimum") ?? long.MinValue;
    }

    public CalculationSubevent SetMinimum(long minimumValue) {
        json.PutLong("minimum", minimumValue);
        return this;
    }

    public CalculationSubevent PrepareBase(RPGLContext context) {
        // TODO what is the purpose of this?
        JsonObject? baseJson = json.GetJsonObject("base");
        SetBase(0);
        if (baseJson is not null) {
            RPGLEffect rpglEffect = new RPGLEffect()
                .SetSource(GetSource().GetUuid())
                .SetTarget(GetTarget().GetUuid());
            SetBase(ProcessFormulaJson(SimplifyCalculationFormula(rpglEffect, this, baseJson, context)));
        }
        return this;
    }

    public CalculationSubevent PrepareBonuses(RPGLContext context) {
        JsonArray? bonuses = json.RemoveJsonArray("bonuses");
        json.PutJsonArray("bonuses", new());
        if (bonuses is not null) {
            RPGLEffect rpglEffect = new RPGLEffect()
                .SetSource(GetSource().GetUuid())
                .SetTarget(GetTarget().GetUuid());
            for (int i = 0; i < bonuses.Count(); i++) {
                JsonObject bonusJson = bonuses.GetJsonObject(i);
                AddBonus(SimplifyCalculationFormula(rpglEffect, this, bonusJson, context));
            }
        }
        return this;
    }

    public CalculationSubevent PrepareMinimum(RPGLContext context) {
        // TODO what is the purpose of this?
        JsonObject? minimumJson = json.RemoveJsonObject("minimum");
        SetMinimum(long.MinValue);
        if (minimumJson is not null) {
            RPGLEffect rpglEffect = new RPGLEffect()
                .SetSource(GetSource().GetUuid())
                .SetTarget(GetTarget().GetUuid());
            SetMinimum(ProcessFormulaJson(SimplifyCalculationFormula(rpglEffect, this, minimumJson, context)));
        }
        return this;
    }

    public static long Scale(long value, JsonObject scaleJson) {
        // TODO possible to abstract out numerator and denominator to use calculation formulae?
        // Would make CalculateMaximumHitPoints less hard-coded, and make other features easier to implement.
        bool roundUp = scaleJson.GetBool("round_up") ?? false;
        if (roundUp) {
            return (long) Math.Ceiling((decimal) value 
                * (scaleJson.GetLong("numerator") ?? 1L)
                / (scaleJson.GetLong("denominator") ?? 1L)
            );
        } else {
            return value
                * (scaleJson.GetLong("numerator") ?? 1L)
                / (scaleJson.GetLong("denominator") ?? 1L);
        }
    }

    /// <summary>
    /// <b>Calculation Formulae</b>
    /// <br />
    /// The following is a list of calculation formulae recognized by RPGLC. Note that all "scale" fields are optional, and will default to a value of
    /// <code>
    /// {
    ///   "numerator": 1,
    ///   "denominator": 1,
    ///   "round_up": false
    /// }
    /// </code>
    /// if not specified. Note also that if these formulas are used to represent damage values, each must have an additional "damage_type" field storing a string representing the damage type being added.
    /// 
    /// <br /><br />
    /// <b>range</b>
    /// <br />
    /// Returns a determined number.
    /// <code>
    /// {
    ///   "formula": "number",
    ///   "number": &lt;long&gt;,
    ///   "scale": {
    ///     "numerator": &lt;long&gt;,
    ///     "denominator": &lt;long&gt;,
    ///     "round_up": &lt;bool = false&gt;
    ///   }
    /// }
    /// </code>
    /// 
    /// <br /><br />
    /// <b>dice</b>
    /// <br />
    /// Returns a set of dice.
    /// <code>
    /// {
    ///   "formula": "dice",
    ///   "dice": [
    ///     { "count": &lt;long&gt;, "size": &lt;long&gt; }
    ///   ],
    ///   "scale": {
    ///     "numerator": &lt;long&gt;,
    ///     "denominator": &lt;long&gt;,
    ///     "round_up": &lt;bool = false&gt;
    ///   }
    /// }
    /// </code>
    /// 
    /// <br /><br />
    /// <b>modifier</b>
    /// <br />
    /// Calculates an object's modifier for a given ability.
    /// <code>
    /// {
    ///   "formula": "modifier",
    ///   "ability": &lt;string&gt;,
    ///   "object": {
    ///     "from": "effect" | "subevent",
    ///     "object": "source" | "target",
    ///     "as_origin": &lt;bool = false&gt;
    ///   },
    ///   "scale": {
    ///     "numerator": &lt;long&gt;,
    ///     "denominator": &lt;long&gt;,
    ///     "round_up": &lt;bool = false&gt;
    ///   }
    /// }
    /// </code>
    /// 
    /// <br /><br />
    /// <b>ability</b>
    /// <br />
    /// Calculates an object's score for a given ability.
    /// <code>
    /// {
    ///   "formula": "ability",
    ///   "ability": &lt;string&gt;,
    ///   "object": {
    ///     "from": "effect" | "subevent",
    ///     "object": "source" | "target",
    ///     "as_origin": &lt;bool = false&gt;
    ///   },
    ///   "scale": {
    ///     "numerator": &lt;long&gt;,
    ///     "denominator": &lt;long&gt;,
    ///     "round_up": &lt;bool = false&gt;
    ///   }
    /// }
    /// </code>
    /// 
    /// <br /><br />
    /// <b>proficiency</b>
    /// <br />
    /// Calculates an object's proficiency bonus.
    /// <code>
    /// {
    ///   "formula": "proficiency",
    ///   "object": {
    ///     "from": "effect" | "subevent",
    ///     "object": "source" | "target",
    ///     "as_origin": &lt;bool = false&gt;
    ///   },
    ///   "scale": {
    ///     "numerator": &lt;long&gt;,
    ///     "denominator": &lt;long&gt;,
    ///     "round_up": &lt;bool = false&gt;
    ///   }
    /// }
    /// </code>
    /// 
    /// <br /><br />
    /// <b>level</b>
    /// <br />
    /// Calculates an object's level in a specified class.
    /// <code>
    /// {
    ///   "formula": "level",
    ///   "class": &lt;string = "*"&gt;,
    ///   "object": {
    ///     "from": "effect" | "subevent",
    ///     "object": "source" | "target",
    ///     "as_origin": &lt;bool = false&gt;
    ///   },
    ///   "scale": {
    ///     "numerator": &lt;long&gt;,
    ///     "denominator": &lt;long&gt;,
    ///     "round_up": &lt;bool = false&gt;
    ///   }
    /// }
    /// </code>
    /// </summary>
    /// <param name="rpglEffect"></param>
    /// <param name="subevent"></param>
    /// <param name="formulaJson"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public static JsonObject SimplifyCalculationFormula(RPGLEffect rpglEffect, Subevent subevent, JsonObject formulaJson, RPGLContext context) {
        string formula = formulaJson.GetString("formula");
        if (formula == "number") {
            return new JsonObject()
                .PutLong("bonus", formulaJson.GetLong("number"))
                .PutJsonArray("dice", new())
                .PutJsonObject("scale", formulaJson.GetJsonObject("scale") ?? new JsonObject()
                    .PutLong("numerator", 1L)
                    .PutLong("denominator", 1L)
                    .PutBool("round_up", false)
                );
        } else if (formula == "dice") {
            return new JsonObject()
                .PutLong("bonus", 0L)
                .PutJsonArray("dice", Die.Unpack(formulaJson.GetJsonArray("dice")))
                .PutJsonObject("scale", formulaJson.GetJsonObject("scale") ?? new JsonObject()
                    .PutLong("numerator", 1L)
                    .PutLong("denominator", 1L)
                    .PutBool("round_up", false)
                );
        } else if (formula == "modifier") {
            RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, subevent, formulaJson.GetJsonObject("object"));
            return new JsonObject()
                .PutLong("bonus", rpglObject.GetAbilityModifierFromAbilityName(formulaJson.GetString("ability"), context))
                .PutJsonArray("dice", new())
                .PutJsonObject("scale", formulaJson.GetJsonObject("scale") ?? new JsonObject()
                    .PutLong("numerator", 1L)
                    .PutLong("denominator", 1L)
                    .PutBool("round_up", false)
                );
        } else if (formula == "ability") {
            RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, subevent, formulaJson.GetJsonObject("object"));
            return new JsonObject()
                .PutLong("bonus", rpglObject.GetAbilityScoreFromAbilityName(formulaJson.GetString("ability"), context))
                .PutJsonArray("dice", new())
                .PutJsonObject("scale", formulaJson.GetJsonObject("scale") ?? new JsonObject()
                    .PutLong("numerator", 1L)
                    .PutLong("denominator", 1L)
                    .PutBool("round_up", false)
                );
        } else if (formula == "proficiency") {
            RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, subevent, formulaJson.GetJsonObject("object"));
            return new JsonObject()
                .PutLong("bonus", rpglObject.GetEffectiveProficiencyBonus(context))
                .PutJsonArray("dice", new())
                .PutJsonObject("scale", formulaJson.GetJsonObject("scale") ?? new JsonObject()
                    .PutLong("numerator", 1L)
                    .PutLong("denominator", 1L)
                    .PutBool("round_up", false)
                );
        } else if (formula == "level") {
            RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, subevent, formulaJson.GetJsonObject("object"));
            string classDatapackId = formulaJson.GetString("class") ?? "*";
            return new JsonObject()
                .PutLong("bonus", classDatapackId == "*"
                    ? rpglObject.GetLevel()
                    : rpglObject.GetLevel(classDatapackId)
                )
                .PutJsonArray("dice", new())
                .PutJsonObject("scale", formulaJson.GetJsonObject("scale") ?? new JsonObject()
                    .PutLong("numerator", 1L)
                    .PutLong("denominator", 1L)
                    .PutBool("round_up", false)
                );
        } else {
            return new JsonObject()
                .PutLong("bonus", 1L)
                .PutJsonArray("dice", new())
                .PutJsonObject("scale", formulaJson.GetJsonObject("scale") ?? new JsonObject()
                    .PutLong("numerator", 1L)
                    .PutLong("denominator", 1L)
                    .PutBool("round_up", false)
                );
        }
    }

    public static long ProcessFormulaJson(JsonObject formulaJson) {
        long value = formulaJson.GetLong("bonus") ?? 0L;
        JsonArray diceArray = formulaJson.GetJsonArray("dice") ?? new();
        for (int i = 0; i < diceArray.Count(); i++) {
            JsonObject dieObject = diceArray.GetJsonObject(i);
            Die.Roll(dieObject);
            value += (long) dieObject.GetLong("roll");
        }
        return Scale(value, formulaJson.GetJsonObject("scale") ?? new());
    }

};
