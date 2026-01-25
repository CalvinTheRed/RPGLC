using com.rpglc.core;
using com.rpglc.runtime;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class CriticalDamageConfirmationTest {

    [Fact(DisplayName = "defaults")]
    public void Defaults() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new CriticalDamageConfirmation());

        // skip over inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.True((subevent.subevent as CriticalDamageConfirmation).DealsCriticalDamage());
    }

    [Fact(DisplayName = "suppresses critical damage")]
    public void SuppressesCriticalDamage() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new CriticalDamageConfirmation());

        // skip over inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);

        (subevent.subevent as CriticalDamageConfirmation).SuppressCriticalDamage();
        Assert.False((subevent.subevent as CriticalDamageConfirmation).DealsCriticalDamage());
    }

};
