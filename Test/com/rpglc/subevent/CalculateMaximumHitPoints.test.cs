using com.rpglc.core;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class CalculateMaximumHitPointsTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares")]
    public void Prepares() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        CalculateMaximumHitPoints calculateMaximumHitPoints = new CalculateMaximumHitPoints()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal(1000L, calculateMaximumHitPoints.Get());
    }

};
