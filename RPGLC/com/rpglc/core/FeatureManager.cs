using com.rpglc.core;
using com.rpglc.database;
using com.rpglc.json;

namespace RPGLC.com.rpglc.core;

public static class FeatureManager {

    public static void GrantGainedEffects(RPGLObject rpglObject, JsonObject gainedFeatures, JsonObject choices) {
        JsonArray effects = gainedFeatures.GetJsonArray("effects") ?? new();
        for (int i = 0; i < effects.Count(); i++) {
            var effectElement = effects.AsList()[i];
            if (effectElement is string effectDatapackId) {
                _ = RPGLFactory.NewEffect(
                    effectDatapackId,
                    rpglObject.GetUuid(),
                    rpglObject.GetUuid()
                );
            } else if (effectElement is Dictionary<string, object>) {
                JsonObject effectsAlternatives = effects.GetJsonObject(i);
                string name = effectsAlternatives.GetString("name");
                long count = effectsAlternatives.GetInt("count") ?? 1L;
                JsonArray options = effectsAlternatives.GetJsonArray("options");
                JsonArray choiceIndices = new();
                foreach (string key in choices.AsDict().Keys) {
                    if (name == key) {
                        choiceIndices = choices.GetJsonArray(key);
                        break;
                    }
                }
                for (int j = 0; j < count; j++) {
                    _ = RPGLFactory.NewEffect(
                        options.GetString((int) choiceIndices.GetInt(j)),
                        rpglObject.GetUuid(),
                        rpglObject.GetUuid()
                    );
                }
            }
        }
    }

    public static void GrantGainedEvents(RPGLObject rpglObject, JsonObject gainedFeatures) {
        JsonArray events = gainedFeatures.GetJsonArray("events") ?? new();
        rpglObject.GetEvents().AsList().AddRange(events.AsList());
    }

    public static void GrantGainedResources(RPGLObject rpglObject, JsonObject gainedFeatures) {
        JsonArray resources = gainedFeatures.GetJsonArray("resources") ?? new();
        for (int i = 0; i < resources.Count(); i++) {
            JsonObject resourceData = resources.GetJsonObject(i);
            long count = resourceData.GetInt("count") ?? 1L;
            for (int j = 0; j < count; j++) {
                RPGLResource rpglResource = RPGLFactory.NewResource(resourceData.GetString("resource"));
                rpglObject.AddResource(rpglResource);
            }
        }
    }

    public static void RevokeLostEffects(RPGLObject rpglObject, JsonObject lostFeatures) {
        JsonArray lostEffects = lostFeatures.GetJsonArray("effects") ?? new();
        List<RPGLEffect> effects = DBManager.QueryRPGLEffects(x =>
            x.Target == rpglObject.GetUuid()
        );
        for (int i = 0; i < lostEffects.Count(); i++) {
            string lostEffectDatapackId = lostEffects.GetString(i);
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
    }

    public static void RevokeLostResources(RPGLObject rpglObject, JsonObject lostFeatures) {
        JsonArray lostResources = lostFeatures.GetJsonArray("resources") ?? new();
        JsonArray resourceUuidList = rpglObject.GetResources();
        for (int i = 0; i < lostResources.Count(); i++) {
            // TODO make this work with counts similar to gaining?
            string lostResourceDatapackId = lostResources.GetString(i);
            List<RPGLResource> resources = DBManager.QueryRPGLResources(x =>
                x.DatapackId == lostResourceDatapackId
                // TODO can this query be such that only resources with a uuid contained in resourceUuidList are returned?
            );
            RPGLResource? rpglResource = null;
            foreach (RPGLResource _ in resources) {
                if (resourceUuidList.Contains(_.GetUuid())) {
                    rpglResource = _;
                    break;
                }
            }
            if (rpglResource is not null) {
                DBManager.DeleteRPGLResource(rpglResource);
            } else {
                break;
            }
        }
    }

}
