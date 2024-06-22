using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

public class RepeatDamageDice : Function {

    public RepeatDamageDice() : base("repeat_damage_dice") {

    }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is DamageCollection damageCollection) {
            RepeatDice(damageCollection.GetDamageCollection(), functionJson);
        } else if (subevent is CriticalHitDamageCollection criticalHitDamageCollection) {
            RepeatDice(criticalHitDamageCollection.GetDamageCollection(), functionJson);
        }
    }

    private void RepeatDice(JsonArray damageArray, JsonObject functionJson) {
        if (damageArray.Count() > 0) {
            JsonObject damageJson = damageArray.GetJsonObject(0);
            JsonArray dice = damageJson.GetJsonArray("dice");
            if (dice.Count() > 0) {
                JsonObject die = dice.GetJsonObject(0);
                for (int i = 0; i < (functionJson.GetLong("count") ?? 1L); i++) {
                    dice.AddJsonObject(die.DeepClone());
                }
            }
        }
    }

};
