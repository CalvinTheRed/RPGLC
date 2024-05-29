using com.rpglc.json;

namespace com.rpglc.database;

public abstract class RPGLTemplate : JsonObject {

    public abstract JsonObject NewInstance();

    public virtual void Setup(JsonObject other) {
        other.Join(this);
    }

    public virtual JsonObject ApplyBonuses(JsonArray bonuses) {
        JsonObject withBonuses = DeepClone();
        for (int i = 0; i < bonuses.Count(); i++) {
            JsonObject fieldBonus = bonuses.GetJsonObject(i);
            string field = fieldBonus.GetString("field");
            withBonuses.InsertInt(field, withBonuses.SeekInt(field) + fieldBonus.GetInt("bonus"));
        }
        return withBonuses;
    }

};
