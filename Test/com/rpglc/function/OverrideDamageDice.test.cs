﻿using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class OverrideDamageDiceTest {

    [Fact(DisplayName = "overrides wild card unbounded damage dice")]
    public void OverridesWildCardUnboundedDamageDice() {
        DamageRoll damageRoll = new DamageRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": 0,
                            "dice": [
                                { "size": 6, "determined": [ 1, -1 ] },
                                { "size": 6, "determined": [ 3, -1 ] },
                                { "size": 6, "determined": [ 6, -1 ] }
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
                                { "size": 6, "determined": [ 1, -1 ] },
                                { "size": 6, "determined": [ 3, -1 ] },
                                { "size": 6, "determined": [ 6, -1 ] }
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

        new OverrideDamageDice().Execute(
            new RPGLEffect(),
            damageRoll,
            new JsonObject().LoadFromString("""
                {
                    "function": "override_damage_dice",
                    "override": {
                        "formula": "number",
                        "number": 4
                    }
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
                      -1
                    ],
                    "roll": 4,
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
                    "roll": 4,
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
                      -1
                    ],
                    "roll": 4,
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
                    "roll": 4,
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
            """, damageRoll.GetDamage().PrettyPrint());
    }

    [Fact(DisplayName = "overrides wild card bounded damage dice")]
    public void OverridesWildCardBoundedDamageDice() {
        DamageRoll damageRoll = new DamageRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": 0,
                            "dice": [
                                { "size": 6, "determined": [ 1, -1 ] },
                                { "size": 6, "determined": [ 3, -1 ] },
                                { "size": 6, "determined": [ 6, -1 ] }
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
                                { "size": 6, "determined": [ 1, -1 ] },
                                { "size": 6, "determined": [ 3, -1 ] },
                                { "size": 6, "determined": [ 6, -1 ] }
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

        new OverrideDamageDice().Execute(
            new RPGLEffect(),
            damageRoll,
            new JsonObject().LoadFromString("""
                {
                    "function": "override_damage_dice",
                    "override": {
                        "formula": "number",
                        "number": 4
                    },
                    "lower_bound": 2,
                    "upper_bound": 5
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
                ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, damageRoll.GetDamage().PrettyPrint());
    }

    [Fact(DisplayName = "overrides typed unbounded damage dice")]
    public void OverridesTypedUnboundedDamageDice() {
        DamageRoll damageRoll = new DamageRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": 0,
                            "dice": [
                                { "size": 6, "determined": [ 1, -1 ] },
                                { "size": 6, "determined": [ 3, -1 ] },
                                { "size": 6, "determined": [ 6, -1 ] }
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
                                { "size": 6, "determined": [ 1, -1 ] },
                                { "size": 6, "determined": [ 3, -1 ] },
                                { "size": 6, "determined": [ 6, -1 ] }
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

        new OverrideDamageDice().Execute(
            new RPGLEffect(),
            damageRoll,
            new JsonObject().LoadFromString("""
                {
                    "function": "override_damage_dice",
                    "damage_type": "fire",
                    "override": {
                        "formula": "number",
                        "number": 4
                    }
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
                      -1
                    ],
                    "roll": 4,
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
                    "roll": 4,
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
                      -1
                    ],
                    "roll": 1,
                    "size": 6
                  },
                  {
                    "determined": [
                      -1
                    ],
                    "roll": 3,
                    "size": 6
                  },
                  {
                    "determined": [
                      -1
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
            """, damageRoll.GetDamage().PrettyPrint());
    }

    [Fact(DisplayName = "overrides typed bounded damage dice")]
    public void OverridesTypedBoundedDamageDice() {
        DamageRoll damageRoll = new DamageRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": 0,
                            "dice": [
                                { "size": 6, "determined": [ 1, -1 ] },
                                { "size": 6, "determined": [ 3, -1 ] },
                                { "size": 6, "determined": [ 6, -1 ] }
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
                                { "size": 6, "determined": [ 1, -1 ] },
                                { "size": 6, "determined": [ 3, -1 ] },
                                { "size": 6, "determined": [ 6, -1 ] }
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

        new OverrideDamageDice().Execute(
            new RPGLEffect(),
            damageRoll,
            new JsonObject().LoadFromString("""
                {
                    "function": "override_damage_dice",
                    "damage_type": "fire",
                    "override": {
                      "formula": "number",
                      "number": 4
                    },
                    "lower_bound": 2,
                    "upper_bound": 5
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
                      -1
                    ],
                    "roll": 1,
                    "size": 6
                  },
                  {
                    "determined": [
                      -1
                    ],
                    "roll": 3,
                    "size": 6
                  },
                  {
                    "determined": [
                      -1
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
            """, damageRoll.GetDamage().PrettyPrint());
    }

};
