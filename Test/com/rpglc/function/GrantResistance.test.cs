using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class GrantResistanceTest {

    [Fact(DisplayName = "grants resistance (default)")]
    public void GrantsResistanceDefault() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DamageAffinity()
            .AddDamageType("fire")
            .AddDamageType("cold");

        GrantResistance function = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "grant_resistance"
            }
            """);

        FunctionState.StateData result;

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.True((subevent as DamageAffinity).json.SeekBool("affinities[0].resistance"));
        Assert.True((subevent as DamageAffinity).json.SeekBool("affinities[1].resistance"));
    }

    [Fact(DisplayName = "grants resistance (customized)")]
    public void GrantsResistanceCustomized() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DamageAffinity()
            .AddDamageType("fire")
            .AddDamageType("cold");

        GrantResistance function = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "grant_resistance",
                "damage_type": "fire"
            }
            """);

        FunctionState.StateData result;

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.True((subevent as DamageAffinity).json.SeekBool("affinities[0].resistance"));
        Assert.False((subevent as DamageAffinity).json.SeekBool("affinities[1].resistance"));
    }

};
