using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class MaximizeTemporaryHitPointsTest {

    [Fact(DisplayName = "maximizes temporary hit point roll")]
    public void MaximizesTemporaryHitPointRoll() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new TemporaryHitPointRoll().JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "temporary_hit_points": [
                    {
                        "bonus": 0,
                        "dice": [
                            { "roll": 1, "size": 6 }
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
                            { "roll": 1, "size": 6 }
                        ],
                        "scale": {
                            "numerator": 1,
                            "denominator": 1,
                            "round_up": false
                        }
                    }
                ]
            }
            """
        ));

        MaximizeTemporaryHitPoints maximizeTemporaryHitPoints = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "maximize_temporary_hit_points"
            }
            """);

        FunctionState.StateData result;

        result = maximizeTemporaryHitPoints.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal("""
            [
              {
                "bonus": 0,
                "dice": [
                  {
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

    [Fact(DisplayName = "maximizes temporary hit point delivery")]
    public void MaximizesTemporaryHitPointDelivery() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new TemporaryHitPointDelivery().JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "temporary_hit_points": [
                    {
                        "bonus": 0,
                        "dice": [
                            { "roll": 1, "size": 6 }
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
                            { "roll": 1, "size": 6 }
                        ],
                        "scale": {
                            "numerator": 1,
                            "denominator": 1,
                            "round_up": false
                        }
                    }
                ]
            }
            """
        ));

        MaximizeTemporaryHitPoints maximizeTemporaryHitPoints = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "maximize_temporary_hit_points"
            }
            """);

        FunctionState.StateData result;

        result = maximizeTemporaryHitPoints.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal("""
            [
              {
                "bonus": 0,
                "dice": [
                  {
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
            """, (subevent as TemporaryHitPointDelivery).json.GetJsonArray("temporary_hit_points").PrettyPrint());
    }

};
