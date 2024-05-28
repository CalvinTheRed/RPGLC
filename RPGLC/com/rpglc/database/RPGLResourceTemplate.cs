using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.database;

public class RPGLResourceTemplate : RPGLTemplate {

    public RPGLResourceTemplate() {

    }

    public RPGLResourceTemplate(JsonObject other) : this() {
        base.Join(other);
    }

    public override RPGLResource NewInstance() {
        RPGLResource rpglResource = new();
        this.Setup(rpglResource);

        // TODO save to the database
        return rpglResource;
    }

};
