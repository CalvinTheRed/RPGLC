using com.rpglc.core;
using com.rpglc.database;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.condition;

[AssignDatabase]
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

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "item does have tag")]
    public void ItemDoesHaveTag() {
        RPGLItem rpglItem = RPGLFactory.NewItem("test:dummy");
        rpglItem.AddTag("test_tag");
        DBManager.UpdateRPGLItem(rpglItem);

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
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

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "item does not have tag")]
    public void ItemDoesNotHaveTag() {
        RPGLItem rpglItem = RPGLFactory.NewItem("test:dummy");

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
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
