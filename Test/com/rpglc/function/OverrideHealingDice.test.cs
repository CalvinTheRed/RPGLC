﻿using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class OverrideHealingDiceTest {

    [Fact(DisplayName = "overrides unbounded healing dice")]
    public void OverridesUnboundedHealingDice() {
        HealingRoll healingRoll = new HealingRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "healing": [
                        {
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

        new OverrideHealingDice().Execute(
            new RPGLEffect(),
            healingRoll,
            new JsonObject().LoadFromString("""
                {
                    "function": "override_healing_dice",
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
            """, healingRoll.GetHealing().PrettyPrint());
    }

    [Fact(DisplayName = "overrides bounded healing dice")]
    public void OverridesBoundedHealingDice() {
        HealingRoll healingRoll = new HealingRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "healing": [
                        {
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

        new OverrideHealingDice().Execute(
            new(),
            healingRoll,
            new JsonObject().LoadFromString("""
                {
                    "function": "override_healing_dice",
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
            """, healingRoll.GetHealing().PrettyPrint());
    }

};
