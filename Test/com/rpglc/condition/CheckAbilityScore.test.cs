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
public class CheckAbilityScoreTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "ability does meet requirement")]
    public void AbilityDoesMeetRequirement() {
        long strScore = 12L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.GetAbilityScores().PutLong("str", strScore);
        RPGLContext context = new DummyContext().Add(rpglObject);
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent()
            .SetSource(rpglObject);

        CheckAbilityScore condition = new CheckAbilityScore().Clone();

        JsonObject conditionJson = new JsonObject().LoadFromString($$"""
            {
                "condition": "check_ability_score",
                "object": {
                    "from": "subevent",
                    "object": "source"
                },
                "ability": "str",
                "comparison": "=",
                "compare_to": {{strScore}}
            }
            """);

        ConditionState.StateData result;

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.Null(result.conditionDependency);
        Assert.NotNull(result.subeventDependency);
        Assert.False(result.stepCompleted);
        Assert.True(result.subeventDependency is CalculateAbilityScore);

        (result.subeventDependency as CalculateAbilityScore)
            .SetMinimum(long.MinValue)
            .SetBase(strScore)
            .JoinSubeventData(new JsonObject().LoadFromString("""{ "bonuses": [ ] }"""));

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.Equal(new ConditionState.StateData() { conditionDependency = null, subeventDependency = null, stepCompleted = true }, result);

        Assert.True(condition.evaluation);
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "ability does not meet requirement")]
    public void AbilityDoesNotMeetRequirement() {
        long strScore = 12L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.GetAbilityScores().PutLong("str", strScore);
        RPGLContext context = new DummyContext().Add(rpglObject);
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent()
            .SetSource(rpglObject);

        CheckAbilityScore condition = new CheckAbilityScore().Clone();

        JsonObject conditionJson = new JsonObject().LoadFromString($$"""
            {
                "condition": "check_ability_score",
                "object": {
                    "from": "subevent",
                    "object": "source"
                },
                "ability": "str",
                "comparison": "!=",
                "compare_to": {{strScore}}
            }
            """);

        ConditionState.StateData result;

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.Null(result.conditionDependency);
        Assert.NotNull(result.subeventDependency);
        Assert.False(result.stepCompleted);
        Assert.True(result.subeventDependency is CalculateAbilityScore);

        (result.subeventDependency as CalculateAbilityScore)
            .SetMinimum(long.MinValue)
            .SetBase(strScore)
            .JoinSubeventData(new JsonObject().LoadFromString("""{ "bonuses": [ ] }"""));

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.Equal(new ConditionState.StateData() { conditionDependency = null, subeventDependency = null, stepCompleted = true }, result);

        Assert.False(condition.evaluation);
    }

};
