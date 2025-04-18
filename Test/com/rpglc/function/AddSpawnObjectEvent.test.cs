using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class AddSpawnObjectEventTest {

    [Fact(DisplayName = "adds event")]
    public void AddsEvent() {
        SpawnObject spawnObject = new SpawnObject()
            .Prepare(new DummyContext(), new());

        new AddSpawnObjectEvent().Execute(
            new RPGLEffect(),
            spawnObject,
            new JsonObject().LoadFromString("""
                {
                    "function": "add_spawn_object_event",
                    "event": "test:dummy"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.Equal(
            """
            [
              "test:dummy"
            ]
            """,
            spawnObject.json.GetJsonArray("extra_events").PrettyPrint()
        );
    }

};
