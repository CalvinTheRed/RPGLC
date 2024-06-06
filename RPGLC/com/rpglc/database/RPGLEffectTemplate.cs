using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.database;

public class RPGLEffectTemplate : RPGLTemplate {

    public RPGLEffectTemplate() : base() {

    }

    public RPGLEffectTemplate(JsonObject other) : this() {
        Join(other);
    }

    public override RPGLEffectTemplate ApplyBonuses(JsonArray bonuses) {
        return new(base.ApplyBonuses(bonuses));
    }

    public RPGLEffect NewInstance(string uuid) {
        RPGLEffect rpglEffect = (RPGLEffect) new RPGLEffect().SetUuid(uuid);
        Setup(rpglEffect);

        return rpglEffect;
    }

};
