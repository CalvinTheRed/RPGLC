using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class RepeatDamageDiceTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "repeats default dice (damage collection)")]
    public void RepeatsDefaultDice_DamageCollection() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        DamageCollection damageCollection = new DamageCollection()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "formula": "dice",
                            "damage_type": "fire",
                            "dice": [
                                { "size": 6, "determined": [ 3 ] }
                            ]
                        },
                        {
                            "formula": "dice",
                            "damage_type": "cold",
                            "dice": [
                                { "size": 6, "determined": [ 3 ] }
                            ]
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        new RepeatDamageDice().Execute(
            new RPGLEffect(),
            damageCollection,
            new JsonObject().LoadFromString("""
                {
                    "function": "repeat_damage_dice"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.Equal("""
            [
              {
                "bonus": 0,
                "damage_type": "fire",
                "dice": [
                  {
                    "determined": [
                      3
                    ],
                    "size": 6
                  },
                  {
                    "determined": [
                      3
                    ],
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
                      3
                    ],
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
            """, damageCollection.GetDamageCollection().PrettyPrint());
    }

    [Fact(DisplayName = "repeats default dice (critical hit damage collection)")]
    public void RepeatsDefaultDice_CriticalHitDamageCollection() {
        CriticalHitDamageCollection criticalHitDamageCollection = new CriticalHitDamageCollection()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
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
                            "damage_type": "cold",
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

        new RepeatDamageDice().Execute(
            new RPGLEffect(),
            criticalHitDamageCollection,
            new JsonObject().LoadFromString("""
                {
                    "function": "repeat_damage_dice"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.Equal("""
            [
              {
                "bonus": 0,
                "damage_type": "fire",
                "dice": [
                  {
                    "determined": [
                      3
                    ],
                    "size": 6
                  },
                  {
                    "determined": [
                      3
                    ],
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
                      3
                    ],
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
            """, criticalHitDamageCollection.GetDamageCollection().PrettyPrint());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "repeats indicated dice (damage collection)")]
    public void RepeatsIndicatedDice_DamageCollection() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        DamageCollection damageCollection = new DamageCollection()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "formula": "dice",
                            "damage_type": "fire",
                            "dice": [
                                { "size": 6, "determined": [ 3 ] }
                            ]
                        },
                        {
                            "formula": "dice",
                            "damage_type": "cold",
                            "dice": [
                                { "size": 6, "determined": [ 3 ] }
                            ]
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        new RepeatDamageDice().Execute(
            new RPGLEffect(),
            damageCollection,
            new JsonObject().LoadFromString("""
                {
                    "function": "repeat_damage_dice",
                    "count": 2
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.Equal("""
            [
              {
                "bonus": 0,
                "damage_type": "fire",
                "dice": [
                  {
                    "determined": [
                      3
                    ],
                    "size": 6
                  },
                  {
                    "determined": [
                      3
                    ],
                    "size": 6
                  },
                  {
                    "determined": [
                      3
                    ],
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
                      3
                    ],
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
            """, damageCollection.GetDamageCollection().PrettyPrint());
    }

    [Fact(DisplayName = "repeats indicated dice (critical hit damage collection)")]
    public void RepeatsIndicatedDice_CriticalHitDamageCollection() {
        CriticalHitDamageCollection criticalHitDamageCollection = new CriticalHitDamageCollection()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
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
                            "damage_type": "cold",
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

        new RepeatDamageDice().Execute(
            new RPGLEffect(),
            criticalHitDamageCollection,
            new JsonObject().LoadFromString("""
                {
                    "function": "repeat_damage_dice",
                    "count": 2
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.Equal("""
            [
              {
                "bonus": 0,
                "damage_type": "fire",
                "dice": [
                  {
                    "determined": [
                      3
                    ],
                    "size": 6
                  },
                  {
                    "determined": [
                      3
                    ],
                    "size": 6
                  },
                  {
                    "determined": [
                      3
                    ],
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
                      3
                    ],
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
            """, criticalHitDamageCollection.GetDamageCollection().PrettyPrint());
    }

};
