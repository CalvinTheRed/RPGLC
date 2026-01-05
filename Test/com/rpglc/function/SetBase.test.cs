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
public class SetBaseTest {

    [Fact(DisplayName = "sets base (number)")]
    public void SetsBaseNumber() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummyCalculationSubevent();

        SetBase setBase = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "set_base",
                "base": {
                    "formula": "number",
                    "number": 10
                }
            }
            """);

        FunctionState.StateData result;

        result = setBase.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal(10, (subevent as CalculationSubevent).GetBase());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "sets base (modifier)")]
    public void SetsBaseModifier() {
        long strScore = 12L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.GetAbilityScores().PutLong("str", strScore);
        RPGLContext context = new DummyContext().Add(rpglObject);
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummyCalculationSubevent()
            .SetSource(rpglObject);

        SetBase setBase = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "set_base",
                "base": {
                    "formula": "modifier",
                    "object": {
                        "from": "subevent",
                        "object": "source"
                    },
                    "ability": "str"
                }
            }
            """);

        FunctionState.StateData result;

        result = setBase.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.NotNull(result.dependency);
        Assert.False(result.stepCompleted);
        Assert.Equal(0, (subevent as CalculationSubevent).GetBase());
        Assert.True(result.dependency is CalculateAbilityScore);

        (result.dependency as CalculateAbilityScore)
            .SetMinimum(long.MinValue)
            .SetBase(strScore)
            .JoinSubeventData(new JsonObject().LoadFromString("""{ "bonuses": [ ] }"""));

        result = setBase.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal(1, (subevent as CalculationSubevent).GetBase());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "sets base (ability)")]
    public void SetsBaseAbility() {
        long strScore = 12L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.GetAbilityScores().PutLong("str", strScore);
        RPGLContext context = new DummyContext().Add(rpglObject);
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummyCalculationSubevent()
            .SetSource(rpglObject);

        SetBase setBase = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "set_base",
                "base": {
                    "formula": "ability",
                    "object": {
                        "from": "subevent",
                        "object": "source"
                    },
                    "ability": "str"
                }
            }
            """);

        FunctionState.StateData result;

        result = setBase.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.NotNull(result.dependency);
        Assert.False(result.stepCompleted);
        Assert.Equal(0, (subevent as CalculationSubevent).GetBase());
        Assert.True(result.dependency is CalculateAbilityScore);

        (result.dependency as CalculateAbilityScore)
            .SetMinimum(long.MinValue)
            .SetBase(strScore)
            .JoinSubeventData(new JsonObject().LoadFromString("""{ "bonuses": [ ] }"""));

        result = setBase.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal(12, (subevent as CalculationSubevent).GetBase());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "sets base (proficiency)")]
    public void SetsBaseProficiency() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext().Add(rpglObject);
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummyCalculationSubevent()
            .SetSource(rpglObject);

        SetBase setBase = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "set_base",
                "base": {
                    "formula": "proficiency",
                    "object": {
                        "from": "subevent",
                        "object": "source"
                    }
                }
            }
            """);

        FunctionState.StateData result;

        result = setBase.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.NotNull(result.dependency);
        Assert.False(result.stepCompleted);
        Assert.Equal(0, (subevent as CalculationSubevent).GetBase());
        Assert.True(result.dependency is CalculateProficiencyBonus);

        (result.dependency as CalculationSubevent)
            .SetMinimum(long.MinValue)
            .SetBase(2)
            .JoinSubeventData(new JsonObject().LoadFromString("""{ "bonuses": [ ] }"""));

        result = setBase.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal(2, (subevent as CalculationSubevent).GetBase());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "sets base (level)")]
    public void SetsBaseLevel() {
        long firstClassLevel = 1;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.SetClasses(new JsonArray().LoadFromString($$"""
            [
                {
                  "additional_nested_classes": { },
                  "id": "test:dummy",
                  "level": {{firstClassLevel}},
                  "name": "Dummy"
                }
            ]
            """));
        RPGLContext context = new DummyContext().Add(rpglObject);
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummyCalculationSubevent()
            .SetSource(rpglObject);

        SetBase setBase = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "set_base",
                "base": {
                    "formula": "level",
                    "object": {
                        "from": "subevent",
                        "object": "source"
                    },
                    "class": "test:dummy"
                }
            }
            """);

        FunctionState.StateData result;

        result = setBase.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal(firstClassLevel, (subevent as CalculationSubevent).GetBase());
    }

};
