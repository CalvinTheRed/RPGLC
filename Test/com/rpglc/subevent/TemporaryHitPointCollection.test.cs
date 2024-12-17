using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[AssignDatabase]
[Collection("Serial")]
public class TemporaryHitPointCollectionTest {

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares default")]
    public void PreparesDefault() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        TemporaryHitPointCollection temporaryHitPointCollection = new TemporaryHitPointCollection()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal("""[]""", temporaryHitPointCollection.GetTemporaryHitPointCollection().ToString());
    }

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares temporary hit points")]
    public void PreparesTemporaryHitPoints() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        TemporaryHitPointCollection temporaryHitPointCollection = new TemporaryHitPointCollection()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "temporary_hit_points": [
                        {
                            "formula": "range",
                            "bonus": 1,
                            "dice": [
                                { "count": 1, "size": 6, "determined": [ 3 ] }
                            ],
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal("""
            [
              {
                "bonus": 1,
                "dice": [
                  {
                    "determined": [
                      3
                    ],
                    "size": 6
                  }
                ],
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

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "adds temporary hit points")]
    public void AddsTemporaryHitPoints() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        TemporaryHitPointCollection temporaryHitPointCollection = new TemporaryHitPointCollection()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .AddTemporaryHitPoints(CalculationSubevent.ProcessBonusJson(
                new(),
                new DummySubevent(),
                new JsonObject().LoadFromString("""
                    {
                        "formula": "range",
                        "bonus": 1,
                        "dice": [
                            { "count": 1, "size": 6, "determined": [ 3 ] }
                        ],
                    }
                    """),
                new DummyContext()
            )
        );

        Assert.Equal("""
            [
              {
                "bonus": 1,
                "dice": [
                  {
                    "determined": [
                      3
                    ],
                    "size": 6
                  }
                ],
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
