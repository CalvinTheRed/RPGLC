using com.rpglc.condition;
using com.rpglc.function;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.core;

public class RPGLEffect : TaggableContent {

    // =====================================================================
    // Static code.
    // =====================================================================

    private static readonly List<RPGLEffect> RPGL_EFFECTS = [];

    public static void AddRPGLEffect(RPGLEffect rpglEffect) {
        RPGL_EFFECTS.Add(rpglEffect);
    }

    public static void RemoveRPGLEffect(RPGLEffect rpglEffect) {
        RPGL_EFFECTS.Remove(rpglEffect);
    }

    public static List<RPGLEffect> GetRPGLEffects() {
        return [.. RPGL_EFFECTS];
    }

    public static void ClearRPGL() {
        RPGL_EFFECTS.Clear();
    }

    // =====================================================================
    // Instance code.
    // =====================================================================

    public JsonObject GetSubeventFilters() {
        return GetJsonObject("subevent_filters");
    }

    public RPGLEffect SetSubeventFilters(JsonObject subeventFilters) {
        PutJsonObject("subevent_filters", subeventFilters);
        return this;
    }

    public string? GetOriginItem() {
        return GetString("origin_item");
    }

    public RPGLEffect SetOriginItem(string? originItem) {
        PutString("origin_item", originItem);
        return this;
    }

    public string GetSource() {
        return GetString("source");
    }

    public RPGLEffect SetSource(string? source) {
        PutString("source", source);
        return this;
    }

    public string GetTarget() {
        return GetString("target");
    }

    public RPGLEffect SetTarget(string? target) {
        PutString("target", target);
        return this;
    }

    // =====================================================================
    // Utility methods.
    // =====================================================================

    public bool ProcessSubevent(Subevent subevent, RPGLContext context, JsonArray originPoint) {
        JsonObject subeventFilters = GetSubeventFilters();
        foreach (string subeventId in subeventFilters.AsDict().Keys) {
            if (subevent.GetSubeventId() == subeventId) {
                JsonArray matchedFilterBehaviors = subeventFilters.GetJsonArray(subeventId);
                for (int i = 0; i < matchedFilterBehaviors.Count(); i++) {
                    JsonObject matchedFilterBehavior = matchedFilterBehaviors.GetJsonObject(i);
                    JsonArray conditions = matchedFilterBehavior.GetJsonArray("conditions");
                    if (!subevent.IsEffectApplied(this) && EvaluateConditions(subevent, conditions, context, originPoint)) {
                        ExecuteFunctions(subevent, matchedFilterBehavior.GetJsonArray("functions"), context, originPoint);
                        subevent.AddModifyingEffect(this);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public bool EvaluateConditions(Subevent subevent, JsonArray conditions, RPGLContext context, JsonArray originPoint) {
        bool conditionsMet = true;
        for (int i = 0; i < conditions.Count(); i++) {
            JsonObject conditionJson = conditions.GetJsonObject(i);
            conditionsMet &= Condition.Conditions[conditionJson.GetString("condition")]
                .Evaluate(this, subevent, conditionJson, context, originPoint);
        }
        return conditionsMet;
    }

    public void ExecuteFunctions(Subevent subevent, JsonArray functions, RPGLContext context, JsonArray originPoint) {
        for (int i = 0; i < functions.Count(); i++) {
            JsonObject functionJson = functions.GetJsonObject(i);
            Function.Functions[functionJson.GetString("function")]
                .Execute(this, subevent, functionJson, context, originPoint);
        }
    }

    public static RPGLObject? GetObject(RPGLEffect rpglEffect, Subevent subevent, JsonObject instructions) {
        string fromAlias = instructions.GetString("from");
        string objectAlias = instructions.GetString("object");
        RPGLObject? rpglObject = null;
        if (fromAlias == "subevent") {
            if (objectAlias == "source") {
                rpglObject = subevent.GetSource();
            } else if (objectAlias == "target") {
                rpglObject = subevent.GetTarget();
            }
        } else if (fromAlias == "effect") {
            if (objectAlias == "source") {
                rpglObject = RPGLObject.GetRPGLObjects().Find(x => x.GetUuid() == rpglEffect.GetSource());
            } else if (objectAlias == "target") {
                rpglObject = RPGLObject.GetRPGLObjects().Find(x => x.GetUuid() == rpglEffect.GetTarget());
            }
        }
        if (instructions.GetBool("as_origin") ?? false) {
            rpglObject = RPGLObject.GetRPGLObjects().Find(x => x.GetUuid() == rpglObject.GetOriginObject());
        }
        return rpglObject;
    }

};
