using com.rpglc.core;
using com.rpglc.database;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils;
using com.rpglc.testutils.mocks;

namespace com.rpglc.condition;

[AssignDatabase]
[Collection("Serial")]
public class ObjectHasTagTest {

    [Fact(DisplayName = "condition mismatch")]
    public void ConditionMismatch() {
        bool result = new ObjectHasTag().Evaluate(new(), new DummySubevent(), new JsonObject().LoadFromString("""
            {
                "condition": "not-a-condition"
            }
            """), new DummyContext(), new());

        Assert.False(result);
    }

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "object does have tag")]
    public void ObjectDoesHaveTag() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        rpglObject.AddTag("test_tag");
        DBManager.UpdateRPGLObject(rpglObject);

        bool result = new ObjectHasTag().Evaluate(
            new(),
            new DummySubevent().SetSource(rpglObject),
            new JsonObject().LoadFromString("""
                {
                    "condition": "object_has_tag",
                    "object": {
                        "from": "subevent",
                        "object": "source"
                    },
                    "tag": "test_tag"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.True(result);
    }

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "object does not have tag")]
    public void ObjectDoesNotHaveTag() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");

        bool result = new ObjectHasTag().Evaluate(
            new(),
            new DummySubevent().SetSource(rpglObject),
            new JsonObject().LoadFromString("""
                {
                    "condition": "object_has_tag",
                    "object": {
                        "from": "subevent",
                        "object": "source"
                    },
                    "tag": "test_tag"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.False(result);
    }

};
