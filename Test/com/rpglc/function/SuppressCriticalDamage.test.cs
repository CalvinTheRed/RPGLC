using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class SuppressCriticalDamageTest {

    [Fact(DisplayName = "suppresses critical damage")]
    public void SuppressesCriticalDamage() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new CriticalDamageConfirmation();

        SuppressCriticalDamage function = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "suppress_critical_damage"
            }
            """);

        FunctionState.StateData result;

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.False((subevent as CriticalDamageConfirmation).DealsCriticalDamage());
    }

};
