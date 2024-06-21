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
    [DefaultMock]
    [Fact(DisplayName = "executes functions")]
    [ResetCountersAfterTest]
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
    [DefaultMock]
    [ExtraEffectsMock]
    [Fact(DisplayName = "processes subevent")]
    [ResetCountersAfterTest]
    public void ProcessesSubevent() {
        RPGLEffect rpglEffect = RPGLFactory.NewEffect("test:complex_effect");

        rpglEffect.ProcessSubevent(new DummySubevent(), new DummyContext(), new());

        Assert.Equal(1L, DummyFunction.Counter);
    }

};
