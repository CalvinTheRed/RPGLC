using com.rpglc.core;
using com.rpglc.database;
using com.rpglc.json;

namespace com.rpglc.database;

public class RPGLItemTemplate : RPGLTemplate {

    public RPGLItemTemplate() {

    }

    public RPGLItemTemplate(JsonObject other) : this() {
        base.Join(other);
    }

    public override RPGLItem NewInstance() {
        RPGLItem rpglItem = new();
        this.Setup(rpglItem);
        // TODO process effects into data stored in the database
        // TODO process resources into data stored in the database

        // save to the database
        return rpglItem;
    }

};
