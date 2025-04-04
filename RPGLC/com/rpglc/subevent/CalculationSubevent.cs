using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.math;

namespace com.rpglc.subevent;

public abstract class CalculationSubevent(string subeventId) : Subevent(subeventId) {
    
    public override CalculationSubevent Prepare(RPGLContext context, JsonArray originPoint) {
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
            SetBase(ProcessSetJson(rpglEffect, this, baseJson, context));
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
                AddBonus(ProcessBonusJson(rpglEffect, this, bonusJson, context));
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
            SetMinimum(ProcessSetJson(rpglEffect, this, minimumJson, context));
        }
        return this;
    }

    public static long Scale(long value, JsonObject scaleJson) {
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

    public static JsonObject ProcessBonusJson(RPGLEffect rpglEffect, Subevent subevent, JsonObject formulaJson, RPGLContext context) {
        /*[
            {
                "formula": "range",
                "bonus": #,
                "dice": [
                    { "count": #, "size": #, "determined": [ # ] }
                ],
                "scale": {
                    "numerator": #,
                    "denominator": #,
                    "round_up": t/f
                }
            },
            {
                "formula": "modifier",
                "ability": "...",
                "object": {
                    "from": "effect" | "subevent",
                    "object": "source" | "target",
                    "as_origin": t/f
                },
                "scale": {
                    "numerator": #,
                    "denominator": #,
                    "round_up": t/f
                }
            },
            {
                "formula": "ability",
                "ability": "...",
                "object": {
                    "from": "effect" | "subevent",
                    "object": "source" | "target",
                    "as_origin": t/f
                },
                "scale": {
                    "numerator": #,
                    "denominator": #,
                    "round_up": t/f
                }
            },
            {
                "formula": "proficiency",
                "object": {
                    "from": "effect" | "subevent",
                    "object": "source" | "target",
                    "as_origin": t/f
                },
                "scale": {
                    "numerator": #,
                    "denominator": #,
                    "round_up": t/f
                }
            },
            {
                "formula": "level",
                "class": "...",
                "object": {
                    "from": "effect" | "subevent",
                    "object": "source" | "target",
                    "as_origin": t/f
                },
                "scale": {
                    "numerator": #,
                    "denominator": #,
                    "round_up": t/f
                }
            }
        ]*/
        string formula = formulaJson.GetString("formula");
        if (formula == "range") {
            return new JsonObject()
                .PutLong("bonus", formulaJson.GetLong("bonus") ?? 0L)
                .PutJsonArray("dice", Die.Unpack(formulaJson.GetJsonArray("dice") ?? new()))
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

    public static long ProcessSetJson(RPGLEffect rpglEffect, Subevent subevent, JsonObject formulaJson, RPGLContext context) {
        /*[
            {
                "formula": "number",
                "number": #,
                "scale": {
                    "numerator": #,
                    "denominator": #,
                    "round_up": t/f
                }
            },
            {
                "formula": "modifier",
                "ability": "...",
                "object": {
                    "from": "effect" | "subevent",
                    "object": "source" | "target",
                    "as_origin": t/f
                },
                "scale": {
                    "numerator": #,
                    "denominator": #,
                    "round_up": t/f
                }
            },
            {
                "formula": "ability",
                "ability": "...",
                "object": {
                    "from": "effect" | "subevent",
                    "object": "source" | "target",
                    "as_origin": t/f
                },
                "scale": {
                    "numerator": #,
                    "denominator": #,
                    "round_up": t/f
                }
            },
            {
                "formula": "proficiency",
                "object": {
                    "from": "effect" | "subevent",
                    "object": "source" | "target",
                    "as_origin": t/f
                },
                "scale": {
                    "numerator": #,
                    "denominator": #,
                    "round_up": t/f
                }
            },
            {
                "formula": "level",
                "class": "...",
                "object": {
                    "from": "effect" | "subevent",
                    "object": "source" | "target",
                    "as_origin": t/f
                },
                "scale": {
                    "numerator": #,
                    "denominator": #,
                    "round_up": t/f
                }
            }
        ]*/
        long setValue;
        string formula = formulaJson.GetString("formula");
        if (formula == "number") {
            setValue = (long) formulaJson.GetLong("number");
        } else if (formula == "modifier") {
            setValue = RPGLEffect
                .GetObject(rpglEffect, subevent, formulaJson.GetJsonObject("object"))
                .GetAbilityModifierFromAbilityName(formulaJson.GetString("ability"), context);
        } else if (formula == "ability") {
            setValue = RPGLEffect
                .GetObject(rpglEffect, subevent, formulaJson.GetJsonObject("object"))
                .GetAbilityScoreFromAbilityName(formulaJson.GetString("ability"), context);
        } else if (formula == "proficiency") {
            setValue = RPGLEffect
                .GetObject(rpglEffect, subevent, formulaJson.GetJsonObject("object"))
                .GetEffectiveProficiencyBonus(context);
        } else if (formula == "level") {
            string classDatapackId = formulaJson.GetString("class") ?? "*";
            if (classDatapackId == "*") {
                setValue = RPGLEffect
                    .GetObject(rpglEffect, subevent, formulaJson.GetJsonObject("object"))
                    .GetLevel();
            } else {
                setValue = RPGLEffect
                    .GetObject(rpglEffect, subevent, formulaJson.GetJsonObject("object"))
                    .GetLevel(classDatapackId);
            }
        } else {
            return 0L;
        }
        JsonObject scale = formulaJson.GetJsonObject("scale") ?? new ();
        return Scale(setValue, scale);
    }

};
