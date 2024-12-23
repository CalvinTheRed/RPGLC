﻿using com.rpglc.json;

namespace com.rpglc.data;

public abstract class RPGLTemplate : JsonObject {

    public RPGLTemplate() { }

    public RPGLTemplate(JsonObject other) {
        Join(other);
    }

    public void Setup(JsonObject other) {
        other.Join(this);
    }

    public string GetDatapackId() {
        return GetString("datapack_id");
    }

    public virtual JsonObject ApplyBonuses(JsonArray bonuses) {
        JsonObject withBonuses = DeepClone();
        for (int i = 0; i < bonuses.Count(); i++) {
            JsonObject fieldBonus = bonuses.GetJsonObject(i);
            string field = fieldBonus.GetString("field");
            withBonuses.InsertLong(field, withBonuses.SeekLong(field) + fieldBonus.GetLong("bonus"));
        }
        return withBonuses;
    }

};
