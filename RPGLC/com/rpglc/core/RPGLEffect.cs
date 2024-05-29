using com.rpglc.json;

namespace com.rpglc.core;

public class RPGLEffect : PersistentContent {

    public JsonObject GetSubeventFilters() {
        return GetJsonObject("subevent_filters");
    }

    public RPGLEffect SetSubeventFilters(JsonObject subeventFilters) {
        PutJsonObject("subevent_filters", subeventFilters);
        return this;
    }

    public long? GetOriginItem() {
        return GetInt("origin_item");
    }

    public RPGLEffect SetOriginItem(long? originItem) {
        PutInt("origin_item", originItem);
        return this;
    }

    public long GetSource() {
        return (long) GetInt("source");
    }

    public RPGLEffect SetSource(long source) {
        PutInt("source", source);
        return this;
    }

    public long GetTarget() {
        return (long) GetInt("target");
    }

    public RPGLEffect SetTarget(long target) {
        PutInt("target", target);
        return this;
    }

};
