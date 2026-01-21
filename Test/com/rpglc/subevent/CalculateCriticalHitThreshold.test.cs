using com.rpglc.core;
using com.rpglc.runtime;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class CalculateCriticalHitThresholdTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "calculates default")]
    public void CalculatesDefault() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new CalculateCriticalHitThreshold());

        // skip over inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);

        Assert.Equal(20L, (subevent.subevent as CalculateCriticalHitThreshold).Get());
    }

};
