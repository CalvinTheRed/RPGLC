using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class DamageRollTest {

    [Fact(DisplayName = "defaults")]
    public void Defaults() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new DamageRoll());

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Empty((subevent.subevent as DamageRoll).GetDamage().AsList());
    }

    [DieTestingMode]
    [Fact(DisplayName = "rolls damage")]
    public void RollsDamage() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new DamageRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": 1,
                            "dice": [
                                { "size": 6, "determined": [ 3, -1 ] }
                            ]
                        }
                    ]
                }
                """)));

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);
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
                    "roll": 3,
                    "size": 6
                  }
                ]
              }
            ]
            """, (subevent.subevent as DamageRoll).GetDamage().PrettyPrint());
    }

    [DieTestingMode]
    [Fact(DisplayName = "does include damage type")]
    public void DoesIncludeDamageType() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new DamageRoll()
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
                """)));

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.True((subevent.subevent as DamageRoll).IncludesDamageType("fire"));
    }

    [DieTestingMode]
    [Fact(DisplayName = "does not include damage type")]
    public void DoesNotIncludeDamageType() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new DamageRoll()
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
                """)));

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.False((subevent.subevent as DamageRoll).IncludesDamageType("cold"));
    }

    [DieTestingMode]
    [Fact(DisplayName = "rerolls damage dice (typed)")]
    public void RerollsDamageDiceTyped() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new DamageRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": 1,
                            "dice": [
                                { "size": 6, "determined": [ 3, 6, -1 ] },
                                { "size": 6, "determined": [ 3, 6, -1 ] }
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
                """)));

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);

        (subevent.subevent as DamageRoll).RerollDamageDice("fire", 1, 6);

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
            """, (subevent.subevent as DamageRoll).GetDamage().PrettyPrint());
    }

    [DieTestingMode]
    [Fact(DisplayName = "rerolls damage dice (untyped)")]
    public void RerollsDamageDiceUntyped() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new DamageRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": 1,
                            "dice": [
                                { "size": 6, "determined": [ 3, 6, -1 ] },
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
                """)));

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);

        (subevent.subevent as DamageRoll).RerollDamageDice("*", 1, 6);

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
                    "roll": 6,
                    "size": 6
                  }
                ]
              }
            ]
            """, (subevent.subevent as DamageRoll).GetDamage().PrettyPrint());
    }

    [DieTestingMode]
    [Fact(DisplayName = "maximizes damage dice (typed)")]
    public void MaximizesDamageDiceTyped() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new DamageRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": 1,
                            "dice": [
                                { "size": 6, "determined": [ 3, -1 ] },
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
                """)));

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);

        (subevent.subevent as DamageRoll).MaximizeDamageDice("fire");

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
            """, (subevent.subevent as DamageRoll).GetDamage().PrettyPrint());
    }

    [DieTestingMode]
    [Fact(DisplayName = "maximizes damage dice (untyped)")]
    public void MaximizesDamageDiceUntyped() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new DamageRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": 1,
                            "dice": [
                                { "size": 6, "determined": [ 3, -1 ] },
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
                """)));

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);

        (subevent.subevent as DamageRoll).MaximizeDamageDice("*");

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
                    "roll": 6,
                    "size": 6
                  }
                ]
              }
            ]
            """, (subevent.subevent as DamageRoll).GetDamage().PrettyPrint());
    }

};
