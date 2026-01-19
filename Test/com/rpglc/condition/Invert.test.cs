using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.condition;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.condition;

[Collection("Serial")]
[RPGLInitTesting]
public class InvertTest {

    [Fact(DisplayName = "inverts true condition")]
    public void InvertsTrueCondition() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent();

        Invert condition = new Invert().Clone();

        JsonObject conditionJson = new JsonObject().LoadFromString("""
            {
                "condition": "invert",
                "invert": {
                    "condition": "true"
                }
            }
            """);

        ConditionState.StateData result;

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.NotNull(result.conditionDependency);
        Assert.Null(result.subeventDependency);
        Assert.False(result.stepCompleted);
        Assert.True(result.conditionDependency.condition is True);

        result.conditionDependency.condition.evaluation = true;

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.Equal(new() { conditionDependency = null, subeventDependency = null, stepCompleted = true }, result);

        Assert.False(condition.evaluation);
    }

    [Fact(DisplayName = "inverts false condition")]
    public void InvertsFalseCondition() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent();

        Invert condition = new Invert().Clone();

        JsonObject conditionJson = new JsonObject().LoadFromString("""
            {
                "condition": "invert",
                "invert": {
                    "condition": "false"
                }
            }
            """);

        ConditionState.StateData result;

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.NotNull(result.conditionDependency);
        Assert.Null(result.subeventDependency);
        Assert.False(result.stepCompleted);
        Assert.True(result.conditionDependency.condition is False);

        result.conditionDependency.condition.evaluation = false;

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.Equal(new() { conditionDependency = null, subeventDependency = null, stepCompleted = true }, result);

        Assert.True(condition.evaluation);
    }

};
