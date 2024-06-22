using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

public class AddBonus : Function {

    public AddBonus() : base("add_bonus") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is CalculationSubevent calculationSubevent) {
            JsonArray bonusArray = functionJson.GetJsonArray("bonus");
            for (int i = 0; i < bonusArray.Count(); i++) {
                calculationSubevent.AddBonus(CalculationSubevent.ProcessBonusJson(rpglEffect, subevent, bonusArray.GetJsonObject(i), context));
            }
        }
    }

};
