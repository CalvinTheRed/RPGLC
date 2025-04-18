using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class AddSpawnObjectBonusTest {

    [Fact(DisplayName = "adds bonus")]
    public void AddsBonus() {
        SpawnObject spawnObject = new SpawnObject()
            .Prepare(new DummyContext(), new());

        new AddSpawnObjectBonus().Execute(
            new RPGLEffect(),
            spawnObject,
            new JsonObject().LoadFromString("""
                {
                    "function": "add_spawn_object_bonus",
                    "bonus": {
                        "field": "test_field",
                        "bonus": 5
                    }
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.Equal(
            """
            [
              {
                "bonus": 5,
                "field": "test_field"
              }
            ]
            """,
            spawnObject.json.GetJsonArray("object_bonuses").PrettyPrint()
        );
    }

};
