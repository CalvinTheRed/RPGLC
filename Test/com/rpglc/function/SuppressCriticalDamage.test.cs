using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class SuppressCriticalDamageTest {

    [Fact(DisplayName = "suppresses critical damage")]
    public void SuppressesCriticalDamage() {
        CriticalDamageConfirmation criticalDamageConfirmation = new CriticalDamageConfirmation()
            .Prepare(new DummyContext(), new());

        new SuppressCriticalDamage().Execute(
            new RPGLEffect(),
            criticalDamageConfirmation,
            new JsonObject().LoadFromString("""
                {
                    "function": "suppress_critical_damage"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.False(criticalDamageConfirmation.DealsCriticalDamage());
    }

};
