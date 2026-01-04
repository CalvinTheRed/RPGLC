using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class RepeatDamageDiceTest {

    [Fact(DisplayName = "repeats damage collection die (default)")]
    public void RepeatsDamageCollectionDieDefault() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DamageCollection().JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "damage": [
                    {
                        "damage_type": "fire",
                        "bonus": 0,
                        "dice": [
                            { "size": 6 }
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
                            { "size": 6 }
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

        RepeatDamageDice repeatDamageDice = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "repeat_damage_dice"
            }
            """);

        FunctionState.StateData result;

        result = repeatDamageDice.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal("""
            [
              {
                "bonus": 0,
                "damage_type": "fire",
                "dice": [
                  {
                    "size": 6
                  },
                  {
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
            """, (subevent as DamageCollection).GetDamageCollection().PrettyPrint());
    }

    [Fact(DisplayName = "repeats damage collection die (customized)")]
    public void RepeatsDamageCollectionDieCustomized() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DamageCollection().JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "damage": [
                    {
                        "damage_type": "fire",
                        "bonus": 0,
                        "dice": [
                            { "size": 6 }
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
                            { "size": 6 }
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

        RepeatDamageDice repeatDamageDice = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "repeat_damage_dice",
                "count": 2
            }
            """);

        FunctionState.StateData result;

        result = repeatDamageDice.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal("""
            [
              {
                "bonus": 0,
                "damage_type": "fire",
                "dice": [
                  {
                    "size": 6
                  },
                  {
                    "size": 6
                  },
                  {
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
            """, (subevent as DamageCollection).GetDamageCollection().PrettyPrint());
    }

};
