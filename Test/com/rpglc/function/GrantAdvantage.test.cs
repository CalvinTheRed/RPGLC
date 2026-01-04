using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class GrantAdvantageTest {

    [Fact(DisplayName = "sets crit on hit")]
    public void SetsCritOnHit() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new AttackRoll();

        GrantAdvantage grantAdvantage = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "grant_advantage"
            }
            """);

        FunctionState.StateData result;

        result = grantAdvantage.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.True((subevent as AttackRoll).json.GetBool("has_advantage"));
    }

};
