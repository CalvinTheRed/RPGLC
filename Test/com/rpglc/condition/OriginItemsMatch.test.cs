using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.condition;

[AssignDatabase]
[Collection("Serial")]
public class OriginItemsMatchTest {

    [Fact(DisplayName = "condition mismatch")]
    public void ConditionMismatch() {
        bool result = new OriginItemsMatch().Evaluate(new(), new DummySubevent(), new JsonObject().LoadFromString("""
            {
                "condition": "not-a-condition"
            }
            """), new DummyContext(), new());

        Assert.False(result);
    }

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "items do match")]
    public void ItemsDoMatch() {
        RPGLItem rpglItem = RPGLFactory.NewItem("test:dummy");

        bool result = new OriginItemsMatch().Evaluate(
            new RPGLEffect().SetOriginItem(rpglItem.GetUuid()),
            new DummySubevent().SetOriginItem(rpglItem.GetUuid()),
            new JsonObject().LoadFromString("""
                {
                    "condition": "origin_items_match"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.True(result);
    }

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "items do not match")]
    public void ItemsDoNotMatch() {
        RPGLItem effectItem = RPGLFactory.NewItem("test:dummy");
        RPGLItem subeventItem = RPGLFactory.NewItem("test:dummy");

        bool result = new OriginItemsMatch().Evaluate(
            new RPGLEffect().SetOriginItem(effectItem.GetUuid()),
            new DummySubevent().SetOriginItem(subeventItem.GetUuid()),
            new JsonObject().LoadFromString("""
                {
                    "condition": "origin_items_match"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.False(result);
    }

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "null items do not match")]
    public void NullItemsDoNotMatch() {
        bool result = new OriginItemsMatch().Evaluate(
            new RPGLEffect().SetOriginItem(null),
            new DummySubevent().SetOriginItem(null),
            new JsonObject().LoadFromString("""
                {
                    "condition": "origin_items_match"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.False(result);
    }

};
