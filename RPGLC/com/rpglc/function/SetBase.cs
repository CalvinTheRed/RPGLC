using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

public class SetBase : Function {

    public SetBase() : base("set_base") {

    }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is CalculationSubevent calculationSubevent) {
            calculationSubevent.SetBase(CalculationSubevent.ProcessSetJson(rpglEffect, subevent, functionJson.GetJsonObject("base"), context));
        }
    }

};
