using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class RevokeImmunityTest {

    [Fact(DisplayName = "revokes immunity (default)")]
    public void RevokesImmunityDefault() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DamageAffinity()
            .AddDamageType("fire")
            .AddDamageType("cold");

        RevokeImmunity revokeImmunity = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "revoke_immunity"
            }
            """);

        FunctionState.StateData result;

        result = revokeImmunity.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.True((subevent as DamageAffinity).json.SeekBool("affinities[0].immunity_revoked"));
        Assert.True((subevent as DamageAffinity).json.SeekBool("affinities[1].immunity_revoked"));
    }

    [Fact(DisplayName = "revokes immunity (customized)")]
    public void RevokesImmunityCustomized() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DamageAffinity()
            .AddDamageType("fire")
            .AddDamageType("cold");

        RevokeImmunity revokeImmunity = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "revoke_immunity",
                "damage_type": "fire"
            }
            """);

        FunctionState.StateData result;

        result = revokeImmunity.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.True((subevent as DamageAffinity).json.SeekBool("affinities[0].immunity_revoked"));
        Assert.False((subevent as DamageAffinity).json.SeekBool("affinities[1].immunity_revoked"));
    }

};
