using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.database;

public class RPGLEffectTemplate : RPGLTemplate {

    public RPGLEffectTemplate() {

    }

    public RPGLEffectTemplate(JsonObject other) : this() {
        base.Join(other);
    }

    public override RPGLEffect NewInstance() {
        RPGLEffect rpglEffect = new();
        this.Setup(rpglEffect);

        // TODO save to the database
        return rpglEffect;
    }

};
