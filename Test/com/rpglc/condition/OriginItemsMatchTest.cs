using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils;
using com.rpglc.testutils.mocks;

namespace com.rpglc.condition;

[Collection("Serial")]
[AssignDatabase]
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

    [DefaultMock]
    [Fact(DisplayName = "items do match")]
    [ClearDatabaseAfterTest]
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

    [DefaultMock]
    [Fact(DisplayName = "items do not match")]
    [ClearDatabaseAfterTest]
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

};
