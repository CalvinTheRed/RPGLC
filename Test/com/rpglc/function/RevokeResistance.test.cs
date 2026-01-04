using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class RevokeResistanceTest {

    [Fact(DisplayName = "revokes resistance (default)")]
    public void RevokesResistanceDefault() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DamageAffinity()
            .AddDamageType("fire")
            .AddDamageType("cold");

        RevokeResistance revokeResistance = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "revoke_resistance"
            }
            """);

        FunctionState.StateData result;

        result = revokeResistance.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.True((subevent as DamageAffinity).json.SeekBool("affinities[0].resistance_revoked"));
        Assert.True((subevent as DamageAffinity).json.SeekBool("affinities[1].resistance_revoked"));
    }

    [Fact(DisplayName = "revokes resistance (customized)")]
    public void RevokesResistanceCustomized() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DamageAffinity()
            .AddDamageType("fire")
            .AddDamageType("cold");

        RevokeResistance revokeResistance = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "revoke_resistance",
                "damage_type": "fire"
            }
            """);

        FunctionState.StateData result;

        result = revokeResistance.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.True((subevent as DamageAffinity).json.SeekBool("affinities[0].resistance_revoked"));
        Assert.False((subevent as DamageAffinity).json.SeekBool("affinities[1].resistance_revoked"));
    }

};
