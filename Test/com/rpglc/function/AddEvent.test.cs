using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class AddEventTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "adds event")]
    public void AddsEvent() {
        GetEvents getEvents = new GetEvents()
            .Prepare(new DummyContext(), new());

        new AddEvent().Execute(
            new RPGLEffect(),
            getEvents,
            new JsonObject().LoadFromString("""
                {
                    "function": "add_event",
                    "event": "test:dummy"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.Equal("test:dummy", getEvents.Events().Single().GetDatapackId());
    }

};
