using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.database;

public class RPGLEffectTemplate(JsonObject other) : RPGLTemplate(other) {

    public override RPGLEffectTemplate ApplyBonuses(JsonArray bonuses) {
        return new(base.ApplyBonuses(bonuses));
    }

    public RPGLEffect NewInstance(string uuid) {
        RPGLEffect rpglEffect = (RPGLEffect) new RPGLEffect().SetUuid(uuid);
        Setup(rpglEffect);

        return rpglEffect;
    }

};
