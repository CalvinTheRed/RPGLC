using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.condition;

[Collection("Serial")]
public class CheckSkillTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "ability check does use skill")]
    public void AbilityCheckDoesUseSkill() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new AbilityCheck().JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "skill": "athletics"
            }
            """));

        CheckSkill condition = new CheckSkill().Clone();

        JsonObject conditionJson = new JsonObject().LoadFromString("""
            {
                "condition": "check_skill",
                "skill": "athletics"
            }
            """);

        ConditionState.StateData result;

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.Equal(new ConditionState.StateData() { conditionDependency = null, subeventDependency = null, stepCompleted = true }, result);

        Assert.True(condition.evaluation);
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "ability check does not use skill")]
    public void AbilityCheckDoesNotUseSkill() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new AbilityCheck().JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "skill": "athletics"
            }
            """));

        CheckSkill condition = new CheckSkill().Clone();

        JsonObject conditionJson = new JsonObject().LoadFromString("""
            {
                "condition": "check_skill",
                "skill": "acrobatics"
            }
            """);

        ConditionState.StateData result;

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.Equal(new ConditionState.StateData() { conditionDependency = null, subeventDependency = null, stepCompleted = true }, result);

        Assert.False(condition.evaluation);
    }

};
