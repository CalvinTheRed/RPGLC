using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.function;

[Collection("Serial")]
public class GrantDisadvantageTest {

    [Fact(DisplayName = "grants disadvantage")]
    public void GrantsDisadvantage() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummyRollSubevent();

        GrantDisadvantage function = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "grant_disadvantage"
            }
            """);

        FunctionState.StateData result;

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.True((subevent as DummyRollSubevent).json.GetBool("has_disadvantage"));
    }

};
