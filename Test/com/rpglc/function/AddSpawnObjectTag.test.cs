using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class AddSpawnObjectTagTest {

    [Fact(DisplayName = "adds event")]
    public void AddsEvent() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new SpawnObject().JoinSubeventData(new JsonObject().LoadFromString(
            """{ "object_tags": [ ] }"""
        ));

        AddSpawnObjectTag addSpawnObjectTag = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "add_spawn_object_tag",
                "tag": "test_tag"
            }
            """);

        FunctionState.StateData result;

        result = addSpawnObjectTag.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, completed = true }, result);
        JsonArray effects = (subevent as SpawnObject).GetObjectTags();
        Assert.Equal("""["test_tag"]""", effects.ToString());
    }

};
