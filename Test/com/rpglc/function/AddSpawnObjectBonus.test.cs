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
public class AddSpawnObjectBonusTest {

    [Fact(DisplayName = "adds bonus (number)")]
    public void AddsBonusNumber() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new SpawnObject().JoinSubeventData(new JsonObject().LoadFromString(
            """{ "object_bonuses": [ ] }"""
        ));

        AddSpawnObjectBonus addSpawnObjectBonus = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "add_spawn_object_bonus",
                "bonus": [
                    {
                        "field": "field.first",
                        "formula": "number",
                        "number": 1
                    },
                    {
                        "field": "field.second",
                        "formula": "number",
                        "number": 2
                    }
                ]
            }
            """);

        FunctionState.StateData result;

        result = addSpawnObjectBonus.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = false }, result);
        Assert.Equal("""
            [
              {
                "bonus": 1,
                "dice": [ ],
                "field": "field.first",
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as SpawnObject).GetObjectBonuses().PrettyPrint());

        result = addSpawnObjectBonus.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal("""
            [
              {
                "bonus": 1,
                "dice": [ ],
                "field": "field.first",
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              },
              {
                "bonus": 2,
                "dice": [ ],
                "field": "field.second",
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as SpawnObject).GetObjectBonuses().PrettyPrint());
    }

    [Fact(DisplayName = "adds bonus (dice)")]
    public void AddsBonusDice() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new SpawnObject().JoinSubeventData(new JsonObject().LoadFromString(
            """{ "object_bonuses": [ ] }"""
        ));

        AddSpawnObjectBonus addSpawnObjectBonus = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "add_spawn_object_bonus",
                "bonus": [
                    {
                        "field": "field.first",
                        "formula": "dice",
                        "dice": [
                            { "count": 1, "size": 6, "determined": [ 1 ] }
                        ]
                    },
                    {
                        "field": "field.second",
                        "formula": "dice",
                        "dice": [
                            { "count": 1, "size": 6, "determined": [ 2 ] }
                        ]
                    }
                ]
            }
            """);

        FunctionState.StateData result;

        result = addSpawnObjectBonus.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = false }, result);
        Assert.Equal("""
            [
              {
                "bonus": 0,
                "dice": [
                  {
                    "determined": [
                      1
                    ],
                    "size": 6
                  }
                ],
                "field": "field.first",
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as SpawnObject).GetObjectBonuses().PrettyPrint());

        result = addSpawnObjectBonus.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal("""
            [
              {
                "bonus": 0,
                "dice": [
                  {
                    "determined": [
                      1
                    ],
                    "size": 6
                  }
                ],
                "field": "field.first",
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
                      2
                    ],
                    "size": 6
                  }
                ],
                "field": "field.second",
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as SpawnObject).GetObjectBonuses().PrettyPrint());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "adds bonus (modifier)")]
    public void AddsBonusModifier() {
        long strScore = 12L;
        long dexScore = 14L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.GetAbilityScores().PutLong("str", strScore);
        rpglObject.GetAbilityScores().PutLong("dex", dexScore);
        RPGLContext context = new DummyContext().Add(rpglObject);
        RPGLEffect rpglEffect = new();
        Subevent subevent = new SpawnObject().JoinSubeventData(new JsonObject().LoadFromString(
            """{ "object_bonuses": [ ] }"""
        )).SetSource(rpglObject);

        AddSpawnObjectBonus addSpawnObjectBonus = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "add_spawn_object_bonus",
                "bonus": [
                    {
                        "field": "field.first",
                        "formula": "modifier",
                        "object": {
                            "from": "subevent",
                            "object": "source"
                        },
                        "ability": "str",
                    },
                    {
                        "field": "field.second",
                        "formula": "modifier",
                        "object": {
                            "from": "subevent",
                            "object": "source"
                        },
                        "ability": "dex",
                    }
                ]
            }
            """);

        FunctionState.StateData result;

        result = addSpawnObjectBonus.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.NotNull(result.dependency);
        Assert.False(result.stepCompleted);
        Assert.Equal("[ ]", (subevent as SpawnObject).GetObjectBonuses().PrettyPrint());
        Assert.True(result.dependency is CalculateAbilityScore);

        (result.dependency as CalculateAbilityScore)
            .SetMinimum(long.MinValue)
            .SetBase(strScore)
            .JoinSubeventData(new JsonObject().LoadFromString("""{ "bonuses": [ ] }"""));

        result = addSpawnObjectBonus.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = false }, result);
        Assert.Equal("""
            [
              {
                "bonus": 1,
                "dice": [ ],
                "field": "field.first",
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as SpawnObject).GetObjectBonuses().PrettyPrint());

        result = addSpawnObjectBonus.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.NotNull(result.dependency);
        Assert.False(result.stepCompleted);
        Assert.Equal("""
            [
              {
                "bonus": 1,
                "dice": [ ],
                "field": "field.first",
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as SpawnObject).GetObjectBonuses().PrettyPrint());
        Assert.True(result.dependency is CalculateAbilityScore);

        (result.dependency as CalculateAbilityScore)
            .SetMinimum(long.MinValue)
            .SetBase(dexScore)
            .JoinSubeventData(new JsonObject().LoadFromString("""{ "bonuses": [ ] }"""));

        result = addSpawnObjectBonus.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal("""
            [
              {
                "bonus": 1,
                "dice": [ ],
                "field": "field.first",
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              },
              {
                "bonus": 2,
                "dice": [ ],
                "field": "field.second",
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as SpawnObject).GetObjectBonuses().PrettyPrint());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "adds bonus (ability)")]
    public void AddsBonusAbility() {
        long strScore = 12L;
        long dexScore = 14L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.GetAbilityScores().PutLong("str", strScore);
        rpglObject.GetAbilityScores().PutLong("dex", dexScore);
        RPGLContext context = new DummyContext().Add(rpglObject);
        RPGLEffect rpglEffect = new();
        Subevent subevent = new SpawnObject().JoinSubeventData(new JsonObject().LoadFromString(
            """{ "object_bonuses": [ ] }"""
        )).SetSource(rpglObject);

        AddSpawnObjectBonus addSpawnObjectBonus = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "add_spawn_object_bonus",
                "bonus": [
                    {
                        "field": "field.first",
                        "formula": "ability",
                        "object": {
                            "from": "subevent",
                            "object": "source"
                        },
                        "ability": "str"
                    },
                    {
                        "field": "field.second",
                        "formula": "ability",
                        "object": {
                            "from": "subevent",
                            "object": "source"
                        },
                        "ability": "dex"
                    }
                ]
            }
            """);

        FunctionState.StateData result;

        result = addSpawnObjectBonus.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.NotNull(result.dependency);
        Assert.False(result.stepCompleted);
        Assert.Equal("[ ]", (subevent as SpawnObject).GetObjectBonuses().PrettyPrint());
        Assert.True(result.dependency is CalculateAbilityScore);

        (result.dependency as CalculateAbilityScore)
            .SetMinimum(long.MinValue)
            .SetBase(strScore)
            .JoinSubeventData(new JsonObject().LoadFromString("""{ "bonuses": [ ] }"""));

        result = addSpawnObjectBonus.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = false }, result);
        Assert.Equal("""
            [
              {
                "bonus": 12,
                "dice": [ ],
                "field": "field.first",
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as SpawnObject).GetObjectBonuses().PrettyPrint());

        result = addSpawnObjectBonus.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.NotNull(result.dependency);
        Assert.False(result.stepCompleted);
        Assert.Equal("""
            [
              {
                "bonus": 12,
                "dice": [ ],
                "field": "field.first",
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as SpawnObject).GetObjectBonuses().PrettyPrint());
        Assert.True(result.dependency is CalculateAbilityScore);

        (result.dependency as CalculateAbilityScore)
            .SetMinimum(long.MinValue)
            .SetBase(dexScore)
            .JoinSubeventData(new JsonObject().LoadFromString("""{ "bonuses": [ ] }"""));

        result = addSpawnObjectBonus.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal("""
            [
              {
                "bonus": 12,
                "dice": [ ],
                "field": "field.first",
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              },
              {
                "bonus": 14,
                "dice": [ ],
                "field": "field.second",
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as SpawnObject).GetObjectBonuses().PrettyPrint());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "adds bonus (proficiency)")]
    public void AddsBonusProficiency() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext().Add(rpglObject);
        RPGLEffect rpglEffect = new();
        Subevent subevent = new SpawnObject().JoinSubeventData(new JsonObject().LoadFromString(
            """{ "object_bonuses": [ ] }"""
        )).SetSource(rpglObject);

        AddSpawnObjectBonus addSpawnObjectBonus = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "add_spawn_object_bonus",
                "bonus": [
                    {
                        "field": "field.first",
                        "formula": "proficiency",
                        "object": {
                            "from": "subevent",
                            "object": "source"
                        }
                    },
                    {
                        "field": "field.second",
                        "formula": "proficiency",
                        "object": {
                            "from": "subevent",
                            "object": "source"
                        }
                    }
                ]
            }
            """);

        FunctionState.StateData result;

        result = addSpawnObjectBonus.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.NotNull(result.dependency);
        Assert.False(result.stepCompleted);
        Assert.Equal("[ ]", (subevent as SpawnObject).GetObjectBonuses().PrettyPrint());
        Assert.True(result.dependency is CalculateProficiencyBonus);

        (result.dependency as CalculateProficiencyBonus)
            .SetMinimum(long.MinValue)
            .SetBase(2)
            .JoinSubeventData(new JsonObject().LoadFromString("""{ "bonuses": [ ] }"""));

        result = addSpawnObjectBonus.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = false }, result);
        Assert.Equal("""
            [
              {
                "bonus": 2,
                "dice": [ ],
                "field": "field.first",
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as SpawnObject).GetObjectBonuses().PrettyPrint());

        result = addSpawnObjectBonus.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.NotNull(result.dependency);
        Assert.False(result.stepCompleted);
        Assert.Equal("""
            [
              {
                "bonus": 2,
                "dice": [ ],
                "field": "field.first",
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as SpawnObject).GetObjectBonuses().PrettyPrint());
        Assert.True(result.dependency is CalculateProficiencyBonus);

        (result.dependency as CalculateProficiencyBonus)
            .SetMinimum(long.MinValue)
            .SetBase(2)
            .JoinSubeventData(new JsonObject().LoadFromString("""{ "bonuses": [ ] }"""));

        result = addSpawnObjectBonus.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal("""
            [
              {
                "bonus": 2,
                "dice": [ ],
                "field": "field.first",
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              },
              {
                "bonus": 2,
                "dice": [ ],
                "field": "field.second",
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as SpawnObject).GetObjectBonuses().PrettyPrint());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [ExtraClassesMock]
    [Fact(DisplayName = "adds bonus (level)")]
    public void AddsBonusLevel() {
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
        Subevent subevent = new SpawnObject().JoinSubeventData(new JsonObject().LoadFromString(
            """{ "object_bonuses": [ ] }"""
        )).SetSource(rpglObject);

        AddSpawnObjectBonus addSpawnObjectBonus = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "add_damage",
                "bonus": [
                    {
                        "field": "field.first",
                        "formula": "level",
                        "object": {
                            "from": "subevent",
                            "object": "source"
                        },
                        "class": "test:dummy"
                    },
                    {
                        "field": "field.second",
                        "formula": "level",
                        "object": {
                            "from": "subevent",
                            "object": "source"
                        }
                    }
                ]
            }
            """);

        FunctionState.StateData result;

        result = addSpawnObjectBonus.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = false }, result);
        Assert.Equal($$"""
            [
              {
                "bonus": {{firstClassLevel}},
                "dice": [ ],
                "field": "field.first",
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as SpawnObject).GetObjectBonuses().PrettyPrint());

        result = addSpawnObjectBonus.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal($$"""
            [
              {
                "bonus": {{firstClassLevel}},
                "dice": [ ],
                "field": "field.first",
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              },
              {
                "bonus": {{firstClassLevel + secondClassLevel}},
                "dice": [ ],
                "field": "field.second",
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as SpawnObject).GetObjectBonuses().PrettyPrint());
    }

};
