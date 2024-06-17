using com.rpglc.json;

namespace com.rpglc.database;

public abstract class RPGLTemplate : JsonObject {

    public void Setup(JsonObject other) {
        other.Join(this);
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
