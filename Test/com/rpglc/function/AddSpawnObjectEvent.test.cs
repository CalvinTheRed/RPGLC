using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class AddSpawnObjectEventTest {

    [Fact(DisplayName = "adds event")]
    public void AddsEvent() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new SpawnObject().JoinSubeventData(new JsonObject().LoadFromString(
            """{ "object_events": [ ] }"""
        ));

        AddSpawnObjectEvent addSpawnObjectEvent= new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "add_spawn_object_event",
                "event": "test:dummy"
            }
            """);

        FunctionState.StateData result;

        result = addSpawnObjectEvent.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, completed = true }, result);
        Assert.Equal("""["test:dummy"]""", (subevent as SpawnObject).GetObjectEvents().ToString());
    }

};
