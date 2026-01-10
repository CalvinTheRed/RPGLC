using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.condition;

[Collection("Serial")]
public class SubeventHasTagTest {

    [DefaultMock]
    [Fact(DisplayName = "subevent does have tag")]
    public void SubeventDoesHaveTag() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent()
            .AddTag("test_tag");

        SubeventHasTag condition = new SubeventHasTag().Clone();

        JsonObject conditionJson = new JsonObject().LoadFromString("""
            {
                "condition": "subevent_has_tag",
                "tag": "test_tag"
            }
            """);

        ConditionState.StateData result;

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.Equal(new ConditionState.StateData() { conditionDependency = null, subeventDependency = null, stepCompleted = true }, result);

        Assert.True(condition.evaluation);
    }

    [DefaultMock]
    [Fact(DisplayName = "subevent does not have tag")]
    public void SubeventDoesNotHaveTag() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent();

        SubeventHasTag condition = new SubeventHasTag().Clone();

        JsonObject conditionJson = new JsonObject().LoadFromString("""
            {
                "condition": "subevent_has_tag",
                "tag": "test_tag"
            }
            """);

        ConditionState.StateData result;

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.Equal(new ConditionState.StateData() { conditionDependency = null, subeventDependency = null, stepCompleted = true }, result);

        Assert.False(condition.evaluation);
    }

};
