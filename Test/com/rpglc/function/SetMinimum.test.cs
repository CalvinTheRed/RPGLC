using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.function;

[Collection("Serial")]
public class SetMinimumTest {

    [Fact(DisplayName = "sets minimum")]
    public void SetsMinimum() {
        CalculationSubevent calculationSubevent = new DummyCalculationSubevent()
            .Prepare(new DummyContext(), new());

        new SetMinimum().Execute(
            new RPGLEffect(),
            calculationSubevent,
            new JsonObject().LoadFromString("""
                {
                    "function": "set_minimum",
                    "minimum": {
                        "formula": "number",
                        "number": 5,
                        "scale": {
                            "numerator": 2,
                            "denominator": 1,
                            "round_up": false
                        }
                    }
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.Equal(5 * 2, calculationSubevent.GetMinimum());
    }

};
