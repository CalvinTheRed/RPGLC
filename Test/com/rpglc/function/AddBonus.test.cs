using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.function;

[Collection("Serial")]
public class AddBonusTest {

    [DieTestingMode]
    [Fact(DisplayName = "adds bonus")]
    public void AddsBonus() {
        Subevent subevent = new DummyCalculationSubevent()
            .Prepare(new DummyContext(), new());

        new AddBonus().Execute(
            new RPGLEffect(),
            subevent,
            new JsonObject().LoadFromString("""
                {
                    "function": "add_bonus",
                    "bonus": [
                        {
                            "formula": "dice",
                            "dice": [
                                { "count": 1, "size": 6, "determined": [ 3 ] }
                            ]
                        },
                        {
                            "formula": "number",
                            "number": 1
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
