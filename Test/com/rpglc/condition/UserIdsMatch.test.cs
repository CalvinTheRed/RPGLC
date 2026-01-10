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
public class UserIdsMatchTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "user ids do match")]
    public void UserIdsDoMatch() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext().Add(rpglObject);
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent()
            .SetSource(rpglObject);

        UserIdsMatch condition = new UserIdsMatch().Clone();

        JsonObject conditionJson = new JsonObject().LoadFromString("""
            {
                "condition": "user_ids_match",
                "objects": [
                    {
                        "from": "subevent",
                        "object": "source"
                    },
                    {
                        "from": "subevent",
                        "object": "source"
                    }
                ]
            }
            """);

        ConditionState.StateData result;

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.Equal(new ConditionState.StateData() { conditionDependency = null, subeventDependency = null, stepCompleted = true }, result);

        Assert.True(condition.evaluation);
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "user ids do not match")]
    public void UserIdsDoNotMatch() {
        RPGLObject rpglObject1 = RPGLFactory.NewObject("test:dummy", "user-1");
        RPGLObject rpglObject2 = RPGLFactory.NewObject("test:dummy", "user-2");
        RPGLContext context = new DummyContext()
            .Add(rpglObject1)
            .Add(rpglObject2);
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent()
            .SetSource(rpglObject1)
            .SetTarget(rpglObject2);

        UserIdsMatch condition = new UserIdsMatch().Clone();

        JsonObject conditionJson = new JsonObject().LoadFromString("""
            {
                "condition": "user_ids_match",
                "objects": [
                    {
                        "from": "subevent",
                        "object": "source"
                    },
                    {
                        "from": "subevent",
                        "object": "target"
                    }
                ]
            }
            """);

        ConditionState.StateData result;

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.Equal(new ConditionState.StateData() { conditionDependency = null, subeventDependency = null, stepCompleted = true }, result);

        Assert.False(condition.evaluation);
    }

};
