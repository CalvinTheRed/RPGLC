using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class RevokeResistanceTest {

    [Fact(DisplayName = "revokes particular resistance")]
    public void RevokesParticularResistance() {
        DamageAffinity damageAffinity = new DamageAffinity()
            .Prepare(new DummyContext(), new())
            .AddDamageType("fire");

        new RevokeResistance().Execute(
            new RPGLEffect(),
            damageAffinity,
            new JsonObject().LoadFromString("""
                {
                    "function": "revoke_resistance",
                    "damage_type": "fire"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.Equal("""
            [
              {
                "damage_type": "fire",
                "immunity": false,
                "immunity_revoked": false,
                "resistance": false,
                "resistance_revoked": true,
                "vulnerability": false,
                "vulnerability_revoked": false
              }
            ]
            """,
            damageAffinity.GetAffinities().PrettyPrint()
        );
    }

    [Fact(DisplayName = "revokes blanket resistance")]
    public void RevokesBlanketResistance() {
        DamageAffinity damageAffinity = new DamageAffinity()
            .Prepare(new DummyContext(), new())
            .AddDamageType("fire")
            .AddDamageType("cold");

        new RevokeResistance().Execute(
            new RPGLEffect(),
            damageAffinity,
            new JsonObject().LoadFromString("""
                {
                    "function": "revoke_resistance"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.Equal("""
            [
              {
                "damage_type": "fire",
                "immunity": false,
                "immunity_revoked": false,
                "resistance": false,
                "resistance_revoked": true,
                "vulnerability": false,
                "vulnerability_revoked": false
              },
              {
                "damage_type": "cold",
                "immunity": false,
                "immunity_revoked": false,
                "resistance": false,
                "resistance_revoked": true,
                "vulnerability": false,
                "vulnerability_revoked": false
              }
            ]
            """,
            damageAffinity.GetAffinities().PrettyPrint()
        );
    }

};
