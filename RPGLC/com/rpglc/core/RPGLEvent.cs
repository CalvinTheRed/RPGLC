﻿using com.rpglc.json;

namespace com.rpglc.core;

public class RPGLEvent : DatabaseContent {

    public JsonObject GetAreaOfEffect() {
        return GetJsonObject("area_of_effect");
    }

    public RPGLEvent SetAreaOfEffect(JsonObject areaOfEffect) {
        PutJsonObject("area_of_effect", areaOfEffect);
        return this;
    }

    public JsonArray GetSubevents() {
        return GetJsonArray("subevents");
    }

    public RPGLEvent SetSubevents(JsonArray subevents) {
        PutJsonArray("subevents", subevents);
        return this;
    }

    public JsonArray GetCost() {
        return GetJsonArray("cost");
    }

    public RPGLEvent SetCost(JsonArray cost) {
        PutJsonArray("cost", cost);
        return this;
    }

    public long? GetOriginItem() {
        return GetInt("origin_item");
    }

    public RPGLEvent SetOriginItem(long? originItem) {
        PutInt("origin_item", originItem);
        return this;
    }

};
