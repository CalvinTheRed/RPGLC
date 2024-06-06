using com.rpglc.json;

namespace com.rpglc.core;

public class RPGLItem : TaggableContent {

    public JsonObject GetEffects() {
        return GetJsonObject("effects");
    }

    public RPGLItem SetEffects(JsonObject effects) {
        PutJsonObject("effects", effects);
        return this;
    }

    public JsonArray GetEvents() {
        return GetJsonArray("events");
    }

    public RPGLItem SetEvents(JsonArray events) {
        PutJsonArray("events", events);
        return this;
    }

    public JsonArray GetResources() {
        return GetJsonArray("resources");
    }

    public RPGLItem SetResources(JsonArray resources) {
        PutJsonArray("resources", resources);
        return this;
    }

    public long GetCost() {
        return (long) GetInt("cost");
    }

    public RPGLItem SetCost(long cost) {
        PutInt("cost", cost);
        return this;
    }

    public long GetWeight() {
        return (long) GetInt("weight");
    }

    public RPGLItem SetWeight(long weight) {
        PutInt("weight", weight);
        return this;
    }

};
