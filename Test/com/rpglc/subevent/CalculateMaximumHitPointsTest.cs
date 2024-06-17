using com.rpglc.core;
using com.rpglc.testutils;
using com.rpglc.testutils.mocks;

namespace com.rpglc.subevent;

[AssignDatabase]
[Collection("Serial")]
public class CalculateMaximumHitPointsTest {

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "prepares")]
    public void Prepares() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        CalculateMaximumHitPoints calculateMaximumHitPoints = new CalculateMaximumHitPoints()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal(1000L, calculateMaximumHitPoints.Get());
    }

};
