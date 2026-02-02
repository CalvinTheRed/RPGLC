using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.condition;

[Collection("Serial")]
public class CheckSkillTest {

    [Fact(DisplayName = "ability save does use skill")]
    public void AbilitySaveDoesUseSkill() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new AbilitySave().JoinSubeventData(new JsonObject().LoadFromString("""
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

    [Fact(DisplayName = "ability save does not use skill")]
    public void AbilitySaveDoesNotUseSkill() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new AbilitySave().JoinSubeventData(new JsonObject().LoadFromString("""
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
