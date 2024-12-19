using com.rpglc.core;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class CalculateCriticalHitThreshholdTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares")]
    public void Prepares() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        CalculateCriticalHitThreshhold calculateCriticalHitThreshhold = new CalculateCriticalHitThreshhold()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal(20L, calculateCriticalHitThreshhold.Get());
    }

};
