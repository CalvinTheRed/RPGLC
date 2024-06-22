using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.testutils.subevent;

public class DummyCalculationSubevent : CalculationSubevent {
    
    public DummyCalculationSubevent() : base("dummy_calculation_subevent") { }

    public override Subevent Clone() {
        return this;
    }

    public override Subevent Clone(JsonObject jsonData) {
        return this;
    }

    public override Subevent Run(RPGLContext context, JsonArray originPoint) {
        return this;
    }

};