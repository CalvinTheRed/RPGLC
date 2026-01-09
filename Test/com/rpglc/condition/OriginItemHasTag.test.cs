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
public class OriginItemHasTagTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "origin item does have tag")]
    public void OriginItemDoesHaveTag() {
        RPGLItem rpglItem = RPGLFactory.NewItem("test:dummy").AddTag("test_tag") as RPGLItem;
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent()
            .SetOriginItem(rpglItem.GetUuid());

        OriginItemHasTag condition = new OriginItemHasTag().Clone();

        JsonObject conditionJson = new JsonObject().LoadFromString("""
            {
                "condition": "origin_item_has_tag",
                "origin_item": "subevent",
                "tag": "test_tag"
            }
            """);

        ConditionState.StateData result;

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.Equal(new ConditionState.StateData() { conditionDependency = null, subeventDependency = null, stepCompleted = true }, result);

        Assert.True(condition.evaluation);
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "origin item does not have tag")]
    public void OriginItemDoesNotHaveTag() {
        RPGLItem rpglItem = RPGLFactory.NewItem("test:dummy");
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent()
            .SetOriginItem(rpglItem.GetUuid());

        OriginItemHasTag condition = new OriginItemHasTag().Clone();

        JsonObject conditionJson = new JsonObject().LoadFromString("""
            {
                "condition": "origin_item_has_tag",
                "origin_item": "subevent",
                "tag": "test_tag"
            }
            """);

        ConditionState.StateData result;

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.Equal(new ConditionState.StateData() { conditionDependency = null, subeventDependency = null, stepCompleted = true }, result);

        Assert.False(condition.evaluation);
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "defaults to false")]
    public void DefaultsToFalse() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent();

        OriginItemHasTag condition = new OriginItemHasTag().Clone();

        JsonObject conditionJson = new JsonObject().LoadFromString("""
            {
                "condition": "origin_item_has_tag",
                "origin_item": "subevent",
                "tag": "test_tag"
            }
            """);

        ConditionState.StateData result;

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.Equal(new ConditionState.StateData() { conditionDependency = null, subeventDependency = null, stepCompleted = true }, result);

        Assert.False(condition.evaluation);
    }

};
