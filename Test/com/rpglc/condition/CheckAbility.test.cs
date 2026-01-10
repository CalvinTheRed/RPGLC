using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.condition;

[Collection("Serial")]
public class CheckAbilityTest {

    [Fact(DisplayName = "ability does match")]
    public void ConditionDoesMatch() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent();

        CheckAbility condition = new CheckAbility().Clone();

        JsonObject conditionJson = new JsonObject().LoadFromString("""
            {
                "condition": "check_ability",
                "ability": "str"
            }
            """);

        ConditionState.StateData result;

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.Equal(new() { conditionDependency = null, subeventDependency = null, stepCompleted = true }, result);

        Assert.True(condition.evaluation);
    }

    [Fact(DisplayName = "ability does not match")]
    public void ConditionDoesNotMatch() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent();

        CheckAbility condition = new CheckAbility().Clone();

        JsonObject conditionJson = new JsonObject().LoadFromString("""
            {
                "condition": "check_ability",
                "ability": "dex"
            }
            """);

        ConditionState.StateData result;

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.Equal(new() { conditionDependency = null, subeventDependency = null, stepCompleted = true }, result);

        Assert.False(condition.evaluation);
    }

};
