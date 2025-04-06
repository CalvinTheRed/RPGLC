using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class HealingCollectionTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares default")]
    public void PreparesDefault() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        HealingCollection healingCollection = new HealingCollection()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal("""[]""", healingCollection.GetHealingCollection().ToString());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares healing")]
    public void PreparesHealing() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        HealingCollection healingCollection = new HealingCollection()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "healing": [
                        {
                            "formula": "number",
                            "number": 10
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal("""
            [
              {
                "bonus": 10,
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """,
            healingCollection.GetHealingCollection().PrettyPrint()
        );
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "adds healing")]
    public void AddsHealing() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        HealingCollection healingCollection = new HealingCollection()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .AddHealing(CalculationSubevent.SimplifyCalculationFormulaJson(
                new(),
                new DummySubevent(),
                new JsonObject().LoadFromString("""
                    {
                        "formula": "number",
                        "number": 10
                    }
                    """),
                new DummyContext()
            )
        );

        Assert.Equal("""
            [
              {
                "bonus": 10,
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """,
            healingCollection.GetHealingCollection().PrettyPrint()
        );
    }

};
