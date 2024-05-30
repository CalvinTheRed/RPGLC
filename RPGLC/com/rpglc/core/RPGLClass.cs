using com.rpglc.json;

namespace com.rpglc.core;

public class RPGLClass : DatabaseContent {

    public JsonObject? GetStartingFeatures() {
        return GetJsonObject("starting_features");
    }

    public RPGLClass SetStartingFeatures(JsonObject? startingFeatures) {
        PutJsonObject("starting_features", startingFeatures);
        return this;
    }

    public JsonObject GetFeatures() {
        return GetJsonObject("features");
    }

    public RPGLClass SetFeatures(JsonObject features) {
        PutJsonObject("features", features);
        return this;
    }

    public JsonObject GetNestedClasses() {
        return GetJsonObject("nested_classes");
    }

    public RPGLClass SetNestedClasses(JsonObject nestedClasses) {
        PutJsonObject("nested_classes", nestedClasses);
        return this;
    }

    public JsonArray? GetAbilityScoreIncreaseLevels() {
        return GetJsonArray("ability_score_increase_levels");
    }

    public RPGLClass SetAbilityScoreIncreaseLevels(JsonArray? abilityScoreIncreaseLevels) {
        PutJsonArray("ability_score_increase_levels", abilityScoreIncreaseLevels);
        return this;
    }

    public JsonArray? GetMulticlassRequirements() {
        return GetJsonArray("multiclass_requirements");
    }

    public RPGLClass SetMulticlassRequirements(JsonArray? multiclassRequirements) {
        PutJsonArray("multiclass_requirements", multiclassRequirements);
        return this;
    }

    public long? GetSubclassLevel() {
        return GetInt("subclass_level");
    }

    public RPGLClass SetSubclassLevel(long? subclassLevel) {
        PutInt("subevent_level", subclassLevel);
        return this;
    }

};
