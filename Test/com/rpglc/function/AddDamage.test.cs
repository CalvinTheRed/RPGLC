using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class AddDamageTest {

    [Fact(DisplayName = "adds damage (number)")]
    public void AddsDamageNumber() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DamageCollection().JoinSubeventData(new JsonObject().LoadFromString(
            """{ "damage": [ ] }"""
        ));

        AddDamage addDamage = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "add_damage",
                "bonus": [
                    {
                        "formula": "number",
                        "number": 1,
                        "damage_type": "fire"
                    },
                    {
                        "formula": "number",
                        "number": 2,
                        "damage_type": "fire"
                    }
                ]
            }
            """);

        FunctionState.StateData result;

        result = addDamage.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, completed = false }, result);
        Assert.Equal("""
            [
              {
                "bonus": 1,
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as DamageCollection).GetDamageCollection().PrettyPrint());

        result = addDamage.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, completed = false }, result);
        Assert.Equal("""
            [
              {
                "bonus": 1,
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              },
              {
                "bonus": 2,
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as DamageCollection).GetDamageCollection().PrettyPrint());

        result = addDamage.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, completed = true }, result);
        Assert.Equal("""
            [
              {
                "bonus": 1,
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              },
              {
                "bonus": 2,
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as DamageCollection).GetDamageCollection().PrettyPrint());
    }

    [DieTestingMode]
    [Fact(DisplayName = "adds damage (dice)")]
    public void AddsDamageDice() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DamageCollection().JoinSubeventData(new JsonObject().LoadFromString(
            """{ "damage": [ ] }"""
        ));

        AddDamage addDamage = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "add_damage",
                "bonus": [
                    {
                        "formula": "dice",
                        "dice": [
                            { "count": 1, "size": 6, "determined": [ 1 ] }
                        ],
                        "damage_type": "fire"
                    },
                    {
                        "formula": "dice",
                        "dice": [
                            { "count": 1, "size": 6, "determined": [ 2 ] }
                        ],
                        "damage_type": "fire"
                    }
                ]
            }
            """);

        FunctionState.StateData result;

        result = addDamage.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, completed = false }, result);
        Assert.Equal("""
            [
              {
                "bonus": 0,
                "damage_type": "fire",
                "dice": [
                  {
                    "determined": [
                      1
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
            """, (subevent as DamageCollection).GetDamageCollection().PrettyPrint());

        result = addDamage.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, completed = false }, result);
        Assert.Equal("""
            [
              {
                "bonus": 0,
                "damage_type": "fire",
                "dice": [
                  {
                    "determined": [
                      1
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
                "damage_type": "fire",
                "dice": [
                  {
                    "determined": [
                      2
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
            """, (subevent as DamageCollection).GetDamageCollection().PrettyPrint());

        result = addDamage.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, completed = true }, result);
        Assert.Equal("""
            [
              {
                "bonus": 0,
                "damage_type": "fire",
                "dice": [
                  {
                    "determined": [
                      1
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
                "damage_type": "fire",
                "dice": [
                  {
                    "determined": [
                      2
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
            """, (subevent as DamageCollection).GetDamageCollection().PrettyPrint());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "adds damage (modifier)")]
    public void AddsDamageModifier() {
        long strScore = 12L;
        long dexScore = 14L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.GetAbilityScores().PutLong("str", strScore);
        rpglObject.GetAbilityScores().PutLong("dex", dexScore);
        RPGLContext context = new DummyContext().Add(rpglObject);
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DamageCollection().JoinSubeventData(new JsonObject().LoadFromString(
            """{ "damage": [ ] }"""
        )).SetSource(rpglObject);

        AddDamage addDamage = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "add_damage",
                "bonus": [
                    {
                        "formula": "modifier",
                        "object": {
                            "from": "subevent",
                            "object": "source"
                        },
                        "ability": "str",
                        "damage_type": "fire"
                    },
                    {
                        "formula": "modifier",
                        "object": {
                            "from": "subevent",
                            "object": "source"
                        },
                        "ability": "dex",
                        "damage_type": "fire"
                    }
                ]
            }
            """);

        FunctionState.StateData result;

        result = addDamage.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.NotNull(result.dependency);
        Assert.False(result.completed);
        Assert.Equal("[ ]", (subevent as DamageCollection).GetDamageCollection().PrettyPrint());
        Assert.True(result.dependency is CalculateAbilityScore);

        (result.dependency as CalculateAbilityScore)
            .SetMinimum(long.MinValue)
            .SetBase(strScore)
            .JoinSubeventData(new JsonObject().LoadFromString("""{ "bonuses": [ ] }"""));

        result = addDamage.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, completed = false }, result);
        Assert.Equal("""
            [
              {
                "bonus": 1,
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as DamageCollection).GetDamageCollection().PrettyPrint());

        result = addDamage.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.NotNull(result.dependency);
        Assert.False(result.completed);
        Assert.Equal("""
            [
              {
                "bonus": 1,
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as DamageCollection).GetDamageCollection().PrettyPrint());
        Assert.True(result.dependency is CalculateAbilityScore);

        (result.dependency as CalculateAbilityScore)
            .SetMinimum(long.MinValue)
            .SetBase(dexScore)
            .JoinSubeventData(new JsonObject().LoadFromString("""{ "bonuses": [ ] }"""));

        result = addDamage.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, completed = false }, result);
        Assert.Equal("""
            [
              {
                "bonus": 1,
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              },
              {
                "bonus": 2,
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as DamageCollection).GetDamageCollection().PrettyPrint());

        result = addDamage.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, completed = true }, result);
        Assert.Equal("""
            [
              {
                "bonus": 1,
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              },
              {
                "bonus": 2,
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as DamageCollection).GetDamageCollection().PrettyPrint());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "adds damage (ability)")]
    public void AddsDamageAbility() {
        long strScore = 12L;
        long dexScore = 14L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.GetAbilityScores().PutLong("str", strScore);
        rpglObject.GetAbilityScores().PutLong("dex", dexScore);
        RPGLContext context = new DummyContext().Add(rpglObject);
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DamageCollection().JoinSubeventData(new JsonObject().LoadFromString(
            """{ "damage": [ ] }"""
        )).SetSource(rpglObject);

        AddDamage addDamage = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "add_damage",
                "bonus": [
                    {
                        "formula": "ability",
                        "object": {
                            "from": "subevent",
                            "object": "source"
                        },
                        "ability": "str",
                        "damage_type": "fire"
                    },
                    {
                        "formula": "ability",
                        "object": {
                            "from": "subevent",
                            "object": "source"
                        },
                        "ability": "dex",
                        "damage_type": "fire"
                    }
                ]
            }
            """);

        FunctionState.StateData result;

        result = addDamage.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.NotNull(result.dependency);
        Assert.False(result.completed);
        Assert.Equal("[ ]", (subevent as DamageCollection).GetDamageCollection().PrettyPrint());
        Assert.True(result.dependency is CalculateAbilityScore);

        (result.dependency as CalculateAbilityScore)
            .SetMinimum(long.MinValue)
            .SetBase(strScore)
            .JoinSubeventData(new JsonObject().LoadFromString("""{ "bonuses": [ ] }"""));

        result = addDamage.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, completed = false }, result);
        Assert.Equal("""
            [
              {
                "bonus": 12,
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as DamageCollection).GetDamageCollection().PrettyPrint());

        result = addDamage.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.NotNull(result.dependency);
        Assert.False(result.completed);
        Assert.Equal("""
            [
              {
                "bonus": 12,
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as DamageCollection).GetDamageCollection().PrettyPrint());
        Assert.True(result.dependency is CalculateAbilityScore);

        (result.dependency as CalculateAbilityScore)
            .SetMinimum(long.MinValue)
            .SetBase(dexScore)
            .JoinSubeventData(new JsonObject().LoadFromString("""{ "bonuses": [ ] }"""));

        result = addDamage.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, completed = false }, result);
        Assert.Equal("""
            [
              {
                "bonus": 12,
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              },
              {
                "bonus": 14,
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as DamageCollection).GetDamageCollection().PrettyPrint());

        result = addDamage.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, completed = true }, result);
        Assert.Equal("""
            [
              {
                "bonus": 12,
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              },
              {
                "bonus": 14,
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as DamageCollection).GetDamageCollection().PrettyPrint());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "adds damage (proficiency)")]
    public void AddsDamageProficiency() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext().Add(rpglObject);
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DamageCollection().JoinSubeventData(new JsonObject().LoadFromString(
            """{ "damage": [ ] }"""
        )).SetSource(rpglObject);

        AddDamage addDamage = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "add_damage",
                "bonus": [
                    {
                        "formula": "proficiency",
                        "object": {
                            "from": "subevent",
                            "object": "source"
                        },
                        "damage_type": "fire"
                    },
                    {
                        "formula": "proficiency",
                        "object": {
                            "from": "subevent",
                            "object": "source"
                        },
                        "damage_type": "fire"
                    }
                ]
            }
            """);

        FunctionState.StateData result;

        result = addDamage.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.NotNull(result.dependency);
        Assert.False(result.completed);
        Assert.Equal("[ ]", (subevent as DamageCollection).GetDamageCollection().PrettyPrint());
        Assert.True(result.dependency is CalculateProficiencyBonus);

        (result.dependency as CalculateProficiencyBonus)
            .SetMinimum(long.MinValue)
            .SetBase(2)
            .JoinSubeventData(new JsonObject().LoadFromString("""{ "bonuses": [ ] }"""));

        result = addDamage.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, completed = false }, result);
        Assert.Equal("""
            [
              {
                "bonus": 2,
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as DamageCollection).GetDamageCollection().PrettyPrint());

        result = addDamage.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.NotNull(result.dependency);
        Assert.False(result.completed);
        Assert.Equal("""
            [
              {
                "bonus": 2,
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as DamageCollection).GetDamageCollection().PrettyPrint());
        Assert.True(result.dependency is CalculateProficiencyBonus);

        (result.dependency as CalculateProficiencyBonus)
            .SetMinimum(long.MinValue)
            .SetBase(2)
            .JoinSubeventData(new JsonObject().LoadFromString("""{ "bonuses": [ ] }"""));

        result = addDamage.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, completed = false }, result);
        Assert.Equal("""
            [
              {
                "bonus": 2,
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              },
              {
                "bonus": 2,
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as DamageCollection).GetDamageCollection().PrettyPrint());

        result = addDamage.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, completed = true }, result);
        Assert.Equal("""
            [
              {
                "bonus": 2,
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              },
              {
                "bonus": 2,
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as DamageCollection).GetDamageCollection().PrettyPrint());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [ExtraClassesMock]
    [Fact(DisplayName = "adds damage (level)")]
    public void AddsDamageLevel() {
        long firstClassLevel = 1;
        long secondClassLevel = 2;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.SetClasses(new JsonArray().LoadFromString($$"""
            [
                {
                    "additional_nested_classes": { },
                    "id": "test:dummy",
                    "level": {{firstClassLevel}},
                    "name": "Dummy"
                },
                {
                    "additional_nested_classes": { },
                    "id": "test:nested_class",
                    "level": {{secondClassLevel}},
                    "name": "Nested Class"
                }
            ]
            """));
        RPGLContext context = new DummyContext().Add(rpglObject);
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DamageCollection().JoinSubeventData(new JsonObject().LoadFromString(
            """{ "damage": [ ] }"""
        )).SetSource(rpglObject);

        AddDamage addDamage = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "add_damage",
                "bonus": [
                    {
                        "formula": "level",
                        "object": {
                            "from": "subevent",
                            "object": "source"
                        },
                        "class": "test:dummy",
                        "damage_type": "fire"
                    },
                    {
                        "formula": "level",
                        "object": {
                            "from": "subevent",
                            "object": "source"
                        },
                        "damage_type": "fire"
                    }
                ]
            }
            """);

        FunctionState.StateData result;

        result = addDamage.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, completed = false }, result);
        Assert.Equal($$"""
            [
              {
                "bonus": {{firstClassLevel}},
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as DamageCollection).GetDamageCollection().PrettyPrint());

        result = addDamage.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, completed = false }, result);
        Assert.Equal($$"""
            [
              {
                "bonus": {{firstClassLevel}},
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              },
              {
                "bonus": {{firstClassLevel + secondClassLevel}},
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as DamageCollection).GetDamageCollection().PrettyPrint());

        result = addDamage.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, completed = true }, result);
        Assert.Equal($$"""
            [
              {
                "bonus": {{firstClassLevel}},
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              },
              {
                "bonus": {{firstClassLevel + secondClassLevel}},
                "damage_type": "fire",
                "dice": [ ],
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
