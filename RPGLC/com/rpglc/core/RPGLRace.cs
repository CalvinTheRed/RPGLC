using com.rpglc.json;

namespace com.rpglc.core;

public class RPGLRace : DatabaseContent {

    public RPGLRace(Dictionary<string, object> data) : base(data) { }

    public JsonObject GetAbilityScoreBonuses() {
        return GetJsonObject("ability_score_bonuses");
    }

    public RPGLRace SetAbilityScoreBonuses(JsonObject abilityScoreBonuses) {
        PutJsonObject("ability_score_bonuses", abilityScoreBonuses);
        return this;
    }

    public JsonObject GetFeatures() {
        return GetJsonObject("features");
    }

    public RPGLRace SetFeatures(JsonObject features) {
        PutJsonObject("features", features);
        return this;
    }

    // =====================================================================
    // RPGLObject race/level management helper methods.
    // =====================================================================

    public void LevelUpRPGLObject(RPGLObject rpglObject, JsonObject choices, long level) {
        JsonObject? features = GetFeatures().GetJsonObject($"{level}");
        if (features is not null) {
            JsonObject gainedFeatures = features.GetJsonObject("gain") ?? new();
            FeatureManager.GrantGainedEffects(rpglObject, gainedFeatures, choices);
            FeatureManager.GrantGainedEvents(rpglObject, gainedFeatures);
            FeatureManager.GrantGainedResources(rpglObject, gainedFeatures);
            JsonObject lostFeatures = features.GetJsonObject("lose") ?? new();
            FeatureManager.RevokeLostEffects(rpglObject, lostFeatures);
            FeatureManager.RevokeLostEvents(rpglObject, lostFeatures);
            FeatureManager.RevokeLostResources(rpglObject, lostFeatures);
        }
    }

};
