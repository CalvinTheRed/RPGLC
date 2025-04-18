using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class AddSpawnObjectTagTest {

    [Fact(DisplayName = "adds tag")]
    public void AddsTag() {
        SpawnObject spawnObject = new SpawnObject()
            .Prepare(new DummyContext(), new());

        new AddSpawnObjectTag().Execute(
            new RPGLEffect(),
            spawnObject,
            new JsonObject().LoadFromString("""
                {
                    "function": "add_spawn_object_tag",
                    "tag": "test_tag"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.Equal(
            """
            [
              "test_tag"
            ]
            """,
            spawnObject.json.GetJsonArray("extra_tags").PrettyPrint()
        );
    }

};
