using com.rpglc.data.TO;
using com.rpglc.json;

namespace com.rpglc.core;

public static class FeatureManager {

    public static void GrantGainedEffects(RPGLObject rpglObject, JsonObject gainedFeatures, JsonObject choices) {
        JsonArray gainedEffects = gainedFeatures.GetJsonArray("effects") ?? new();
        for (int i = 0; i < gainedEffects.Count(); i++) {
            var data = gainedEffects.AsList()[i];
            if (data is string effectDatapackId) {
                GrantGainedEffectFromString(rpglObject, effectDatapackId);
            } else if (data is Dictionary<string, object> dict) {
                GrantGainedEffectsFromObject(rpglObject, choices, new(dict));
            }
        }
    }

    private static void GrantGainedEffectFromString(RPGLObject rpglObject, string effectDatapackId) {
        _ = RPGLFactory.NewEffect(effectDatapackId,rpglObject.GetUuid(),rpglObject.GetUuid());
    }

    private static void GrantGainedEffectsFromObject(RPGLObject rpglObject, JsonObject choices, JsonObject gainedEffects) {
        string decisionName = gainedEffects.GetString("name");
        long numDecisions = gainedEffects.GetLong("count") ?? 1L;
        JsonArray options = gainedEffects.GetJsonArray("options");
        for (int i = 0; i < numDecisions; i++) {
            GrantGainedEffectFromString(
                rpglObject,
                options.GetString((int) choices.GetJsonArray(decisionName).GetLong(i))
            );
        }
    }

    public static void GrantGainedEvents(RPGLObject rpglObject, JsonObject gainedFeatures) {
        JsonArray gainedEvents = gainedFeatures.GetJsonArray("events") ?? new();
        rpglObject.GetEvents().AsList().AddRange(gainedEvents.AsList());
    }

    public static void GrantGainedResources(RPGLObject rpglObject, JsonObject gainedFeatures) {
        JsonArray gainedResources = gainedFeatures.GetJsonArray("resources") ?? new();
        for (int i = 0; i < gainedResources.Count(); i++) {
            var data = gainedResources.AsList()[i];
            if (data is string resourceDatapackId) {
                GrantGainedResourceFromString(rpglObject, resourceDatapackId);
            } else if (data is Dictionary<string, object> dict) {
                GrantGainedResourcesFromObject(rpglObject, new(dict));
            }
        }
    }

    private static void GrantGainedResourceFromString(RPGLObject rpglObject, string resourceDatapackId) {
        rpglObject.GiveResource(RPGLFactory.NewResource(resourceDatapackId));
    }

    private static void GrantGainedResourcesFromObject(RPGLObject rpglObject, JsonObject gainedResources) {
        long count = gainedResources.GetLong("count") ?? 1L;
        for (int i = 0; i < count; i++) {
            GrantGainedResourceFromString(rpglObject, gainedResources.GetString("resource"));
        }
    }

    public static void RevokeLostEffects(RPGLObject rpglObject, JsonObject lostFeatures) {
        JsonArray lostEffects = lostFeatures.GetJsonArray("effects") ?? new();
        for (int i = 0; i < lostEffects.Count(); i++) {
            string lostEffectDatapackId = lostEffects.GetString(i);
            List<RPGLEffect> effects = RPGL.GetRPGLEffects().FindAll(
                x => x.GetTarget() == rpglObject.GetUuid()
            );
            for (int j = 0; j < effects.Count; j++) {
                RPGLEffect effect = effects[j];
                if (lostEffectDatapackId == effect.GetDatapackId()) {
                    RPGL.RemoveRPGLEffect(effect);
                    break;
                }
            }
        }
    }

    public static void RevokeLostEvents(RPGLObject rpglObject, JsonObject lostFeatures) {
        JsonArray lostEvents = lostFeatures.GetJsonArray("events") ?? new();
        rpglObject.GetEvents().AsList().RemoveAll(lostEvents.Contains);
    }

    public static void RevokeLostResources(RPGLObject rpglObject, JsonObject lostFeatures) {
        JsonArray lostResources = lostFeatures.GetJsonArray("resources") ?? new();
        for (int i = 0; i < lostResources.Count(); i++) {
            var data = lostResources.AsList()[i];
            if (data is string resourceDatapackId) {
                RevokeLostResourceFromString(rpglObject, resourceDatapackId);
            } else if (data is Dictionary<string, object> dict) {
                RevokeLostResourcesFromObject(rpglObject, new(dict));
            }
        }
    }

    private static void RevokeLostResourceFromString(RPGLObject rpglObject, string resourceDatapackId) {
        List<RPGLResource> resources = RPGL.GetRPGLResources().FindAll(
            x => x.GetDatapackId() == resourceDatapackId && rpglObject.GetResources().Contains(x.GetUuid())
        );
        foreach (RPGLResource rpglResource in resources) {
            rpglObject.GetResources().AsList().Remove(rpglResource.GetUuid());
            RPGL.RemoveRPGLResource(rpglResource);
            break;
        }
    }

    private static void RevokeLostResourcesFromObject(RPGLObject rpglObject, JsonObject lostResources) {
        for (int i = 0; i < lostResources.GetLong("count"); i++) {
            RevokeLostResourceFromString(rpglObject, lostResources.GetString("resource"));
        }
    }

};
