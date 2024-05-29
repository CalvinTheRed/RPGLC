using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.database;

public class RPGLObjectTemplate : RPGLTemplate {

    public RPGLObjectTemplate() : base() {

    }

    public RPGLObjectTemplate(JsonObject other) : this() {
        Join(other);
    }

    public override RPGLObject NewInstance() {
        RPGLObject rpglObject = new();
        Setup(rpglObject);
        // TODO process effects into data stored in the database
        // TODO process inventory into data stored in the database
        // TODO process equipped items into data stored in the database
        // TODO process resources into data stored in the database
        // TODO process classes into data stored in the database

        return rpglObject;
    }

    public override RPGLObjectTemplate ApplyBonuses(JsonArray bonuses) {
        return new(base.ApplyBonuses(bonuses));
    }

};
