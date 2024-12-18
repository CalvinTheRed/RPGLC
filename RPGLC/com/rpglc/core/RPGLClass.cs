using com.rpglc.json;

namespace com.rpglc.core;

public class RPGLClass : DatabaseContent {

    public RPGLClass() : base([]) { }

    public RPGLClass(Dictionary<string, object> data) : base(data) { }

    public JsonObject? GetNestedClasses() {
        return GetJsonObject("nested_classes") ?? new();
    }

    public RPGLClass SetNestedClasses(JsonObject? nestedClasses) {
        PutJsonObject("nested_classes", nestedClasses);
        return this;
    }

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
        return GetLong("subclass_level");
    }

    public RPGLClass SetSubclassLevel(long? subclassLevel) {
        PutLong("subevent_level", subclassLevel);
        return this;
    }

    // =====================================================================
    // RPGLObject class/level management helper methods.
    // =====================================================================

    public void LevelUpRPGLObject(RPGLObject rpglObject, JsonObject choices) {
        long level = IncrementRPGLObjectLevel(rpglObject);
        JsonObject features = GetFeatures().GetJsonObject($"{level}");
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

    private long IncrementRPGLObjectLevel(RPGLObject rpglObject) {
        // TODO check for meeting multiclassing requirements
        long level = rpglObject.GetLevel(GetDatapackId()) + 1;
        if (level == 1) {
            _ = rpglObject.GetClasses().AddJsonObject(new JsonObject()
                .PutString("name", GetName())
                .PutString("id", GetDatapackId())
                .PutLong("level", level)
                .PutJsonObject("additional_nested_classes", new())
            );
        } else {
            JsonArray classes = rpglObject.GetClasses();
            for (int i = 0; i < classes.Count(); i++) {
                JsonObject classData = classes.GetJsonObject(i);
                if (GetDatapackId() == classData.GetString("id")) {
                    classData.PutLong("level", level);
                    break;
                }
            }
        }
        return level;
    }

    // =====================================================================
    // Feature management methods.
    // =====================================================================

    public void GrantStartingFeatures(RPGLObject rpglObject, JsonObject choices) {
        JsonObject startingFeatures = GetStartingFeatures() ?? new();
        FeatureManager.GrantGainedEffects(rpglObject, startingFeatures, choices);
        FeatureManager.GrantGainedEvents(rpglObject, startingFeatures);
        FeatureManager.GrantGainedResources(rpglObject, startingFeatures);
    }

};
