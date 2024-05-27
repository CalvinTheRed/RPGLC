using com.rpglc.json;

namespace com.rpglc.core;

public class RPGLEvent : DatabaseContent {

    public JsonObject GetAreaOfEffect() {
        return base.GetJsonObject("area_of_effect");
    }

    public RPGLEvent SetAreaOfEffect(JsonObject areaOfEffect) {
        base.PutJsonObject("area_of_effect", areaOfEffect);
        return this;
    }

    public JsonArray GetSubevents() {
        return base.GetJsonArray("subevents");
    }

    public RPGLEvent SetSubevents(JsonArray subevents) {
        base.PutJsonArray("subevents", subevents);
        return this;
    }

    public JsonArray GetCost() {
        return base.GetJsonArray("cost");
    }

    public RPGLEvent SetCost(JsonArray cost) {
        base.PutJsonArray("cost", cost);
        return this;
    }

    public string GetOriginItem() {
        return base.GetString("origin_item");
    }

    public RPGLEvent SetOriginItem(string originItem) {
        base.PutString("origin_item", originItem);
        return this;
    }

};
