using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class MaximizeTemporaryHitPointsTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "maximizes temporary hit point roll")]
    public void MaximizesTemporaryHitPointRoll() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        Subevent subevent = new TemporaryHitPointRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "temporary_hit_points": [
                        {
                            "bonus": 0,
                            "dice": [
                                { "size": 6, "determined": [ 3 ] }
                            ],
                            "scale": {
                                "numerator": 1,
                                "denominator": 1,
                                "round_up": false
                            }
                        },
                        {
                            "bonus": 0,
                            "dice": [
                                { "size": 6, "determined": [ 3 ] }
                            ],
                            "scale": {
                                "numerator": 1,
                                "denominator": 1,
                                "round_up": false
                            }
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        new MaximizeTemporaryHitPoints().Execute(
            new RPGLEffect(),
            subevent,
            new JsonObject().LoadFromString("""
                {
                    "function": "maximize_temporary_hit_points"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.Equal("""
            [
              {
                "bonus": 0,
                "dice": [
                  {
                    "determined": [ ],
                    "roll": 6,
                    "size": 6
                  }
                ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              },
              {
                "bonus": 0,
                "dice": [
                  {
                    "determined": [ ],
                    "roll": 6,
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
            """, (subevent as TemporaryHitPointRoll).GetTemporaryHitPoints().PrettyPrint());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "maximizes temporary hit point delivery")]
    public void MaximizesTemporaryHitPointDelivery() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        Subevent subevent = new TemporaryHitPointDelivery()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "temporary_hit_points": [
                        {
                            "bonus": 0,
                            "dice": [
                                { "roll": 3, "size": 6, "determined": [ ] }
                            ],
                            "scale": {
                                "numerator": 1,
                                "denominator": 1,
                                "round_up": false
                            }
                        },
                        {
                            "bonus": 0,
                            "dice": [
                                { "roll": 3, "size": 6, "determined": [ ] }
                            ],
                            "scale": {
                                "numerator": 1,
                                "denominator": 1,
                                "round_up": false
                            }
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        new MaximizeTemporaryHitPoints().Execute(
            new RPGLEffect(),
            subevent,
            new JsonObject().LoadFromString("""
                {
                    "function": "maximize_temporary_hit_points"
                }
                """),
            new DummyContext(),
            new()
        );

        subevent
            .SetTarget(rpglObject)
            .Invoke(new DummyContext(), new());

        Assert.Equal(12, (subevent as TemporaryHitPointDelivery).GetTemporaryHitPoints());
    }

};
