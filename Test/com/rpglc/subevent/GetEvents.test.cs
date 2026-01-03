using com.rpglc.core;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class GetEventsTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares")]
    public void Prepares() {
        RPGLObject source = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        GetEvents getEvents = new GetEvents()
            .SetSource(source)
            .Prepare(new DummyContext(), new());

        Assert.Equal("""[]""", getEvents.json.GetJsonArray("events").ToString());
        Assert.False(getEvents.json.GetBool("suppress_events"));
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "returns events")]
    public void ReturnsEvents() {
        RPGLObject source = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        GetEvents getEvents= new GetEvents()
            .SetSource(source)
            .Prepare(new DummyContext(), new())
            .AddEvent("test:dummy");

        Assert.Equal("test:dummy", getEvents.Events().Single().GetDatapackId());
    }
};
