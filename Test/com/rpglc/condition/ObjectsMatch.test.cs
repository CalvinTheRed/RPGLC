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
public class ObjectsMatchTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "objects do match")]
    public void ObjectsDoMatch() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext().Add(rpglObject);
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent()
            .SetSource(rpglObject);

        ObjectsMatch condition = new ObjectsMatch().Clone();

        JsonObject conditionJson = new JsonObject().LoadFromString("""
            {
                "condition": "objects_match",
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
    [Fact(DisplayName = "objects do not match")]
    public void ObjectsDoNotMatch() {
        RPGLObject rpglObject1 = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLObject rpglObject2 = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext()
            .Add(rpglObject1)
            .Add(rpglObject2);
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent()
            .SetSource(rpglObject1)
            .SetTarget(rpglObject2);

        ObjectsMatch condition = new ObjectsMatch().Clone();

        JsonObject conditionJson = new JsonObject().LoadFromString("""
            {
                "condition": "objects_match",
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

    [DefaultMock]
    [Fact(DisplayName = "defaults to false")]
    public void DefaultsToFalse() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent();

        ObjectsMatch condition = new ObjectsMatch().Clone();

        JsonObject conditionJson = new JsonObject().LoadFromString("""
            {
                "condition": "objects_match",
                "objects": [ ]
            }
            """);

        ConditionState.StateData result;

        result = condition.conditionSteps[0](rpglEffect, subevent, conditionJson, context);
        Assert.Equal(new ConditionState.StateData() { conditionDependency = null, subeventDependency = null, stepCompleted = true }, result);

        Assert.False(condition.evaluation);
    }

};
