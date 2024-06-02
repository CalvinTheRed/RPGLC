using com.rpglc.core;
using com.rpglc.database;
using com.rpglc.json;

namespace RPGLC.com.rpglc.core;

public static class FeatureManager {

    public static void GrantGainedEffects(RPGLObject rpglObject, JsonObject gainedFeatures, JsonObject choices) {
        JsonArray gainedEffects = gainedFeatures.GetJsonArray("effects") ?? new();
        for (int i = 0; i < gainedEffects.Count(); i++) {
            var effectElement = gainedEffects.AsList()[i];
            if (effectElement is string effectDatapackId) {
                GrantGainedEffectFromString(rpglObject, effectDatapackId);
            } else if (effectElement is Dictionary<string, object> dict) {
                GrantGainedEffectsFromObject(rpglObject, choices, new(dict));
            }
        }
    }

    private static void GrantGainedEffectFromString(RPGLObject rpglObject, string effectDatapackId) {
        _ = RPGLFactory.NewEffect(effectDatapackId,rpglObject.GetUuid(),rpglObject.GetUuid());
    }

    private static void GrantGainedEffectsFromObject(RPGLObject rpglObject, JsonObject choices, JsonObject gainedEffects) {
        string decisionName = gainedEffects.GetString("name");
        long numDecisions = gainedEffects.GetInt("count") ?? 1L;
        JsonArray options = gainedEffects.GetJsonArray("options");
        for (int i = 0; i < numDecisions; i++) {
            GrantGainedEffectFromString(
                rpglObject,
                options.GetString((int) choices.GetJsonArray(decisionName).GetInt(i))
            );
        }
    }

    public static void GrantGainedEvents(RPGLObject rpglObject, JsonObject gainedFeatures) {
        JsonArray gainedEvents = gainedFeatures.GetJsonArray("events") ?? new();
        rpglObject.GetEvents().AsList().AddRange(gainedEvents.AsList());
        DBManager.UpdateRPGLObject(rpglObject);
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
        DBManager.UpdateRPGLObject(rpglObject);
    }

    private static void GrantGainedResourceFromString(RPGLObject rpglObject, string resourceDatapackId) {
        rpglObject.AddResource(RPGLFactory.NewResource(resourceDatapackId));
    }

    private static void GrantGainedResourcesFromObject(RPGLObject rpglObject, JsonObject gainedResources) {
        long count = gainedResources.GetInt("count") ?? 1L;
        for (int i = 0; i < count; i++) {
            GrantGainedResourceFromString(rpglObject, gainedResources.GetString("resource"));
        }
    }

    public static void RevokeLostEffects(RPGLObject rpglObject, JsonObject lostFeatures) {
        JsonArray lostEffects = lostFeatures.GetJsonArray("effects") ?? new();
        for (int i = 0; i < lostEffects.Count(); i++) {
            string lostEffectDatapackId = lostEffects.GetString(i);
            List<RPGLEffect> effects = DBManager.QueryRPGLEffects(
                x => x.Target == rpglObject.GetUuid()
            );
            for (int j = 0; j < effects.Count; j++) {
                RPGLEffect effect = effects[j];
                if (lostEffectDatapackId == effect.GetDatapackId()) {
                    DBManager.DeleteRPGLEffect(effect);
                    break;
                }
            }
        }
    }

    public static void RevokeLostEvents(RPGLObject rpglObject, JsonObject lostFeatures) {
        JsonArray lostEvents = lostFeatures.GetJsonArray("events") ?? new();
        rpglObject.GetEvents().AsList().RemoveAll(lostEvents.Contains);
        DBManager.UpdateRPGLObject(rpglObject);
    }

    public static void RevokeLostResources(RPGLObject rpglObject, JsonObject lostFeatures) {
        JsonArray lostResources = lostFeatures.GetJsonArray("resources") ?? new();
        for (int i = 0; i < lostResources.Count(); i++) {
            object data = lostResources.AsList()[i];
            if (data is string resourceDatapackId) {
                RevokeLostResourceFromString(rpglObject, resourceDatapackId);
            } else if (data is Dictionary<string, object> dict) {
                RevokeLostResourcesFromObject(rpglObject, new(dict));
            }
        }
    }

    private static void RevokeLostResourceFromString(RPGLObject rpglObject, string resourceDatapackId) {
        List<RPGLResource> resources = DBManager.QueryRPGLResources(
            x => x.DatapackId == resourceDatapackId
        );
        foreach (RPGLResource rpglResource in resources) {
            if (rpglObject.GetResources().Contains(rpglResource.GetUuid())) {
                rpglObject.GetResources().AsList().Remove(rpglResource.GetUuid());
                DBManager.UpdateRPGLObject(rpglObject);
                DBManager.DeleteRPGLResource(rpglResource);
                break;
            }
        }
    }

    private static void RevokeLostResourcesFromObject(RPGLObject rpglObject, JsonObject lostResources) {
        for (int i = 0; i < lostResources.GetInt("count"); i++) {
            RevokeLostResourceFromString(rpglObject, lostResources.GetString("resource"));
        }
    }

};
