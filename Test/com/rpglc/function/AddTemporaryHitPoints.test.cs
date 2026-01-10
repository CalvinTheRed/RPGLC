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
public class AddTemporaryHitPointsTest {

    [Fact(DisplayName = "adds temporary hit points (number)")]
    public void AddsTemporaryHitPointsNumber() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new TemporaryHitPointCollection().JoinSubeventData(new JsonObject().LoadFromString(
            """{ "temporary_hit_points": [ ] }"""
        ));

        AddTemporaryHitPoints function = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "add_temporary_hit_points",
                "bonus": [
                    {
                        "formula": "number",
                        "number": 1
                    },
                    {
                        "formula": "number",
                        "number": 2
                    }
                ]
            }
            """);

        FunctionState.StateData result;

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = false }, result);
        Assert.Equal("""
            [
              {
                "bonus": 1,
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as TemporaryHitPointCollection).GetTemporaryHitPointCollection().PrettyPrint());

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal("""
            [
              {
                "bonus": 1,
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              },
              {
                "bonus": 2,
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as TemporaryHitPointCollection).GetTemporaryHitPointCollection().PrettyPrint());
    }

    [DieTestingMode]
    [Fact(DisplayName = "adds temporary hit points (dice)")]
    public void AddsTemporaryHitPointsDice() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new TemporaryHitPointCollection().JoinSubeventData(new JsonObject().LoadFromString(
            """{ "temporary_hit_points": [ ] }"""
        ));

        AddTemporaryHitPoints function = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "add_temporary_hit_points",
                "bonus": [
                    {
                        "formula": "dice",
                        "dice": [
                            { "count": 1, "size": 6, "determined": [ 1 ] }
                        ]
                    },
                    {
                        "formula": "dice",
                        "dice": [
                            { "count": 1, "size": 6, "determined": [ 2 ] }
                        ]
                    }
                ]
            }
            """);

        FunctionState.StateData result;

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
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
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as TemporaryHitPointCollection).GetTemporaryHitPointCollection().PrettyPrint());

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
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
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as TemporaryHitPointCollection).GetTemporaryHitPointCollection().PrettyPrint());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "adds temporary hit points (modifier)")]
    public void AddsTemporaryHitPointsModifier() {
        long strScore = 12L;
        long dexScore = 14L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.GetAbilityScores().PutLong("str", strScore);
        rpglObject.GetAbilityScores().PutLong("dex", dexScore);
        RPGLContext context = new DummyContext().Add(rpglObject);
        RPGLEffect rpglEffect = new();
        Subevent subevent = new TemporaryHitPointCollection().JoinSubeventData(new JsonObject().LoadFromString(
            """{ "temporary_hit_points": [ ] }"""
        )).SetSource(rpglObject);

        AddTemporaryHitPoints function = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "add_temporary_hit_points",
                "bonus": [
                    {
                        "formula": "modifier",
                        "object": {
                            "from": "subevent",
                            "object": "source"
                        },
                        "ability": "str"
                    },
                    {
                        "formula": "modifier",
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

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.NotNull(result.dependency);
        Assert.False(result.stepCompleted);
        Assert.Equal("[ ]", (subevent as TemporaryHitPointCollection).GetTemporaryHitPointCollection().PrettyPrint());
        Assert.True(result.dependency is CalculateAbilityScore);

        (result.dependency as CalculateAbilityScore)
            .SetMinimum(long.MinValue)
            .SetBase(strScore)
            .JoinSubeventData(new JsonObject().LoadFromString("""{ "bonuses": [ ] }"""));

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = false }, result);
        Assert.Equal("""
            [
              {
                "bonus": 1,
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as TemporaryHitPointCollection).GetTemporaryHitPointCollection().PrettyPrint());

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.NotNull(result.dependency);
        Assert.False(result.stepCompleted);
        Assert.Equal("""
            [
              {
                "bonus": 1,
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as TemporaryHitPointCollection).GetTemporaryHitPointCollection().PrettyPrint());
        Assert.True(result.dependency is CalculateAbilityScore);

        (result.dependency as CalculateAbilityScore)
            .SetMinimum(long.MinValue)
            .SetBase(dexScore)
            .JoinSubeventData(new JsonObject().LoadFromString("""{ "bonuses": [ ] }"""));

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal("""
            [
              {
                "bonus": 1,
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              },
              {
                "bonus": 2,
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as TemporaryHitPointCollection).GetTemporaryHitPointCollection().PrettyPrint());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "adds temporary hit points (ability)")]
    public void AddsTemporaryHitPointsAbility() {
        long strScore = 12L;
        long dexScore = 14L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.GetAbilityScores().PutLong("str", strScore);
        rpglObject.GetAbilityScores().PutLong("dex", dexScore);
        RPGLContext context = new DummyContext().Add(rpglObject);
        RPGLEffect rpglEffect = new();
        Subevent subevent = new TemporaryHitPointCollection().JoinSubeventData(new JsonObject().LoadFromString(
            """{ "temporary_hit_points": [ ] }"""
        )).SetSource(rpglObject);

        AddTemporaryHitPoints function = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "add_temporary_hit_points",
                "bonus": [
                    {
                        "formula": "ability",
                        "object": {
                            "from": "subevent",
                            "object": "source"
                        },
                        "ability": "str"
                    },
                    {
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

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.NotNull(result.dependency);
        Assert.False(result.stepCompleted);
        Assert.Equal("[ ]", (subevent as TemporaryHitPointCollection).GetTemporaryHitPointCollection().PrettyPrint());
        Assert.True(result.dependency is CalculateAbilityScore);

        (result.dependency as CalculateAbilityScore)
            .SetMinimum(long.MinValue)
            .SetBase(strScore)
            .JoinSubeventData(new JsonObject().LoadFromString("""{ "bonuses": [ ] }"""));

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = false }, result);
        Assert.Equal("""
            [
              {
                "bonus": 12,
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as TemporaryHitPointCollection).GetTemporaryHitPointCollection().PrettyPrint());

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.NotNull(result.dependency);
        Assert.False(result.stepCompleted);
        Assert.Equal("""
            [
              {
                "bonus": 12,
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as TemporaryHitPointCollection).GetTemporaryHitPointCollection().PrettyPrint());
        Assert.True(result.dependency is CalculateAbilityScore);

        (result.dependency as CalculateAbilityScore)
            .SetMinimum(long.MinValue)
            .SetBase(dexScore)
            .JoinSubeventData(new JsonObject().LoadFromString("""{ "bonuses": [ ] }"""));

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal("""
            [
              {
                "bonus": 12,
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              },
              {
                "bonus": 14,
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as TemporaryHitPointCollection).GetTemporaryHitPointCollection().PrettyPrint());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "adds temporary hit points (proficiency)")]
    public void AddsTemporaryHitPointsProficiency() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext().Add(rpglObject);
        RPGLEffect rpglEffect = new();
        Subevent subevent = new TemporaryHitPointCollection().JoinSubeventData(new JsonObject().LoadFromString(
            """{ "temporary_hit_points": [ ] }"""
        )).SetSource(rpglObject);

        AddTemporaryHitPoints function = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "add_temporary_hit_points",
                "bonus": [
                    {
                        "formula": "proficiency",
                        "object": {
                            "from": "subevent",
                            "object": "source"
                        }
                    },
                    {
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

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.NotNull(result.dependency);
        Assert.False(result.stepCompleted);
        Assert.Equal("[ ]", (subevent as TemporaryHitPointCollection).GetTemporaryHitPointCollection().PrettyPrint());
        Assert.True(result.dependency is CalculateProficiencyBonus);

        (result.dependency as CalculateProficiencyBonus)
            .SetMinimum(long.MinValue)
            .SetBase(2)
            .JoinSubeventData(new JsonObject().LoadFromString("""{ "bonuses": [ ] }"""));

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = false }, result);
        Assert.Equal("""
            [
              {
                "bonus": 2,
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as TemporaryHitPointCollection).GetTemporaryHitPointCollection().PrettyPrint());

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.NotNull(result.dependency);
        Assert.False(result.stepCompleted);
        Assert.Equal("""
            [
              {
                "bonus": 2,
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as TemporaryHitPointCollection).GetTemporaryHitPointCollection().PrettyPrint());
        Assert.True(result.dependency is CalculateProficiencyBonus);

        (result.dependency as CalculateProficiencyBonus)
            .SetMinimum(long.MinValue)
            .SetBase(2)
            .JoinSubeventData(new JsonObject().LoadFromString("""{ "bonuses": [ ] }"""));

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal("""
            [
              {
                "bonus": 2,
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              },
              {
                "bonus": 2,
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as TemporaryHitPointCollection).GetTemporaryHitPointCollection().PrettyPrint());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [ExtraClassesMock]
    [Fact(DisplayName = "adds temporary hit points (level)")]
    public void AddsTemporaryHitPointsLevel() {
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
        Subevent subevent = new TemporaryHitPointCollection().JoinSubeventData(new JsonObject().LoadFromString(
            """{ "temporary_hit_points": [ ] }"""
        )).SetSource(rpglObject);

        AddTemporaryHitPoints function = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "add_temporary_hit_points",
                "bonus": [
                    {
                        "formula": "level",
                        "object": {
                            "from": "subevent",
                            "object": "source"
                        },
                        "class": "test:dummy"
                    },
                    {
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

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = false }, result);
        Assert.Equal($$"""
            [
              {
                "bonus": {{firstClassLevel}},
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as TemporaryHitPointCollection).GetTemporaryHitPointCollection().PrettyPrint());

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal($$"""
            [
              {
                "bonus": {{firstClassLevel}},
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              },
              {
                "bonus": {{firstClassLevel + secondClassLevel}},
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as TemporaryHitPointCollection).GetTemporaryHitPointCollection().PrettyPrint());
    }

};
