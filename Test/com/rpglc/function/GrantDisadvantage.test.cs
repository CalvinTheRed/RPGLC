using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class GrantDisadvantageTest {

    [Fact(DisplayName = "grants disadvantage")]
    public void GrantsDisadvantage() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new AttackRoll();

        GrantDisadvantage grantDisadvantage = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "grant_disadvantage"
            }
            """);

        FunctionState.StateData result;

        result = grantDisadvantage.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.True((subevent as AttackRoll).json.GetBool("has_disadvantage"));
    }

};
