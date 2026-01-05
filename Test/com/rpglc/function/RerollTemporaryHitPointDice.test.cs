using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class RerollTemporaryHitPointDiceTest {

    [DieTestingMode]
    [Fact(DisplayName = "rerolls temporary hit point dice (default)")]
    public void RerollsTemporaryHitPointDiceDefault() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new TemporaryHitPointRoll().JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "temporary_hit_points": [
                    {
                        "bonus": 0,
                        "dice": [
                            { "roll": 6, "size": 6, "determined": [ 1 ] },
                            { "roll": 6, "size": 6, "determined": [ 1 ] }
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
                            { "roll": 6, "size": 6, "determined": [ 1 ] },
                            { "roll": 6, "size": 6, "determined": [ 1 ] }
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

        RerollTemporaryHitPointDice rerollTemporaryHitPointDice = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "reroll_temporary_hit_point_dice"
            }
            """);

        FunctionState.StateData result;

        result = rerollTemporaryHitPointDice.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal("""
            [
              {
                "bonus": 0,
                "dice": [
                  {
                    "determined": [ ],
                    "roll": 1,
                    "size": 6
                  },
                  {
                    "determined": [ ],
                    "roll": 1,
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
                    "roll": 1,
                    "size": 6
                  },
                  {
                    "determined": [ ],
                    "roll": 1,
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

    [DieTestingMode]
    [Fact(DisplayName = "rerolls temporary hit point dice (customized)")]
    public void RerollsTemporaryHitPointDiceCustomized() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new TemporaryHitPointRoll().JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "temporary_hit_points": [
                    {
                        "bonus": 0,
                        "dice": [
                            { "roll": 1, "size": 6, "determined": [ 3 ] },
                            { "roll": 6, "size": 6, "determined": [ 3 ] }
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
                            { "roll": 1, "size": 6, "determined": [ 3 ] },
                            { "roll": 6, "size": 6, "determined": [ 3 ] }
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

        RerollTemporaryHitPointDice rerollTemporaryHitPointDice = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "reroll_temporary_hit_point_dice",
                "threshold": {
                    "formula": "number",
                    "number": 2
                }
            }
            """);

        FunctionState.StateData result;

        result = rerollTemporaryHitPointDice.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal("""
            [
              {
                "bonus": 0,
                "dice": [
                  {
                    "determined": [ ],
                    "roll": 3,
                    "size": 6
                  },
                  {
                    "determined": [
                      3
                    ],
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
                    "roll": 3,
                    "size": 6
                  },
                  {
                    "determined": [
                      3
                    ],
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

};
