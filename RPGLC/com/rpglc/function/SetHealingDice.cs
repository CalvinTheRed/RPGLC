using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

public class SetHealingDice : Function {

    public SetHealingDice() : base("set_healing_dice") {

    }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is HealingRoll healingRoll) {
            healingRoll.SetHealingDice(
                (long) functionJson.GetLong("set"),
                functionJson.GetLong("lower_bound") ?? 0L,
                functionJson.GetLong("upper_bound") ?? long.MaxValue
            );
        }
    }

};
