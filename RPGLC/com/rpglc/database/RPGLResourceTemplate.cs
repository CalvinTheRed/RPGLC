using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.database;

public class RPGLResourceTemplate : RPGLTemplate {

    public RPGLResourceTemplate() : base() {

    }

    public RPGLResourceTemplate(JsonObject other) : this() {
        Join(other);
    }

    public override RPGLResource NewInstance() {
        RPGLResource rpglResource = new();
        Setup(rpglResource);

        return rpglResource;
    }

    public override RPGLResourceTemplate ApplyBonuses(JsonArray bonuses) {
        return new(base.ApplyBonuses(bonuses));
    }

};
