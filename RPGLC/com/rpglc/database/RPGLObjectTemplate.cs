using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.database;

public class RPGLObjectTemplate : RPGLTemplate {

    public RPGLObjectTemplate() {

    }

    public RPGLObjectTemplate(JsonObject other) : this() {
        base.Join(other);
    }

    public override RPGLObject NewInstance() {
        RPGLObject rpglObject = new();
        this.Setup(rpglObject);
        // TODO process effects into data stored in the database
        // TODO process inventory into data stored in the database
        // TODO process equipped items into data stored in the database
        // TODO process resources into data stored in the database
        // TODO process classes into data stored in the database

        // TODO save to the database
        return rpglObject;
    }

};
