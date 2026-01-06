using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class GrantImmunityTest {

    [Fact(DisplayName = "grants immunity (default)")]
    public void GrantsImmunityDefault() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DamageAffinity()
            .AddDamageType("fire")
            .AddDamageType("cold");

        GrantImmunity function = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "grant_immunity"
            }
            """);

        FunctionState.StateData result;

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.True((subevent as DamageAffinity).json.SeekBool("affinities[0].immunity"));
        Assert.True((subevent as DamageAffinity).json.SeekBool("affinities[1].immunity"));
    }

    [Fact(DisplayName = "grants immunity (customized)")]
    public void GrantsImmunityCustomized() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DamageAffinity()
            .AddDamageType("fire")
            .AddDamageType("cold");

        GrantImmunity function = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "grant_immunity",
                "damage_type": "fire"
            }
            """);

        FunctionState.StateData result;

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.True((subevent as DamageAffinity).json.SeekBool("affinities[0].immunity"));
        Assert.False((subevent as DamageAffinity).json.SeekBool("affinities[1].immunity"));
    }

};
