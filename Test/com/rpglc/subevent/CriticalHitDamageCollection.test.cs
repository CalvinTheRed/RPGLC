using com.rpglc.core;
using com.rpglc.runtime;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class CriticalHitDamageCollectionTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "defaults")]
    public void Defaults() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new CriticalHitDamageCollection());

        // skip over inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Empty((subevent.subevent as CriticalHitDamageCollection).GetDamageCollection().AsList());
    }

};
