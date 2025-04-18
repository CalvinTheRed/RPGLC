﻿using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.math;

namespace com.rpglc.subevent;

public abstract class CalculationSubevent(string subeventId) : Subevent(subeventId) {
    
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
        return (long) json.GetJsonObject("base").GetLong("value");
    }

    public CalculationSubevent SetBase(long baseValue) {
        JsonObject baseJson = json.GetJsonObject("base");
        if (baseJson is null) {
            json.PutJsonObject("base", new JsonObject().PutLong("value", baseValue));
        } else {
            baseJson.PutLong("value", baseValue);
        }
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
        JsonObject? minimumJson = json.GetJsonObject("minimum");
        if (minimumJson is null) {
            json.PutJsonObject("minimum", new JsonObject().PutLong("value", 0L));
            return 0L;
        } else {
            return minimumJson.GetLong("value") ?? 0L;
        }
    }

    public CalculationSubevent SetMinimum(long minimumValue) {
        long currentMinimum = GetMinimum();
        if (minimumValue > currentMinimum) {
            json.GetJsonObject("minimum").PutLong("value", minimumValue);
        }
        return this;
    }

    public CalculationSubevent PrepareBase(RPGLContext context) {
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
