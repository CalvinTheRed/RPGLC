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
public class EquippedItemHasTagTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "slot does have tag")]
    public void SlotDoesHaveTag() {
        RPGLItem rpglItem = RPGLFactory.NewItem("test:dummy");
        rpglItem.AddTag("test_tag");
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID)
            .GiveItem(rpglItem.GetUuid())
            .EquipItem(rpglItem.GetUuid(), "mainhand");
        RPGLContext context = new DummyContext().Add(rpglObject);
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent()
            .SetSource(rpglObject);

        EquippedItemHasTag condition = new EquippedItemHasTag().Clone();

        JsonObject conditionJson = new JsonObject().LoadFromString("""
            {
                "condition": "equipped_item_has_tag",
                "object": {
                    "from": "subevent",
                    "object": "source"
                },
                "slot": "mainhand",
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
    [Fact(DisplayName = "slot does not have tag")]
    public void SlotDoesNotHaveTag() {
        RPGLItem rpglItem = RPGLFactory.NewItem("test:dummy");
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID)
            .GiveItem(rpglItem.GetUuid())
            .EquipItem(rpglItem.GetUuid(), "mainhand");
        RPGLContext context = new DummyContext().Add(rpglObject);
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent()
            .SetSource(rpglObject);

        EquippedItemHasTag condition = new EquippedItemHasTag().Clone();

        JsonObject conditionJson = new JsonObject().LoadFromString("""
            {
                "condition": "equipped_item_has_tag",
                "object": {
                    "from": "subevent",
                    "object": "source"
                },
                "slot": "mainhand",
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
    [Fact(DisplayName = "equipment does have tag")]
    public void EquipmentDoesHaveTag() {
        RPGLItem rpglItem = RPGLFactory.NewItem("test:dummy");
        rpglItem.AddTag("test_tag");
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID)
            .GiveItem(rpglItem.GetUuid())
            .EquipItem(rpglItem.GetUuid(), "mainhand");
        RPGLContext context = new DummyContext().Add(rpglObject);
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent()
            .SetSource(rpglObject);

        EquippedItemHasTag condition = new EquippedItemHasTag().Clone();

        JsonObject conditionJson = new JsonObject().LoadFromString("""
            {
                "condition": "equipped_item_has_tag",
                "object": {
                    "from": "subevent",
                    "object": "source"
                },
                "slot": "*",
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
    [Fact(DisplayName = "equipment does not have tag")]
    public void EquipmentDoesNotHaveTag() {
        RPGLItem rpglItem = RPGLFactory.NewItem("test:dummy");
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID)
            .GiveItem(rpglItem.GetUuid())
            .EquipItem(rpglItem.GetUuid(), "mainhand");
        RPGLContext context = new DummyContext().Add(rpglObject);
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent()
            .SetSource(rpglObject);

        EquippedItemHasTag condition = new EquippedItemHasTag().Clone();

        JsonObject conditionJson = new JsonObject().LoadFromString("""
            {
                "condition": "equipped_item_has_tag",
                "object": {
                    "from": "subevent",
                    "object": "source"
                },
                "slot": "*",
                "tag": "test_tag"
            }
            """);

        ConditionState.StateData result;

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.Equal(new ConditionState.StateData() { conditionDependency = null, subeventDependency = null, stepCompleted = true }, result);

        Assert.False(condition.evaluation);
    }

};
