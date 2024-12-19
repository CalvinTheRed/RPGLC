using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class DamageRollTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares default")]
    public void PreparesDefault() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        DamageRoll damageRoll = new DamageRoll()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal("""[]""", damageRoll.GetDamage().ToString());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "prepares damage")]
    public void PreparesDamage() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        DamageRoll damageRoll = new DamageRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": 1,
                            "dice": [
                                { "size": 6, "determined": [ 3 ] }
                            ]
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal("""
            [
              {
                "bonus": 1,
                "damage_type": "fire",
                "dice": [
                  {
                    "determined": [ ],
                    "roll": 3,
                    "size": 6
                  }
                ]
              }
            ]
            """,
            damageRoll.GetDamage().PrettyPrint()
        );
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "includes damage type")]
    public void DoesIncludeDamageType() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        DamageRoll damageRoll = new DamageRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": 1,
                            "dice": [
                                { "size": 6, "determined": [ 3 ] }
                            ]
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.True(damageRoll.IncludesDamageType("fire"));
        Assert.False(damageRoll.IncludesDamageType("cold"));
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "rerolls typed damage dice")]
    public void RerollsTypedDamageDice() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        DamageRoll damageRoll = new DamageRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": 1,
                            "dice": [
                                { "size": 6, "determined": [ 1, -1 ] },
                                { "size": 6, "determined": [ 3, 4, -1 ] },
                                { "size": 6, "determined": [ 6, -1 ] }
                            ]
                        },
                        {
                            "damage_type": "cold",
                            "bonus": 1,
                            "dice": [
                                { "size": 6, "determined": [ 3, -1 ] }
                            ]
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        damageRoll.RerollDamageDice("fire", 2, 5);

        Assert.Equal("""
            [
              {
                "bonus": 1,
                "damage_type": "fire",
                "dice": [
                  {
                    "determined": [
                      -1
                    ],
                    "roll": 1,
                    "size": 6
                  },
                  {
                    "determined": [
                      -1
                    ],
                    "roll": 4,
                    "size": 6
                  },
                  {
                    "determined": [
                      -1
                    ],
                    "roll": 6,
                    "size": 6
                  }
                ]
              },
              {
                "bonus": 1,
                "damage_type": "cold",
                "dice": [
                  {
                    "determined": [
                      -1
                    ],
                    "roll": 3,
                    "size": 6
                  }
                ]
              }
            ]
            """,
            damageRoll.GetDamage().PrettyPrint()
        );
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "rerolls untyped damage dice")]
    public void RerollsUntypedDamageDice() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        DamageRoll damageRoll = new DamageRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": 1,
                            "dice": [
                                { "size": 6, "determined": [ 3, 6, -1 ] }
                            ]
                        },
                        {
                            "damage_type": "cold",
                            "bonus": 1,
                            "dice": [
                                { "size": 6, "determined": [ 3, 6, -1 ] }
                            ]
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        damageRoll.RerollDamageDice("", 1, 6);

        Assert.Equal("""
            [
              {
                "bonus": 1,
                "damage_type": "fire",
                "dice": [
                  {
                    "determined": [
                      -1
                    ],
                    "roll": 6,
                    "size": 6
                  }
                ]
              },
              {
                "bonus": 1,
                "damage_type": "cold",
                "dice": [
                  {
                    "determined": [
                      -1
                    ],
                    "roll": 6,
                    "size": 6
                  }
                ]
              }
            ]
            """,
            damageRoll.GetDamage().PrettyPrint()
        );
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "sets typed damage dice")]
    public void SetsTypedDamageDice() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        DamageRoll damageRoll = new DamageRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": 1,
                            "dice": [
                                { "size": 6, "determined": [ 1, -1 ] },
                                { "size": 6, "determined": [ 3, -1 ] },
                                { "size": 6, "determined": [ 6, -1 ] }
                            ]
                        },
                        {
                            "damage_type": "cold",
                            "bonus": 1,
                            "dice": [
                                { "size": 6, "determined": [ 3, -1 ] }
                            ]
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        damageRoll.SetDamageDice("fire", 6, 2, 5);

        Assert.Equal("""
            [
              {
                "bonus": 1,
                "damage_type": "fire",
                "dice": [
                  {
                    "determined": [
                      -1
                    ],
                    "roll": 1,
                    "size": 6
                  },
                  {
                    "determined": [
                      -1
                    ],
                    "roll": 6,
                    "size": 6
                  },
                  {
                    "determined": [
                      -1
                    ],
                    "roll": 6,
                    "size": 6
                  }
                ]
              },
              {
                "bonus": 1,
                "damage_type": "cold",
                "dice": [
                  {
                    "determined": [
                      -1
                    ],
                    "roll": 3,
                    "size": 6
                  }
                ]
              }
            ]
            """,
            damageRoll.GetDamage().PrettyPrint()
        );
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "sets untyped damage dice")]
    public void SetsUntypedDamageDice() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        DamageRoll damageRoll = new DamageRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": 1,
                            "dice": [
                                { "size": 6, "determined": [ 3, -1 ] }
                            ]
                        },
                        {
                            "damage_type": "cold",
                            "bonus": 1,
                            "dice": [
                                { "size": 6, "determined": [ 3, -1 ] }
                            ]
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        damageRoll.SetDamageDice("", 5, 1, 6);

        Assert.Equal("""
            [
              {
                "bonus": 1,
                "damage_type": "fire",
                "dice": [
                  {
                    "determined": [
                      -1
                    ],
                    "roll": 5,
                    "size": 6
                  }
                ]
              },
              {
                "bonus": 1,
                "damage_type": "cold",
                "dice": [
                  {
                    "determined": [
                      -1
                    ],
                    "roll": 5,
                    "size": 6
                  }
                ]
              }
            ]
            """,
            damageRoll.GetDamage().PrettyPrint()
        );
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "maximizes typed damage dice")]
    public void MaximizesTypedDamageDice() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        DamageRoll damageRoll = new DamageRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": 1,
                            "dice": [
                                { "size": 6, "determined": [ 1, -1 ] }
                            ]
                        },
                        {
                            "damage_type": "cold",
                            "bonus": 1,
                            "dice": [
                                { "size": 6, "determined": [ 1, -1 ] }
                            ]
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        damageRoll.MaximizeDamageDice("fire");

        Assert.Equal("""
            [
              {
                "bonus": 1,
                "damage_type": "fire",
                "dice": [
                  {
                    "determined": [
                      -1
                    ],
                    "roll": 6,
                    "size": 6
                  }
                ]
              },
              {
                "bonus": 1,
                "damage_type": "cold",
                "dice": [
                  {
                    "determined": [
                      -1
                    ],
                    "roll": 1,
                    "size": 6
                  }
                ]
              }
            ]
            """,
            damageRoll.GetDamage().PrettyPrint()
        );
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "maximizes untyped damage dice")]
    public void MaximizesUntypedDamageDice() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        DamageRoll damageRoll = new DamageRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": 1,
                            "dice": [
                                { "size": 6, "determined": [ 1, -1 ] }
                            ]
                        },
                        {
                            "damage_type": "cold",
                            "bonus": 1,
                            "dice": [
                                { "size": 6, "determined": [ 1, -1 ] }
                            ]
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        damageRoll.MaximizeDamageDice("");

        Assert.Equal("""
            [
              {
                "bonus": 1,
                "damage_type": "fire",
                "dice": [
                  {
                    "determined": [
                      -1
                    ],
                    "roll": 6,
                    "size": 6
                  }
                ]
              },
              {
                "bonus": 1,
                "damage_type": "cold",
                "dice": [
                  {
                    "determined": [
                      -1
                    ],
                    "roll": 6,
                    "size": 6
                  }
                ]
              }
            ]
            """,
            damageRoll.GetDamage().PrettyPrint()
        );
    }

};
