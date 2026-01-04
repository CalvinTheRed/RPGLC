using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class CritOnHitTest {

    [Fact(DisplayName = "sets crit on hit")]
    public void SetsCritOnHit() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new AttackRoll().JoinSubeventData(new JsonObject().LoadFromString(
            """{ "crit_on_hit": false }"""
        ));

        CritOnHit critOnHit = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "crit_on_hit"
            }
            """);

        FunctionState.StateData result;

        result = critOnHit.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.True((subevent as AttackRoll).GetCritOnHit());
    }

};
