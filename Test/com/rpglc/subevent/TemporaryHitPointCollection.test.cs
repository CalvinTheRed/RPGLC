using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class TemporaryHitPointCollectionTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares default")]
    public void PreparesDefault() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        TemporaryHitPointCollection temporaryHitPointCollection = new TemporaryHitPointCollection()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal("""[]""", temporaryHitPointCollection.GetTemporaryHitPointCollection().ToString());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares temporary hit points")]
    public void PreparesTemporaryHitPoints() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        TemporaryHitPointCollection temporaryHitPointCollection = new TemporaryHitPointCollection()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "temporary_hit_points": [
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
            temporaryHitPointCollection.GetTemporaryHitPointCollection().PrettyPrint()
        );
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "adds temporary hit points")]
    public void AddsTemporaryHitPoints() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        TemporaryHitPointCollection temporaryHitPointCollection = new TemporaryHitPointCollection()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .AddTemporaryHitPoints(CalculationSubevent.SimplifyCalculationFormula(
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
            temporaryHitPointCollection.GetTemporaryHitPointCollection().PrettyPrint()
        );
    }

};
