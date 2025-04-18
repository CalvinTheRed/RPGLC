using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class AddSpawnObjectEffectTest {

    [Fact(DisplayName = "adds effect")]
    public void AddsEffect() {
        SpawnObject spawnObject = new SpawnObject()
            .Prepare(new DummyContext(), new());

        new AddSpawnObjectEffect().Execute(
            new RPGLEffect(),
            spawnObject,
            new JsonObject().LoadFromString("""
                {
                    "function": "add_spawn_object_effect",
                    "effect": "test:dummy"
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
            spawnObject.json.GetJsonArray("extra_effects").PrettyPrint()
        );
    }

};
