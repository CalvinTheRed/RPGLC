using com.rpglc.function;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils;
using com.rpglc.testutils.mocks;

namespace com.rpglc.core;

[AssignDatabase]
[RPGLCInit]
[Collection("Serial")]
public class RPGLEffectTest {

    [DefaultMock]
    [ClearDatabaseAfterTest]
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

    [DefaultMock]
    [ClearDatabaseAfterTest]
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

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [ResetCountersAfterTest]
    [Fact(DisplayName = "executes functions")]
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

    [DefaultMock]
    [ExtraEffectsMock]
    [ClearDatabaseAfterTest]
    [ResetCountersAfterTest]
    [Fact(DisplayName = "processes subevent")]
    public void ProcessesSubevent() {
        RPGLEffect rpglEffect = RPGLFactory.NewEffect("test:complex_effect");

        rpglEffect.ProcessSubevent(new DummySubevent(), new DummyContext(), new());

        Assert.Equal(1L, DummyFunction.Counter);
    }

};
