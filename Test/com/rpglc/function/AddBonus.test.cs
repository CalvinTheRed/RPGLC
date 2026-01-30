using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.function;

[Collection("Serial")]
[DieTestingMode]
public class AddBonusTest {

    [Fact(DisplayName = "adds bonus (number)")]
    public void AddsBonusNumber() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummyCalculationSubevent().JoinSubeventData(new JsonObject().LoadFromString(
            """{ "bonuses": [ ] }"""
        ));

        AddBonus function = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "add_bonus",
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
        Assert.Equal(1, (subevent as CalculationSubevent).GetBonus());

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal(1 + 2, (subevent as CalculationSubevent).GetBonus());
    }

    [Fact(DisplayName = "adds bonus (dice)")]
    public void AddsBonusDice() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummyCalculationSubevent().JoinSubeventData(new JsonObject().LoadFromString(
            """{ "bonuses": [ ] }"""
        ));

        AddBonus function = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "add_bonus",
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
        Assert.Equal(1, (subevent as CalculationSubevent).GetBonus());

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal(1 + 2, (subevent as CalculationSubevent).GetBonus());
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
        Subevent subevent = new DummyCalculationSubevent().JoinSubeventData(new JsonObject().LoadFromString(
            """{ "bonuses": [ ] }"""
        )).SetSource(rpglObject);

        AddBonus function = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "add_bonus",
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
        Assert.Equal(0, (subevent as CalculationSubevent).GetBonus());
        Assert.True(result.dependency is CalculateAbilityScore);

        (result.dependency as CalculateAbilityScore)
            .SetMinimum(long.MinValue)
            .SetBase(strScore)
            .JoinSubeventData(new JsonObject().LoadFromString("""{ "bonuses": [ ] }"""));

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = false }, result);
        Assert.Equal(1, (subevent as CalculationSubevent).GetBonus());

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.NotNull(result.dependency);
        Assert.False(result.stepCompleted);
        Assert.Equal(1, (subevent as CalculationSubevent).GetBonus());
        Assert.True(result.dependency is CalculateAbilityScore);

        (result.dependency as CalculateAbilityScore)
            .SetMinimum(long.MinValue)
            .SetBase(dexScore)
            .JoinSubeventData(new JsonObject().LoadFromString("""{ "bonuses": [ ] }"""));

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal(1 + 2, (subevent as CalculationSubevent).GetBonus());
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
        Subevent subevent = new DummyCalculationSubevent().JoinSubeventData(new JsonObject().LoadFromString(
            """{ "bonuses": [ ] }"""
        )).SetSource(rpglObject);

        AddBonus function = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "add_bonus",
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
        Assert.Equal(0, (subevent as CalculationSubevent).GetBonus());
        Assert.True(result.dependency is CalculateAbilityScore);

        (result.dependency as CalculateAbilityScore)
            .SetMinimum(long.MinValue)
            .SetBase(strScore)
            .JoinSubeventData(new JsonObject().LoadFromString("""{ "bonuses": [ ] }"""));

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = false }, result);
        Assert.Equal(12, (subevent as CalculationSubevent).GetBonus());

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.NotNull(result.dependency);
        Assert.False(result.stepCompleted);
        Assert.Equal(12, (subevent as CalculationSubevent).GetBonus());
        Assert.True(result.dependency is CalculateAbilityScore);

        (result.dependency as CalculateAbilityScore)
            .SetMinimum(long.MinValue)
            .SetBase(dexScore)
            .JoinSubeventData(new JsonObject().LoadFromString("""{ "bonuses": [ ] }"""));

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal(12 + 14, (subevent as CalculationSubevent).GetBonus());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "adds bonus (proficiency)")]
    public void AddsBonusProficiency() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext().Add(rpglObject);
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummyCalculationSubevent().JoinSubeventData(new JsonObject().LoadFromString(
            """{ "bonuses": [ ] }"""
        )).SetSource(rpglObject);

        AddBonus function = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "add_bonus",
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
        Assert.Equal(0, (subevent as CalculationSubevent).GetBonus());
        Assert.True(result.dependency is CalculateProficiencyBonus);

        (result.dependency as CalculationSubevent).JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "base": 2,
                "bonuses": [ ],
                "minimum": 0
            }
            """));

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = false }, result);
        Assert.Equal(2, (subevent as CalculationSubevent).GetBonus());

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.NotNull(result.dependency);
        Assert.False(result.stepCompleted);
        Assert.Equal(2, (subevent as CalculationSubevent).GetBonus());
        Assert.True(result.dependency is CalculateProficiencyBonus);

        (result.dependency as CalculationSubevent)
            .SetMinimum(long.MinValue)
            .SetBase(2)
            .JoinSubeventData(new JsonObject().LoadFromString("""{ "bonuses": [ ] }"""));

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal(2 + 2, (subevent as CalculationSubevent).GetBonus());
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
        Subevent subevent = new DummyCalculationSubevent().JoinSubeventData(new JsonObject().LoadFromString(
            """{ "bonuses": [ ] }"""
        )).SetSource(rpglObject);

        AddBonus function = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "add_bonus",
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
        Assert.Equal(firstClassLevel, (subevent as CalculationSubevent).GetBonus());

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal(firstClassLevel + (firstClassLevel + secondClassLevel), (subevent as CalculationSubevent).GetBonus());
    }

};
