using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.testutils;
using com.rpglc.testutils.mocks;

namespace com.rpglc.subevent;

[AssignDatabase]
[Collection("Serial")]
public class DamageRollTest {

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "prepares with no damage")]
    public void PreparesWithNoDamage() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DamageRoll damageRoll = new DamageRoll()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal("""[]""", damageRoll.GetDamage().ToString());
    }

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [DieTestingMode]
    [Fact(DisplayName = "prepares with damage")]
    public void PreparesWithDamage() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DamageRoll damageRoll = new DamageRoll()
            .JoinSubeventData(new JsonObject()
                /*{
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": 1,
                            "dice": [
                                { "size": 6, "determined": [ 3 ] }
                            ]
                        }
                    ]
                }*/
                .PutJsonArray("damage", new JsonArray()
                    .AddJsonObject(new JsonObject()
                        .PutString("damage_type", "fire")
                        .PutLong("bonus", 1)
                        .PutJsonArray("dice", new JsonArray()
                            .AddJsonObject(new JsonObject()
                                .PutLong("size", 6)
                                .PutJsonArray("determined", new JsonArray()
                                    .AddLong(3)
                                )
                            )
                        )
                    )
                )
            )
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

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [DieTestingMode]
    [Fact(DisplayName = "includes damage type")]
    public void DoesIncludeDamageType() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DamageRoll damageRoll = new DamageRoll()
            .JoinSubeventData(new JsonObject()
                /*{
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": 1,
                            "dice": [
                                { "size": 6, "determined": [ 3 ] }
                            ]
                        }
                    ]
                }*/
                .PutJsonArray("damage", new JsonArray()
                    .AddJsonObject(new JsonObject()
                        .PutString("damage_type", "fire")
                        .PutLong("bonus", 1)
                        .PutJsonArray("dice", new JsonArray()
                            .AddJsonObject(new JsonObject()
                                .PutLong("size", 6)
                                .PutJsonArray("determined", new JsonArray()
                                    .AddLong(3)
                                )
                            )
                        )
                    )
                )
            )
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.True(damageRoll.IncludesDamageType("fire"));
        Assert.False(damageRoll.IncludesDamageType("cold"));
    }

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [DieTestingMode]
    [Fact(DisplayName = "rerolls typed damage dice")]
    public void RerollsTypedDamageDice() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DamageRoll damageRoll = new DamageRoll()
            .JoinSubeventData(new JsonObject()
                /*{
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
                }*/
                .PutJsonArray("damage", new JsonArray()
                    .AddJsonObject(new JsonObject()
                        .PutString("damage_type", "fire")
                        .PutLong("bonus", 1)
                        .PutJsonArray("dice", new JsonArray()
                            .AddJsonObject(new JsonObject()
                                .PutLong("size", 6)
                                .PutJsonArray("determined", new JsonArray()
                                    .AddLong(1)
                                    .AddLong(-1)
                                )
                            )
                            .AddJsonObject(new JsonObject()
                                .PutLong("size", 6)
                                .PutJsonArray("determined", new JsonArray()
                                    .AddLong(3)
                                    .AddLong(4)
                                    .AddLong(-1)
                                )
                            )
                            .AddJsonObject(new JsonObject()
                                .PutLong("size", 6)
                                .PutJsonArray("determined", new JsonArray()
                                    .AddLong(6)
                                    .AddLong(-1)
                                )
                            )
                        )
                    )
                    .AddJsonObject(new JsonObject()
                        .PutString("damage_type", "cold")
                        .PutLong("bonus", 1)
                        .PutJsonArray("dice", new JsonArray()
                            .AddJsonObject(new JsonObject()
                                .PutLong("size", 6)
                                .PutJsonArray("determined", new JsonArray()
                                    .AddLong(3)
                                    .AddLong(-1)
                                )
                            )
                        )
                    )
                )
            )
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

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [DieTestingMode]
    [Fact(DisplayName = "rerolls typeless damage dice")]
    public void RerollsTypelessDamageDice() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DamageRoll damageRoll = new DamageRoll()
            .JoinSubeventData(new JsonObject()
                /*{
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
                }*/
                .PutJsonArray("damage", new JsonArray()
                    .AddJsonObject(new JsonObject()
                        .PutString("damage_type", "fire")
                        .PutLong("bonus", 1)
                        .PutJsonArray("dice", new JsonArray()
                            .AddJsonObject(new JsonObject()
                                .PutLong("size", 6)
                                .PutJsonArray("determined", new JsonArray()
                                    .AddLong(3)
                                    .AddLong(6)
                                    .AddLong(-1)
                                )
                            )
                        )
                    )
                    .AddJsonObject(new JsonObject()
                        .PutString("damage_type", "cold")
                        .PutLong("bonus", 1)
                        .PutJsonArray("dice", new JsonArray()
                            .AddJsonObject(new JsonObject()
                                .PutLong("size", 6)
                                .PutJsonArray("determined", new JsonArray()
                                    .AddLong(3)
                                    .AddLong(6)
                                    .AddLong(-1)
                                )
                            )
                        )
                    )
                )
            )
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

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [DieTestingMode]
    [Fact(DisplayName = "sets typed damage dice")]
    public void SetsTypedDamageDice() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DamageRoll damageRoll = new DamageRoll()
            .JoinSubeventData(new JsonObject()
                /*{
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
                }*/
                .PutJsonArray("damage", new JsonArray()
                    .AddJsonObject(new JsonObject()
                        .PutString("damage_type", "fire")
                        .PutLong("bonus", 1)
                        .PutJsonArray("dice", new JsonArray()
                            .AddJsonObject(new JsonObject()
                                .PutLong("size", 6)
                                .PutJsonArray("determined", new JsonArray()
                                    .AddLong(1)
                                    .AddLong(-1)
                                )
                            )
                            .AddJsonObject(new JsonObject()
                                .PutLong("size", 6)
                                .PutJsonArray("determined", new JsonArray()
                                    .AddLong(3)
                                    .AddLong(-1)
                                )
                            )
                            .AddJsonObject(new JsonObject()
                                .PutLong("size", 6)
                                .PutJsonArray("determined", new JsonArray()
                                    .AddLong(6)
                                    .AddLong(-1)
                                )
                            )
                        )
                    )
                    .AddJsonObject(new JsonObject()
                        .PutString("damage_type", "cold")
                        .PutLong("bonus", 1)
                        .PutJsonArray("dice", new JsonArray()
                            .AddJsonObject(new JsonObject()
                                .PutLong("size", 6)
                                .PutJsonArray("determined", new JsonArray()
                                    .AddLong(3)
                                    .AddLong(-1)
                                )
                            )
                        )
                    )
                )
            )
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

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [DieTestingMode]
    [Fact(DisplayName = "sets typeless damage dice")]
    public void SetsTypelessDamageDice() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DamageRoll damageRoll = new DamageRoll()
            .JoinSubeventData(new JsonObject()
                /*{
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
                }*/
                .PutJsonArray("damage", new JsonArray()
                    .AddJsonObject(new JsonObject()
                        .PutString("damage_type", "fire")
                        .PutLong("bonus", 1)
                        .PutJsonArray("dice", new JsonArray()
                            .AddJsonObject(new JsonObject()
                                .PutLong("size", 6)
                                .PutJsonArray("determined", new JsonArray()
                                    .AddLong(3)
                                    .AddLong(-1)
                                )
                            )
                        )
                    )
                    .AddJsonObject(new JsonObject()
                        .PutString("damage_type", "cold")
                        .PutLong("bonus", 1)
                        .PutJsonArray("dice", new JsonArray()
                            .AddJsonObject(new JsonObject()
                                .PutLong("size", 6)
                                .PutJsonArray("determined", new JsonArray()
                                    .AddLong(3)
                                    .AddLong(-1)
                                )
                            )
                        )
                    )
                )
            )
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

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [DieTestingMode]
    [Fact(DisplayName = "maximizes typed damage dice")]
    public void MaximizesTypedDamageDice() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DamageRoll damageRoll = new DamageRoll()
            .JoinSubeventData(new JsonObject()
                /*{
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
                }*/
                .PutJsonArray("damage", new JsonArray()
                    .AddJsonObject(new JsonObject()
                        .PutString("damage_type", "fire")
                        .PutLong("bonus", 1)
                        .PutJsonArray("dice", new JsonArray()
                            .AddJsonObject(new JsonObject()
                                .PutLong("size", 6)
                                .PutJsonArray("determined", new JsonArray()
                                    .AddLong(1)
                                    .AddLong(-1)
                                )
                            )
                        )
                    )
                    .AddJsonObject(new JsonObject()
                        .PutString("damage_type", "cold")
                        .PutLong("bonus", 1)
                        .PutJsonArray("dice", new JsonArray()
                            .AddJsonObject(new JsonObject()
                                .PutLong("size", 6)
                                .PutJsonArray("determined", new JsonArray()
                                    .AddLong(1)
                                    .AddLong(-1)
                                )
                            )
                        )
                    )
                )
            )
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

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [DieTestingMode]
    [Fact(DisplayName = "maximizes typeless damage dice")]
    public void MaximizesTypelessDamageDice() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DamageRoll damageRoll = new DamageRoll()
            .JoinSubeventData(new JsonObject()
                /*{
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
                }*/
                .PutJsonArray("damage", new JsonArray()
                    .AddJsonObject(new JsonObject()
                        .PutString("damage_type", "fire")
                        .PutLong("bonus", 1)
                        .PutJsonArray("dice", new JsonArray()
                            .AddJsonObject(new JsonObject()
                                .PutLong("size", 6)
                                .PutJsonArray("determined", new JsonArray()
                                    .AddLong(1)
                                    .AddLong(-1)
                                )
                            )
                        )
                    )
                    .AddJsonObject(new JsonObject()
                        .PutString("damage_type", "cold")
                        .PutLong("bonus", 1)
                        .PutJsonArray("dice", new JsonArray()
                            .AddJsonObject(new JsonObject()
                                .PutLong("size", 6)
                                .PutJsonArray("determined", new JsonArray()
                                    .AddLong(1)
                                    .AddLong(-1)
                                )
                            )
                        )
                    )
                )
            )
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
