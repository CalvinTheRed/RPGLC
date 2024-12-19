using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;
using com.rpglc.testutils;

namespace com.rpglc.condition;

[Collection("Serial")]
public class EquippedItemHasTagTest {

    [Fact(DisplayName = "condition mismatch")]
    public void ConditionMismatch() {
        bool result = new EquippedItemHasTag().Evaluate(new(), new DummySubevent(), new JsonObject().LoadFromString("""
            {
                "condition": "not-a-condition"
            }
            """), new DummyContext(), new());

        Assert.False(result);
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "item does have tag")]
    public void ItemDoesHaveTag() {
        RPGLItem rpglItem = RPGLFactory.NewItem("test:dummy");
        rpglItem.AddTag("test_tag");

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.GiveItem(rpglItem.GetUuid());
        rpglObject.EquipItem(rpglItem.GetUuid(), "test_slot");

        bool result = new EquippedItemHasTag().Evaluate(
            new(),
            new DummySubevent().SetSource(rpglObject),
            new JsonObject().LoadFromString("""
                {
                    "condition": "equipped_item_has_tag",
                    "object": {
                        "from": "subevent",
                        "object": "source"
                    },
                    "slot": "test_slot",
                    "tag": "test_tag"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.True(result);
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "item does not have tag")]
    public void ItemDoesNotHaveTag() {
        RPGLItem rpglItem = RPGLFactory.NewItem("test:dummy");

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.GiveItem(rpglItem.GetUuid());
        rpglObject.EquipItem(rpglItem.GetUuid(), "test_slot");

        bool result = new EquippedItemHasTag().Evaluate(
            new(),
            new DummySubevent().SetSource(rpglObject),
            new JsonObject().LoadFromString("""
                {
                    "condition": "equipped_item_has_tag",
                    "object": {
                        "from": "subevent",
                        "object": "source"
                    },
                    "slot": "test_slot",
                    "tag": "test_tag"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.False(result);
    }

};
