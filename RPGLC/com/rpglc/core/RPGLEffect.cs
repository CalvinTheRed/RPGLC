using com.rpglc.json;

namespace com.rpglc.core;

public class RPGLEffect : DatabaseContent {

    public JsonObject GetSubeventFilters() {
        return base.GetJsonObject("subevent_filters");
    }

    public RPGLEffect SetSubeventFilters(JsonObject subeventFilters) {
        base.PutJsonObject("subevent_filters", subeventFilters);
        return this;
    }

    public string GetSource() {
        return base.GetString("source");
    }

    public RPGLEffect SetSource(string source) {
        base.PutString("source", source);
        return this;
    }

    public string GetTarget() {
        return base.GetString("target");
    }

    public RPGLEffect SetTarget(string target) {
        base.PutString("target", target);
        return this;
    }

    public string GetOriginItem() {
        return base.GetString("origin_item");
    }

    public RPGLEffect SetOriginItem(string originItem) {
        base.PutString("origin_item", originItem);
        return this;
    }

};