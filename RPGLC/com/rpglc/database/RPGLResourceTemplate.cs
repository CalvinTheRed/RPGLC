using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.database;

public class RPGLResourceTemplate : RPGLTemplate {

    public RPGLResourceTemplate() : base() {

    }

    public RPGLResourceTemplate(JsonObject other) : this() {
        Join(other);
    }

    public RPGLResource NewInstance(long uuid) {
        RPGLResource rpglResource = (RPGLResource) new RPGLResource().SetUuid(uuid);
        Setup(rpglResource);

        return rpglResource;
    }

    public override RPGLResourceTemplate ApplyBonuses(JsonArray bonuses) {
        return new(base.ApplyBonuses(bonuses));
    }

};
