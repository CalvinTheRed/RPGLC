using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

public class RerollDamageDice : Function {

    public RerollDamageDice() : base("reroll_damage_dice") {

    }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is DamageRoll damageRoll) {
            damageRoll.RerollDamageDice(
                functionJson.GetString("damage_type") ?? "",
                functionJson.GetLong("lower_bound") ?? 0L,
                functionJson.GetLong("upper_bound") ?? long.MaxValue
            );
        }
    }

};
