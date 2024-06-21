using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils;
using com.rpglc.testutils.mocks;

namespace com.rpglc.condition;

[AssignDatabase]
[Collection("Serial")]
public class UserIdsMatchTest {

    [Fact(DisplayName = "condition mismatch")]
    public void ConditionMismatch() {
        bool result = new UserIdsMatch().Evaluate(new(), new DummySubevent(), new JsonObject().LoadFromString("""
            {
                "condition": "not-a-condition"
            }
            """), new DummyContext(), new());

        Assert.False(result);
    }

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "user ids do match")]
    public void ObjectsDoMatch() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");

        bool result = new UserIdsMatch().Evaluate(
            new RPGLEffect().SetSource(rpglObject.GetUuid()),
            new DummySubevent().SetSource(rpglObject),
            new JsonObject().LoadFromString("""
                {
                    "condition": "user_ids_match",
                    "effect": "source",
                    "subevent": "source"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.True(result);
    }

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "user ids do not match")]
    public void ObjectsDoNotMatch() {
        RPGLObject effectObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        RPGLObject subeventObjejct = RPGLFactory.NewObject("test:dummy", "Player 2");

        bool result = new UserIdsMatch().Evaluate(
            new RPGLEffect().SetSource(effectObject.GetUuid()),
            new DummySubevent().SetSource(subeventObjejct),
            new JsonObject().LoadFromString("""
                {
                    "condition": "user_ids_match",
                    "effect": "source",
                    "subevent": "source"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.False(result);
    }

};
