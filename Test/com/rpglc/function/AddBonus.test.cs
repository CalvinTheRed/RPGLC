using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.function;

[Collection("Serial")]
public class AddBonusTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "adds bonus")]
    public void AddsBonus() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        Subevent subevent = new DummyCalculationSubevent()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        new AddBonus().Execute(
            new RPGLEffect(),
            subevent,
            new JsonObject().LoadFromString("""
                {
                    "function": "add_bonus",
                    "bonus": [
                        {
                            "formula": "range",
                            "bonus": 0,
                            "dice": [
                                { "count": 1, "size": 6, "determined": [ 3 ] }
                            ]
                        },
                        {
                            "formula": "range",
                            "bonus": 1,
                            "dice": [ ]
                        }
                    ]
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.Equal(3 + 1, (subevent as DummyCalculationSubevent).GetBonus());
    }

};
