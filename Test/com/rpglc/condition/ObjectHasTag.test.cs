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
public class ObjectHasTagTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "object does have tag")]
    public void ObjectDoesHaveTag() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.AddTag("test_tag");
        RPGLContext context = new DummyContext().Add(rpglObject);
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent()
            .SetSource(rpglObject);

        ObjectHasTag condition = new ObjectHasTag().Clone();

        JsonObject conditionJson = new JsonObject().LoadFromString("""
            {
                "condition": "object_has_tag",
                "object": {
                    "from": "subevent",
                    "object": "source"
                },
                "tag": "test_tag"
            }
            """);

        ConditionState.StateData result;

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.Null(result.conditionDependency);
        Assert.NotNull(result.subeventDependency);
        Assert.False(result.stepCompleted);
        Assert.True(result.subeventDependency is GetObjectTags);

        (result.subeventDependency as GetObjectTags)
            .AddObjectTag("test_tag");

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.Equal(new ConditionState.StateData() { conditionDependency = null, subeventDependency = null, stepCompleted = true }, result);

        Assert.True(condition.evaluation);
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "object does not have tag")]
    public void ObjectDoesNotHaveTag() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext().Add(rpglObject);
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent()
            .SetSource(rpglObject);

        ObjectHasTag condition = new ObjectHasTag().Clone();

        JsonObject conditionJson = new JsonObject().LoadFromString("""
            {
                "condition": "object_has_tag",
                "object": {
                    "from": "subevent",
                    "object": "source"
                },
                "tag": "test_tag"
            }
            """);

        ConditionState.StateData result;

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.Null(result.conditionDependency);
        Assert.NotNull(result.subeventDependency);
        Assert.False(result.stepCompleted);
        Assert.True(result.subeventDependency is GetObjectTags);

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.Equal(new ConditionState.StateData() { conditionDependency = null, subeventDependency = null, stepCompleted = true }, result);

        Assert.False(condition.evaluation);
    }

};
