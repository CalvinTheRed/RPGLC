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
public class OverrideHealingDiceTest {

    [Fact(DisplayName = "overrides healing dice (number)")]
    public void OverridesHealingDiceNumber() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new HealingRoll().JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "healing": [
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

        OverrideHealingDice overrideHealingDice = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "override_healing_dice",
                "override": {
                    "formula": "number",
                    "number": 3
                }
            }
            """);

        FunctionState.StateData result;

        result = overrideHealingDice.functionSteps[0](rpglEffect, subevent, functionJson, context);
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
            """, (subevent as HealingRoll).GetHealing().PrettyPrint());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "overrides healing dice (modifier)")]
    public void OverridesHealingDiceModifier() {
        long strScore = 16L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.GetAbilityScores().PutLong("str", strScore);
        RPGLContext context = new DummyContext().Add(rpglObject);
        RPGLEffect rpglEffect = new();
        Subevent subevent = new HealingRoll().JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "healing": [
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

        OverrideHealingDice overrideHealingDice = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "override_healing_dice",
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

        result = overrideHealingDice.functionSteps[0](rpglEffect, subevent, functionJson, context);
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
            """, (subevent as HealingRoll).GetHealing().PrettyPrint());
        Assert.True(result.dependency is CalculateAbilityScore);

        (result.dependency as CalculateAbilityScore)
            .SetMinimum(long.MinValue)
            .SetBase(strScore)
            .JoinSubeventData(new JsonObject().LoadFromString("""{ "bonuses": [ ] }"""));

        result = overrideHealingDice.functionSteps[0](rpglEffect, subevent, functionJson, context);
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
            """, (subevent as HealingRoll).GetHealing().PrettyPrint());
    }

};
