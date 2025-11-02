using com.rpglc.core;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class CalculateCriticalHitThresholdTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares")]
    public void Prepares() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        CalculateCriticalHitThreshold calculateCriticalHitThreshold = new CalculateCriticalHitThreshold()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal(20L, calculateCriticalHitThreshold.json.GetLong("critical_hit_threshold"));
    }

};
