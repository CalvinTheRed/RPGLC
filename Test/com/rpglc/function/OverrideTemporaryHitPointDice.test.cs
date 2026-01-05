using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class OverrideTemporaryHitPointDiceTest {

    [Fact(DisplayName = "overrides temporary hit point dice (number)")]
    public void OverridesTemporaryHitPointDiceNumber() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new TemporaryHitPointRoll().JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "temporary_hit_points": [
                    {
                        "bonus": 0,
                        "dice": [
                            { "roll": 1, "size": 6 },
                            { "roll": 6, "size": 6 }
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
                            { "roll": 1, "size": 6 },
                            { "roll": 6, "size": 6 }
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

        OverrideTemporaryHitPointDice overrideTemporaryHitPointDice = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "override_temporary_hit_point_dice",
                "override": {
                    "formula": "number",
                    "number": 3
                }
            }
            """);

        FunctionState.StateData result;

        result = overrideTemporaryHitPointDice.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal("""
            [
              {
                "bonus": 0,
                "dice": [
                  {
                    "roll": 3,
                    "size": 6
                  },
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
                    "roll": 3,
                    "size": 6
                  },
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

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "overrides temporary hit point dice (modifier)")]
    public void OverridesTemporaryHitPointDiceModifier() {
        long strScore = 16L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.GetAbilityScores().PutLong("str", strScore);
        RPGLContext context = new DummyContext().Add(rpglObject);
        RPGLEffect rpglEffect = new();
        Subevent subevent = new TemporaryHitPointRoll().JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "temporary_hit_points": [
                    {
                        "bonus": 0,
                        "dice": [
                            { "roll": 1, "size": 6 },
                            { "roll": 6, "size": 6 }
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
                            { "roll": 1, "size": 6 },
                            { "roll": 6, "size": 6 }
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
        )).SetSource(rpglObject);

        OverrideTemporaryHitPointDice overrideTemporaryHitPointDice = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "override_temporary_hit_point_dice",
                "override": {
                    "formula": "modifier",
                    "object": {
                        "from": "subevent",
                        "object": "source"
                    },
                    "ability": "str"
                }
            }
            """);

        FunctionState.StateData result;

        result = overrideTemporaryHitPointDice.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.NotNull(result.dependency);
        Assert.False(result.stepCompleted);
        Assert.Equal("""
            [
              {
                "bonus": 0,
                "dice": [
                  {
                    "roll": 1,
                    "size": 6
                  },
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
                    "roll": 1,
                    "size": 6
                  },
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
        Assert.True(result.dependency is CalculateAbilityScore);

        (result.dependency as CalculateAbilityScore)
            .SetMinimum(long.MinValue)
            .SetBase(strScore)
            .JoinSubeventData(new JsonObject().LoadFromString("""{ "bonuses": [ ] }"""));

        result = overrideTemporaryHitPointDice.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal("""
            [
              {
                "bonus": 0,
                "dice": [
                  {
                    "roll": 3,
                    "size": 6
                  },
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
                    "roll": 3,
                    "size": 6
                  },
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

};
