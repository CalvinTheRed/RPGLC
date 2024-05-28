using com.rpglc.json;

namespace com.rpglc.database;

public abstract class RPGLTemplate : JsonObject {

    public abstract JsonObject NewInstance();

    public virtual void Setup(JsonObject other) {
        other.Join(this);
    }

    public JsonObject ApplyBonuses(JsonArray bonuses) {
        JsonObject withBonuses = this.DeepClone();
        for (int i = 0; i < bonuses.Count(); i++) {
            JsonObject fieldBonus = bonuses.GetJsonObject(i);
            String field = fieldBonus.GetString("field");
            withBonuses.InsertInt(field, withBonuses.SeekInt(field) + fieldBonus.GetInt("bonus"));
        }
        return withBonuses;
    }

};
