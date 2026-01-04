using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class AddSpawnObjectEffectTest {

    [DefaultMock]
    [Fact(DisplayName = "adds effect")]
    public void AddsEffect() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new SpawnObject().JoinSubeventData(new JsonObject().LoadFromString(
            """{ "object_effects": [ ] }"""
        ));

        AddSpawnObjectEffect addSpawnObjectEffect = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "add_spawn_object_effect",
                "effect": "test:dummy"
            }
            """);

        FunctionState.StateData result;

        result = addSpawnObjectEffect.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, completed = true }, result);
        JsonArray effects = (subevent as SpawnObject).GetObjectEffects();
        Assert.Equal("""["test:dummy"]""", effects.ToString());
    }

};
