using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class FailSaveTest {

    [Fact(DisplayName = "determines saving throw failure")]
    public void DeterminesSavingThrowFailure() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new SavingThrow();

        FailSave failSave = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "fail_save"
            }
            """);

        FunctionState.StateData result;

        result = failSave.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal("fail", (subevent as SavingThrow).GetDeterminedResolution());
    }

};
