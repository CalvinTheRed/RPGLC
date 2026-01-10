using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class AddEventTest {

    [DefaultMock]
    [Fact(DisplayName = "adds event")]
    public void AddsEvent() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new GetEvents().JoinSubeventData(new JsonObject().LoadFromString(
            """{ "events": [ ] }"""
        ));

        AddEvent function = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "add_event",
                "event": "test:dummy"
            }
            """);

        FunctionState.StateData result;

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        List<RPGLEvent> events = (subevent as GetEvents).Events();
        Assert.Single(events);
        Assert.Equal("test:dummy", events[0].GetDatapackId());
    }

};
