using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

public class SetTemporaryHitPointDice : Function {

    public SetTemporaryHitPointDice() : base("set_temporary_hit_point_dice") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is TemporaryHitPointRoll temporaryHitPointRoll) {
            temporaryHitPointRoll.SetTemporaryHitPointDice(rpglEffect, functionJson, context);
        }
    }

};
