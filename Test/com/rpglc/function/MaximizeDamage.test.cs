using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[AssignDatabase]
[Collection("Serial")]
public class MaximizeDamageTest {

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "maximizes specified damage type (damage roll)")]
    public void MaximizesSpecificDamageType_DamageRoll() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        Subevent subevent = new DamageRoll()
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
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        new MaximizeDamage().Execute(
            new RPGLEffect(),
            subevent,
            new JsonObject().LoadFromString("""
                {
                    "function": "maximize_damage",
                    "damage_type": "fire"
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
                    "roll": 3,
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

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "maximizes specified damage type (damage delivery)")]
    public void MaximizesSpecificDamageType_DamageDelivery() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        Subevent subevent = new DamageDelivery()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": 0,
                            "dice": [
                                { "roll": 3, "size": 6, "determined": [ ] }
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
                                { "roll": 3, "size": 6, "determined": [ ] }
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
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        new MaximizeDamage().Execute(
            new RPGLEffect(),
            subevent,
            new JsonObject().LoadFromString("""
                {
                    "function": "maximize_damage",
                    "damage_type": "fire"
                }
                """),
            new DummyContext(),
            new()
        );

        subevent
            .SetTarget(rpglObject)
            .Invoke(new DummyContext(), new());

        Assert.Equal("""
            {
              "cold": 3,
              "fire": 6
            }
            """, (subevent as DamageDelivery).GetDamage().PrettyPrint());
    }

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "maximizes every damage type (damage roll)")]
    public void MaximizesEveryDamageType_DamageRoll() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        Subevent subevent = new DamageRoll()
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
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        new MaximizeDamage().Execute(
            new RPGLEffect(),
            subevent,
            new JsonObject().LoadFromString("""
                {
                    "function": "maximize_damage"
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

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "maximizes every damage type (damage delivery)")]
    public void MaximizesEveryDamageType_DamageDelivery() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        Subevent subevent = new DamageDelivery()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": 0,
                            "dice": [
                                { "roll": 3, "size": 6, "determined": [ ] }
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
                                { "roll": 3, "size": 6, "determined": [ ] }
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
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        new MaximizeDamage().Execute(
            new RPGLEffect(),
            subevent,
            new JsonObject().LoadFromString("""
                {
                    "function": "maximize_damage"
                }
                """),
            new DummyContext(),
            new()
        );

        subevent
            .SetTarget(rpglObject)
            .Invoke(new DummyContext(), new());

        Assert.Equal("""
            {
              "cold": 6,
              "fire": 6
            }
            """, (subevent as DamageDelivery).GetDamage().PrettyPrint());
    }

};
