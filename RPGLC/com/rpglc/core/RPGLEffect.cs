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

    public string? GetOriginItem() {
        return GetString("origin_item");
    }

    public RPGLEffect SetOriginItem(string? originItem) {
        PutString("origin_item", originItem);
        return this;
    }

    public string GetSource() {
        return GetString("source");
    }

    public RPGLEffect SetSource(string? source) {
        PutString("source", source);
        return this;
    }

    public string GetTarget() {
        return GetString("target");
    }

    public RPGLEffect SetTarget(string? target) {
        PutString("target", target);
        return this;
    }

};
