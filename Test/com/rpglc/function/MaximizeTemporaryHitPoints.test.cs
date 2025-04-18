using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class MaximizeTemporaryHitPointsTest {

    [Fact(DisplayName = "maximizes temporary hit point roll")]
    public void MaximizesTemporaryHitPointRoll() {
        TemporaryHitPointRoll temporaryHitPointRoll = new TemporaryHitPointRoll()
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
            .Prepare(new DummyContext(), new());

        new MaximizeTemporaryHitPoints().Execute(
            new RPGLEffect(),
            temporaryHitPointRoll,
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
            """, temporaryHitPointRoll.GetTemporaryHitPoints().PrettyPrint());
    }

    [Fact(DisplayName = "maximizes temporary hit point delivery")]
    public void MaximizesTemporaryHitPointDelivery() {
        TemporaryHitPointDelivery temporaryHitPointDelivery = new TemporaryHitPointDelivery()
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
            .Prepare(new DummyContext(), new());

        new MaximizeTemporaryHitPoints().Execute(
            new RPGLEffect(),
            temporaryHitPointDelivery,
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
            """, temporaryHitPointDelivery.json.GetJsonArray("temporary_hit_points").PrettyPrint());
    }

};
