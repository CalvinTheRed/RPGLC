using com.rpglc.json;

namespace com.rpglc.core;

public class RPGLRace : DatabaseContent {

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

};
