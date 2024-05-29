using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.database;

public class RPGLItemTemplate : RPGLTemplate {

    public RPGLItemTemplate() : base() {

    }

    public RPGLItemTemplate(JsonObject other) : this() {
        Join(other);
    }

    public override RPGLItem NewInstance() {
        RPGLItem rpglItem = new();
        Setup(rpglItem);
        // TODO process effects into data stored in the database
        // TODO process resources into data stored in the database

        return rpglItem;
    }

    public override RPGLItemTemplate ApplyBonuses(JsonArray bonuses) {
        return new(base.ApplyBonuses(bonuses));
    }

};
