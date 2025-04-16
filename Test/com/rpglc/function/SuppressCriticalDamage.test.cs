using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class SuppressCriticalDamageTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "suppresses critical damage")]
    public void SuppressesCriticalDamage() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        CriticalDamageConfirmation criticalDamageConfirmation = new CriticalDamageConfirmation()
            .SetSource(rpglObject)
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
