using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class MaximizeHealingTest {

    [Fact(DisplayName = "maximizes healing roll")]
    public void MaximizesHealingRoll() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new HealingRoll().JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "healing": [
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

        MaximizeHealing function = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "maximize_healing"
            }
            """);

        FunctionState.StateData result;

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
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
            """, (subevent as HealingRoll).GetHealing().PrettyPrint());
    }

    [Fact(DisplayName = "maximizes healing delivery")]
    public void MaximizesHealingDelivery() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new HealingDelivery().JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "healing": [
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

        MaximizeHealing function = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "maximize_healing"
            }
            """);

        FunctionState.StateData result;

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
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
            """, (subevent as HealingDelivery).json.GetJsonArray("healing").PrettyPrint());
    }

};
