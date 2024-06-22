using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

public class MaximizeHealing : Function {

    public MaximizeHealing() : base("maximize_healing") {

    }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is HealingRoll healingRoll) {
            healingRoll.MaximizeHealingDice();
        } else if (subevent is HealingDelivery healingDelivery) {
            healingDelivery.MaximizeHealingDice();
        }
    }

};
