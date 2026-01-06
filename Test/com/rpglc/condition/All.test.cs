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
public class AllTest {

    [Fact(DisplayName = "evaluates (all true)")]
    public void EvaluatesAllTrue() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent();

        All condition = new All().Clone();

        JsonObject conditionJson = new JsonObject().LoadFromString("""
            {
                "condition": "all",
                "conditions": [
                    {
                        "condition": "true"
                    },
                    {
                        "condition": "true"
                    }
                ]
            }
            """);

        ConditionState.StateData result;

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.Equal(new() { conditionDependency = null, subeventDependency = null, stepCompleted = true }, result);

        result = condition.conditionSteps[1](rpglEffect, subevent, conditionJson, context);
        Assert.NotNull(result.conditionDependency);
        Assert.Null(result.subeventDependency);
        Assert.False(result.stepCompleted);
        Assert.True(result.conditionDependency is True);

        result.conditionDependency.evaluation = true;

        result = condition.conditionSteps[1](rpglEffect, subevent, conditionJson, context);
        Assert.Equal(new() { conditionDependency = null, subeventDependency = null, stepCompleted = false }, result);

        result = condition.conditionSteps[1](rpglEffect, subevent, conditionJson, context);
        Assert.NotNull(result.conditionDependency);
        Assert.Null(result.subeventDependency);
        Assert.False(result.stepCompleted);
        Assert.True(result.conditionDependency is True);

        result.conditionDependency.evaluation = true;

        result = condition.conditionSteps[1](rpglEffect, subevent, conditionJson, context);
        Assert.Equal(new() { conditionDependency = null, subeventDependency = null, stepCompleted = true }, result);

        Assert.True(condition.evaluation);
    }

    [Fact(DisplayName = "evaluates (some true)")]
    public void EvaluatesSomeTrue() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent();

        All condition = new All().Clone();

        JsonObject conditionJson = new JsonObject().LoadFromString("""
            {
                "condition": "all",
                "conditions": [
                    {
                        "condition": "true"
                    },
                    {
                        "condition": "false"
                    }
                ]
            }
            """);

        ConditionState.StateData result;

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.Equal(new() { conditionDependency = null, subeventDependency = null, stepCompleted = true }, result);

        result = condition.conditionSteps[1](rpglEffect, subevent, conditionJson, context);
        Assert.NotNull(result.conditionDependency);
        Assert.Null(result.subeventDependency);
        Assert.False(result.stepCompleted);
        Assert.True(result.conditionDependency is True);

        result.conditionDependency.evaluation = true;

        result = condition.conditionSteps[1](rpglEffect, subevent, conditionJson, context);
        Assert.Equal(new() { conditionDependency = null, subeventDependency = null, stepCompleted = false }, result);

        result = condition.conditionSteps[1](rpglEffect, subevent, conditionJson, context);
        Assert.NotNull(result.conditionDependency);
        Assert.Null(result.subeventDependency);
        Assert.False(result.stepCompleted);
        Assert.True(result.conditionDependency is False);

        result.conditionDependency.evaluation = false;

        result = condition.conditionSteps[1](rpglEffect, subevent, conditionJson, context);
        Assert.Equal(new() { conditionDependency = null, subeventDependency = null, stepCompleted = true }, result);

        Assert.False(condition.evaluation);
    }

    [Fact(DisplayName = "evaluates (none true)")]
    public void EvaluatesNoneTrue() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent();

        All condition = new All().Clone();

        JsonObject conditionJson = new JsonObject().LoadFromString("""
            {
                "condition": "all",
                "conditions": [
                    {
                        "condition": "false"
                    },
                    {
                        "condition": "false"
                    }
                ]
            }
            """);

        ConditionState.StateData result;

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.Equal(new() { conditionDependency = null, subeventDependency = null, stepCompleted = true }, result);

        result = condition.conditionSteps[1](rpglEffect, subevent, conditionJson, context);
        Assert.NotNull(result.conditionDependency);
        Assert.Null(result.subeventDependency);
        Assert.False(result.stepCompleted);
        Assert.True(result.conditionDependency is False);

        result.conditionDependency.evaluation = false;

        result = condition.conditionSteps[1](rpglEffect, subevent, conditionJson, context);
        Assert.Equal(new() { conditionDependency = null, subeventDependency = null, stepCompleted = true }, result);

        Assert.False(condition.evaluation);
    }

};
