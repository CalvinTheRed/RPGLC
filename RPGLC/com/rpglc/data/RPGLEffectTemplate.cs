using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.data;

public class RPGLEffectTemplate : RPGLTemplate {

    public RPGLEffectTemplate() : base() { }

    public RPGLEffectTemplate(JsonObject other) : base(other) { }

    public override RPGLEffectTemplate ApplyBonuses(JsonArray bonuses) {
        return new(base.ApplyBonuses(bonuses));
    }

    public RPGLEffect NewInstance(string uuid) {
        RPGLEffect rpglEffect = (RPGLEffect) new RPGLEffect().SetUuid(uuid);
        Setup(rpglEffect);
        ProcessInheritedEffects(rpglEffect, this);

        return rpglEffect;
    }

    private static void ProcessInheritedEffects(RPGLEffect rpglEffect, RPGLEffectTemplate template) {
        GenerateInheritedBehavior(rpglEffect.GetSubeventFilters(), template, [rpglEffect.GetDatapackId()]);
    }

    private static void GenerateInheritedBehavior(JsonObject subeventFilters, RPGLEffectTemplate template, List<string> inheritance) {
        JsonArray inheritedEffectIds = template.GetJsonArray("inherited_effects");
        for (int i = 0; i < inheritedEffectIds.Count(); i++) {
            string inheritedEffectId = inheritedEffectIds.GetString(i);
            if (inheritance.Contains(inheritedEffectId)) {
                // TODO log warning for circular inheritance and skip
                continue;
            }
            inheritance.Add(inheritedEffectId);
            RPGLEffectTemplate inheritedTemplate = RPGL.GetRPGLEffectTemplate(inheritedEffectId);
            if (inheritedTemplate is null) {
                // TODO log warning for missing dependency and skip
                continue;
            }
            JsonObject inheritedSubeventFilters = inheritedTemplate.GetJsonObject("subevent_filters");
            foreach (string key in  inheritedSubeventFilters.AsDict().Keys) {
                if (subeventFilters.AsDict().ContainsKey(key)) {
                    subeventFilters.GetJsonArray(key).AsList().AddRange(inheritedSubeventFilters.GetJsonArray(key).DeepClone().AsList());
                } else {
                    subeventFilters.PutJsonArray(key, inheritedSubeventFilters.GetJsonArray(key).DeepClone());
                }
            }
            GenerateInheritedBehavior(subeventFilters, inheritedTemplate, inheritance);
        }
    }

};
