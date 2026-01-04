using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class RerollDamageDiceTest {

    [DieTestingMode]
    [Fact(DisplayName = "rerolls damage dice (default)")]
    public void RerollsDamageDiceDefault() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DamageRoll().JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "damage": [
                    {
                        "damage_type": "fire",
                        "bonus": 0,
                        "dice": [
                            { "roll": 1, "size": 6, "determined": [ 6 ] },
                            { "roll": 1, "size": 6, "determined": [ 6 ] }
                        ],
                        "scale": {
                            "numerator": 1,
                            "denominator": 1,
                            "round_up": false
                        }
                    },
                    {
                        "damage_type": "cold",
                        "bonus": 0,
                        "dice": [
                            { "roll": 1, "size": 6, "determined": [ 6 ] },
                            { "roll": 1, "size": 6, "determined": [ 6 ] }
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

        RerollDamageDice rerollDamageDice = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "reroll_damage_dice"
            }
            """);

        FunctionState.StateData result;

        result = rerollDamageDice.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal("""
            [
              {
                "bonus": 0,
                "damage_type": "fire",
                "dice": [
                  {
                    "determined": [ ],
                    "roll": 6,
                    "size": 6
                  },
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
                "damage_type": "cold",
                "dice": [
                  {
                    "determined": [ ],
                    "roll": 6,
                    "size": 6
                  },
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
            """, (subevent as DamageRoll).GetDamage().PrettyPrint());
    }

    [DieTestingMode]
    [Fact(DisplayName = "rerolls damage dice (threshold)")]
    public void RerollsDamageDiceThreshold() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DamageRoll().JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "damage": [
                    {
                        "damage_type": "fire",
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
                        "damage_type": "cold",
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

        RerollDamageDice rerollDamageDice = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "reroll_damage_dice",
                "threshold": {
                    "formula": "number",
                    "number": 2
                }
            }
            """);

        FunctionState.StateData result;

        result = rerollDamageDice.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal("""
            [
              {
                "bonus": 0,
                "damage_type": "fire",
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
                "damage_type": "cold",
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
            """, (subevent as DamageRoll).GetDamage().PrettyPrint());
    }

    [DieTestingMode]
    [Fact(DisplayName = "rerolls damage dice (typed)")]
    public void RerollsDamageDiceTyped() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DamageRoll().JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "damage": [
                    {
                        "damage_type": "fire",
                        "bonus": 0,
                        "dice": [
                            { "roll": 1, "size": 6, "determined": [ 6 ] },
                            { "roll": 1, "size": 6, "determined": [ 6 ] }
                        ],
                        "scale": {
                            "numerator": 1,
                            "denominator": 1,
                            "round_up": false
                        }
                    },
                    {
                        "damage_type": "cold",
                        "bonus": 0,
                        "dice": [
                            { "roll": 1, "size": 6, "determined": [ 6 ] },
                            { "roll": 1, "size": 6, "determined": [ 6 ] }
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

        RerollDamageDice rerollDamageDice = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "reroll_damage_dice",
                "damage_type": "fire"
            }
            """);

        FunctionState.StateData result;

        result = rerollDamageDice.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal("""
            [
              {
                "bonus": 0,
                "damage_type": "fire",
                "dice": [
                  {
                    "determined": [ ],
                    "roll": 6,
                    "size": 6
                  },
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
                "damage_type": "cold",
                "dice": [
                  {
                    "determined": [
                      6
                    ],
                    "roll": 1,
                    "size": 6
                  },
                  {
                    "determined": [
                      6
                    ],
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
            """, (subevent as DamageRoll).GetDamage().PrettyPrint());
    }

};
