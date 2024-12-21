using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

public class MaximizeTemporaryHitPoints : Function {

    public MaximizeTemporaryHitPoints() : base("maximize_temporary_hit_points") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is TemporaryHitPointRoll temporaryHitPointRoll) {
            temporaryHitPointRoll.MaximizeTemporaryHitPointDice();
        } else if (subevent is TemporaryHitPointDelivery temporaryHitPointDelivery) {
            temporaryHitPointDelivery.MaximizeTemporaryHitPointDice();
        }
    }

};
