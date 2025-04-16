using com.rpglc.core;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class CriticalDamageConfirmationTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares")]
    public void PreparesAssignedProficiencyBonus() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        CriticalDamageConfirmation criticalDamageConfirmation = new CriticalDamageConfirmation()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.True(criticalDamageConfirmation.DealsCriticalDamage());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "suppresses critical damage")]
    public void SuppressesCriticalDamage() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        CriticalDamageConfirmation criticalDamageConfirmation = new CriticalDamageConfirmation()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .SuppressCriticalDamage();

        Assert.False(criticalDamageConfirmation.DealsCriticalDamage());
    }

};
