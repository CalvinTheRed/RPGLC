using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.condition;

[Collection("Serial")]
public class IncludesDamageTypeTest {

    [Fact(DisplayName = "does include damage type")]
    public void DoesIncludeDamageType() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent();

        IncludesDamageType condition = new IncludesDamageType().Clone();

        JsonObject conditionJson = new JsonObject().LoadFromString("""
            {
                "condition": "includes_damage_type",
                "damage_type": "fire"
            }
            """);

        ConditionState.StateData result;

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.Equal(new ConditionState.StateData() { conditionDependency = null, subeventDependency = null, stepCompleted = true }, result);

        Assert.True(condition.evaluation);
    }

    [Fact(DisplayName = "does not include damage type")]
    public void DoesNotIncludeDamageType() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent();

        IncludesDamageType condition = new IncludesDamageType().Clone();

        JsonObject conditionJson = new JsonObject().LoadFromString("""
            {
                "condition": "includes_damage_type",
                "damage_type": "cold"
            }
            """);

        ConditionState.StateData result;

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.Equal(new ConditionState.StateData() { conditionDependency = null, subeventDependency = null, stepCompleted = true }, result);

        Assert.False(condition.evaluation);
    }

};
