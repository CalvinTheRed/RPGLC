using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class RevokeImmunityTest {

    [Fact(DisplayName = "revokes particular immunity")]
    public void RevokesParticularImmunity() {
        DamageAffinity damageAffinity = new DamageAffinity()
            .Prepare(new DummyContext(), new())
            .AddDamageType("fire");

        new RevokeImmunity().Execute(
            new RPGLEffect(),
            damageAffinity,
            new JsonObject().LoadFromString("""
                {
                    "function": "revoke_immunity",
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
                "immunity_revoked": true,
                "resistance": false,
                "resistance_revoked": false,
                "vulnerability": false,
                "vulnerability_revoked": false
              }
            ]
            """,
            damageAffinity.GetAffinities().PrettyPrint()
        );
    }

    [Fact(DisplayName = "revokes blanket immunity")]
    public void RevokesBlanketImmunity() {
        DamageAffinity damageAffinity = new DamageAffinity()
            .Prepare(new DummyContext(), new())
            .AddDamageType("fire")
            .AddDamageType("cold");

        new RevokeImmunity().Execute(
            new RPGLEffect(),
            damageAffinity,
            new JsonObject().LoadFromString("""
                {
                    "function": "revoke_immunity"
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
                "immunity_revoked": true,
                "resistance": false,
                "resistance_revoked": false,
                "vulnerability": false,
                "vulnerability_revoked": false
              },
              {
                "damage_type": "cold",
                "immunity": false,
                "immunity_revoked": true,
                "resistance": false,
                "resistance_revoked": false,
                "vulnerability": false,
                "vulnerability_revoked": false
              }
            ]
            """,
            damageAffinity.GetAffinities().PrettyPrint()
        );
    }

};
