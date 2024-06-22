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
public class OriginItemHasTagTest {

    [Fact(DisplayName = "condition mismatch")]
    public void ConditionMismatch() {
        bool result = new OriginItemHasTag().Evaluate(new(), new DummySubevent(), new JsonObject().LoadFromString("""
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

        bool result = new OriginItemHasTag().Evaluate(
            new(),
            new DummySubevent()
                .SetOriginItem(rpglItem.GetUuid())
                .SetSource(rpglObject),
            new JsonObject().LoadFromString("""
                {
                    "condition": "origin_item_has_tag",
                    "origin_item": "subevent",
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

        bool result = new OriginItemHasTag().Evaluate(
            new(),
            new DummySubevent()
                .SetOriginItem(rpglItem.GetUuid())
                .SetSource(rpglObject),
            new JsonObject().LoadFromString("""
                {
                    "condition": "origin_item_has_tag",
                    "origin_item": "subevent",
                    "tag": "test_tag"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.False(result);
    }

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "null item does not have tag")]
    public void NullItemDoesNotHaveTag() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");

        bool result = new OriginItemHasTag().Evaluate(
            new(),
            new DummySubevent()
                .SetOriginItem(null)
                .SetSource(rpglObject),
            new JsonObject().LoadFromString("""
                {
                    "condition": "origin_item_has_tag",
                    "origin_item": "subevent",
                    "tag": "test_tag"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.False(result);
    }

};
