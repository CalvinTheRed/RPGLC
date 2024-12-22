using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;
using com.rpglc.testutils;
using com.rpglc.testutils.subevent;

namespace com.rpglc.condition;

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

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "object does have tag")]
    public void ObjectDoesHaveTag() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.AddTag("test_tag");

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

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "object does not have tag")]
    public void ObjectDoesNotHaveTag() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);

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
