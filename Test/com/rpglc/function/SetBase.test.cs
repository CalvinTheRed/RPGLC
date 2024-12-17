using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.function;

[AssignDatabase]
[Collection("Serial")]
public class SetBaseTest {

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "sets base")]
    public void SetsBase() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        Subevent subevent = new DummyCalculationSubevent()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        new SetBase().Execute(
            new RPGLEffect(),
            subevent,
            new JsonObject().LoadFromString("""
                {
                    "function": "set_base",
                    "base": {
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

        Assert.Equal(5 * 2, (subevent as CalculationSubevent).GetBase());
    }

};
