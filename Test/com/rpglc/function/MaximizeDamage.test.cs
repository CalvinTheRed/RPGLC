using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class MaximizeDamageTest {

    [Fact(DisplayName = "maximizes damage roll (default)")]
    public void MaximizesDamageRollDefault() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DamageRoll().JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "damage": [
                    {
                        "damage_type": "fire",
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
                        "damage_type": "cold",
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

        MaximizeDamage function = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "maximize_damage"
            }
            """);

        FunctionState.StateData result;

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal("""
            [
              {
                "bonus": 0,
                "damage_type": "fire",
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
                "damage_type": "cold",
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
            """, (subevent as DamageRoll).GetDamage().PrettyPrint());
    }

    [Fact(DisplayName = "maximizes damage roll (customized)")]
    public void MaximizesDamageRollCustomized() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DamageRoll().JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "damage": [
                    {
                        "damage_type": "fire",
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
                        "damage_type": "cold",
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

        MaximizeDamage function = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "maximize_damage",
                "damage_type": "fire"
            }
            """);

        FunctionState.StateData result;

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal("""
            [
              {
                "bonus": 0,
                "damage_type": "fire",
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
                "damage_type": "cold",
                "dice": [
                  {
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

    [Fact(DisplayName = "maximizes damage delivery (default)")]
    public void MaximizesDamageDeliveryDefault() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DamageDelivery().JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "damage": [
                    {
                        "damage_type": "fire",
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
                        "damage_type": "cold",
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

        MaximizeDamage function = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "maximize_damage"
            }
            """);

        FunctionState.StateData result;

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal("""
            [
              {
                "bonus": 0,
                "damage_type": "fire",
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
                "damage_type": "cold",
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
            """, (subevent as DamageDelivery).json.GetJsonArray("damage").PrettyPrint());
    }

    [Fact(DisplayName = "maximizes damage delivery (customized)")]
    public void MaximizesDamageDeliveryCustomized() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DamageDelivery().JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "damage": [
                    {
                        "damage_type": "fire",
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
                        "damage_type": "cold",
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

        MaximizeDamage function = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "maximize_damage",
                "damage_type": "fire"
            }
            """);

        FunctionState.StateData result;

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal("""
            [
              {
                "bonus": 0,
                "damage_type": "fire",
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
                "damage_type": "cold",
                "dice": [
                  {
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
            """, (subevent as DamageDelivery).json.GetJsonArray("damage").PrettyPrint());
    }

};
