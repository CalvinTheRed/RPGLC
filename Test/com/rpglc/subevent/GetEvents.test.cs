using com.rpglc.core;
using com.rpglc.runtime;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class GetEventsTest {

    [Fact(DisplayName = "defaults")]
    public void Defaults() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new GetEvents());

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Empty((subevent.subevent as GetEvents).Events());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "adds event")]
    public void AddsEvent() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new GetEvents());

        var result = subevent.Advance(context);
        (subevent.subevent as GetEvents).AddEvent("test:dummy");
        Assert.Equal((null, true), result);
        List<RPGLEvent> events = (subevent.subevent as GetEvents).Events();
        Assert.Single(events);
        Assert.Equal("test:dummy", events[0].GetDatapackId());
    }

};
