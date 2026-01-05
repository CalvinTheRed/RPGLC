using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class PassSaveTest {

    [Fact(DisplayName = "determines saving throw success")]
    public void DeterminesSavingThrowSuccess() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new SavingThrow();

        PassSave passSave = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "pass_save"
            }
            """);

        FunctionState.StateData result;

        result = passSave.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal("pass", (subevent as SavingThrow).GetDeterminedResolution());
    }

};
