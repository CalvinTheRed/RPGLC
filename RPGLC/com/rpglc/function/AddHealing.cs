using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

public class AddHealing : Function {

    public AddHealing() : base("add_healing") {

    }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is HealingCollection healingCollection) {
            JsonArray healingArray = functionJson.GetJsonArray("healing");
            for (int i = 0; i < healingArray.Count(); i++) {
                healingCollection.AddHealing(CalculationSubevent.ProcessBonusJson(rpglEffect, subevent, healingArray.GetJsonObject(i), context));
            }
        }
    }

};
