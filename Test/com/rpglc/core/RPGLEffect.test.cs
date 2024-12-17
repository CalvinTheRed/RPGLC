using com.rpglc.function;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.core;

[AssignDatabase]
[Collection("Serial")]
[RPGLCInit]
public class RPGLEffectTest {

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "evaluates conditions (true)")]
    public void EvaluatesConditionsTrue() {
        RPGLEffect rpglEffect = RPGLFactory.NewEffect("test:dummy");

        bool evaluation = rpglEffect.EvaluateConditions(new DummySubevent(), new JsonArray().LoadFromString("""
            [
                {
                    "condition": "true"
                }
            ]
            """), new DummyContext(), new());

        Assert.True(evaluation);
    }

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "evaluates conditions (false)")]
    public void EvaluatesConditionsFalse() {
        RPGLEffect rpglEffect = RPGLFactory.NewEffect("test:dummy");

        bool evaluation = rpglEffect.EvaluateConditions(new DummySubevent(), new JsonArray().LoadFromString("""
            [
                {
                    "condition": "false"
                }
            ]
            """), new DummyContext(), new());

        Assert.False(evaluation);
    }

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "executes functions")]
    [DummyCounterManager]
    public void ExecutesFunctions() {
        RPGLEffect rpglEffect = RPGLFactory.NewEffect("test:dummy");

        rpglEffect.ExecuteFunctions(new DummySubevent(), new JsonArray().LoadFromString("""
            [
                {
                    "function": "dummy_function"
                }
            ]
            """), new DummyContext(), new());

        Assert.Equal(1L, DummyFunction.Counter);
    }

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [ExtraEffectsMock]
    [Fact(DisplayName = "processes subevent")]
    [DummyCounterManager]
    public void ProcessesSubevent() {
        RPGLEffect rpglEffect = RPGLFactory.NewEffect("test:complex_effect");

        rpglEffect.ProcessSubevent(new DummySubevent(), new DummyContext(), new());

        Assert.Equal(1L, DummyFunction.Counter);
    }

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "gets effect source object")]
    public void GetsEffectSourceObject() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");

        RPGLEffect rpglEffect = new RPGLEffect().SetSource(rpglObject.GetUuid());
        Subevent subevent = new DummySubevent();

        Assert.Equal(
            rpglObject.GetUuid(),
            RPGLEffect.GetObject(
                rpglEffect,
                subevent,
                new JsonObject().LoadFromString("""
                    {
                        "from": "effect",
                        "object": "source"
                    }
                    """)
            ).GetUuid()
        );
    }

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "gets effect target object")]
    public void GetsEffectTargetObject() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");

        RPGLEffect rpglEffect = new RPGLEffect().SetTarget(rpglObject.GetUuid());
        Subevent subevent = new DummySubevent();

        Assert.Equal(
            rpglObject.GetUuid(),
            RPGLEffect.GetObject(
                rpglEffect,
                subevent,
                new JsonObject().LoadFromString("""
                    {
                        "from": "effect",
                        "object": "target"
                    }
                    """)
            ).GetUuid()
        );
    }

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "gets subevent source object")]
    public void GetsSubeventSourceObject() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");

        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent().SetSource(rpglObject);

        Assert.Equal(
            rpglObject.GetUuid(),
            RPGLEffect.GetObject(
                rpglEffect,
                subevent,
                new JsonObject().LoadFromString("""
                    {
                        "from": "subevent",
                        "object": "source"
                    }
                    """)
            ).GetUuid()
        );
    }

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "gets subevent target object")]
    public void GetsSubeventTargetObject() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");

        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent().SetTarget(rpglObject);

        Assert.Equal(
            rpglObject.GetUuid(),
            RPGLEffect.GetObject(
                rpglEffect,
                subevent,
                new JsonObject().LoadFromString("""
                    {
                        "from": "subevent",
                        "object": "target"
                    }
                    """)
            ).GetUuid()
        );
    }

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "gets origin object")]
    public void GetsOriginObject() {
        RPGLObject originObject = RPGLFactory.NewObject("test:dummy", "Player 1");

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1")
            .SetOriginObject(originObject.GetUuid());

        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent().SetSource(rpglObject);

        Assert.Equal(
            originObject.GetUuid(),
            RPGLEffect.GetObject(
                rpglEffect,
                subevent,
                new JsonObject().LoadFromString("""
                    {
                        "from": "subevent",
                        "object": "source",
                        "as_origin": true
                    }
                    """)
            ).GetUuid()
        );
    }

};
