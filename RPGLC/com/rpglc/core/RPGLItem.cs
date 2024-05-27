using com.rpglc.json;

namespace com.rpglc.core;

public class RPGLItem : TaggableContent {

    public Int64 GetWeight() {
        return base.GetInt("weight");
    }

    public RPGLItem SetWeight(Int64 weight) {
        base.PutInt("weight", weight);
        return this;
    }

    public Int64 GetCost() {
        return base.GetInt("cost");
    }

    public RPGLItem SetCost(Int64 cost) {
        base.PutInt("cost", cost);
        return this;
    }

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

};
