using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class SuppressEventsTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "suppresses events")]
    public void SuppressesEvents() {
        GetEvents getEvents = new GetEvents()
            .Prepare(new DummyContext(), new())
            .AddEvent("test:dummy");

        new SuppressEvents().Execute(
            new RPGLEffect(),
            getEvents,
            new JsonObject().LoadFromString("""
                {
                    "function": "suppress_events"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.Empty(getEvents.Events());
    }

};
