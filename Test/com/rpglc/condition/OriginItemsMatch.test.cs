using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.condition;

[Collection("Serial")]
public class OriginItemsMatchTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "origin items do match")]
    public void OriginItemsDoMatch() {
        RPGLItem rpglItem = RPGLFactory.NewItem("test:dummy");
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = RPGLFactory.NewEffect("test:dummy")
            .SetOriginItem(rpglItem.GetUuid());
        Subevent subevent = new DummySubevent()
            .SetOriginItem(rpglItem.GetUuid());

        OriginItemsMatch condition = new OriginItemsMatch().Clone();

        JsonObject conditionJson = new JsonObject().LoadFromString("""
            {
                "condition": "origin_items_match"
            }
            """);

        ConditionState.StateData result;

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.Equal(new ConditionState.StateData() { conditionDependency = null, subeventDependency = null, stepCompleted = true }, result);

        Assert.True(condition.evaluation);
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "origin items do not match")]
    public void OriginItemsDoNotMatch() {
        RPGLItem rpglItem1 = RPGLFactory.NewItem("test:dummy");
        RPGLItem rpglItem2 = RPGLFactory.NewItem("test:dummy");
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = RPGLFactory.NewEffect("test:dummy")
            .SetOriginItem(rpglItem1.GetUuid());
        Subevent subevent = new DummySubevent()
            .SetOriginItem(rpglItem2.GetUuid());

        OriginItemsMatch condition = new OriginItemsMatch().Clone();

        JsonObject conditionJson = new JsonObject().LoadFromString("""
            {
                "condition": "origin_items_match"
            }
            """);

        ConditionState.StateData result;

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.Equal(new ConditionState.StateData() { conditionDependency = null, subeventDependency = null, stepCompleted = true }, result);

        Assert.False(condition.evaluation);
    }

    [DefaultMock]
    [Fact(DisplayName = "defaults to false")]
    public void DefaultsToFalse() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent();

        OriginItemsMatch condition = new OriginItemsMatch().Clone();

        JsonObject conditionJson = new JsonObject().LoadFromString("""
            {
                "condition": "origin_items_match"
            }
            """);

        ConditionState.StateData result;

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.Equal(new ConditionState.StateData() { conditionDependency = null, subeventDependency = null, stepCompleted = true }, result);

        Assert.False(condition.evaluation);
    }

};
