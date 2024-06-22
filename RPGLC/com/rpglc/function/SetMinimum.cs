using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

public class SetMinimum : Function {

    public SetMinimum() : base("set_minimum") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is CalculationSubevent calculationSubevent) {
            calculationSubevent.SetMinimum(CalculationSubevent.ProcessSetJson(rpglEffect, subevent, functionJson.GetJsonObject("minimum"), context));
        }
    }

};
