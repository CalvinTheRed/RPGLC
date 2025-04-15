using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;
using com.rpglc.testutils;
using com.rpglc.testutils.subevent;

namespace com.rpglc.condition;

[Collection("Serial")]
public class ObjectsMatchTest {

    [Fact(DisplayName = "condition mismatch")]
    public void ConditionMismatch() {
        bool result = new ObjectsMatch().Evaluate(new(), new DummySubevent(), new JsonObject().LoadFromString("""
            {
                "condition": "not-a-condition"
            }
            """), new DummyContext(), new());

        Assert.False(result);
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "objects do match")]
    public void ObjectsDoMatch() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);

        bool result = new ObjectsMatch().Evaluate(
            new RPGLEffect().SetSource(rpglObject.GetUuid()),
            new DummySubevent().SetSource(rpglObject),
            new JsonObject().LoadFromString("""
                {
                    "condition": "objects_match",
                    "objects": [
                        {
                            "from": "effect",
                            "object": "source",
                            "as_origin": false
                        },
                        {
                            "from": "subevent",
                            "object": "source",
                            "as_origin": false
                        }
                    ]
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.True(result);
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "objects do not match")]
    public void ObjectsDoNotMatch() {
        RPGLObject effectObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLObject subeventObjejct = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);

        bool result = new ObjectsMatch().Evaluate(
            new RPGLEffect().SetSource(effectObject.GetUuid()),
            new DummySubevent().SetSource(subeventObjejct),
            new JsonObject().LoadFromString("""
                {
                    "condition": "objects_match",
                    "objects": [
                        {
                            "from": "effect",
                            "object": "source",
                            "as_origin": false
                        },
                        {
                            "from": "subevent",
                            "object": "source",
                            "as_origin": false
                        }
                    ]
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.False(result);
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "defaults to false")]
    public void DefaultsToFalse() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);

        bool result = new ObjectsMatch().Evaluate(
            new RPGLEffect().SetSource(rpglObject.GetUuid()),
            new DummySubevent().SetSource(rpglObject),
            new JsonObject().LoadFromString("""
                {
                    "condition": "objects_match",
                    "objects": [ ]
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.False(result);
    }

};
