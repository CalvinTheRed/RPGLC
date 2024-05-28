using com.rpglc.json;

namespace com.rpglc.core;

public class RPGLItem : TaggableContent {

    public JsonArray GetEffects() {
        return base.GetJsonArray("effects");
    }

    public RPGLItem SetEffects(JsonArray effects) {
        base.PutJsonArray("effects", effects);
        return this;
    }

    public JsonArray GetEvents() {
        return base.GetJsonArray("events");
    }

    public RPGLItem SetEvents(JsonArray events) {
        base.PutJsonArray("events", events);
        return this;
    }

    public JsonArray GetResources() {
        return base.GetJsonArray("resources");
    }

    public RPGLItem SetResources(JsonArray resources) {
        base.PutJsonArray("resources", resources);
        return this;
    }

    public long GetCost() {
        return base.GetInt("cost");
    }

    public RPGLItem SetCost(long cost) {
        base.PutInt("cost", cost);
        return this;
    }

    public long GetWeight() {
        return base.GetInt("weight");
    }

    public RPGLItem SetWeight(long weight) {
        base.PutInt("weight", weight);
        return this;
    }

};
