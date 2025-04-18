using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class GrantResistanceTest {

    [Fact(DisplayName = "grants particular resistance")]
    public void GrantsParticularResistance() {
        Subevent subevent = new DamageAffinity()
            .Prepare(new DummyContext(), new())
            .AddDamageType("fire");

        new GrantResistance().Execute(
            new RPGLEffect(),
            subevent,
            new JsonObject().LoadFromString("""
                {
                    "function": "grant_resistance",
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
                "resistance": true,
                "resistance_revoked": false,
                "vulnerability": false,
                "vulnerability_revoked": false
              }
            ]
            """,
            (subevent as DamageAffinity).GetAffinities().PrettyPrint()
        );
    }

    [Fact(DisplayName = "grants blanket resistance")]
    public void GrantsBlanketResistance() {
        Subevent subevent = new DamageAffinity()
            .Prepare(new DummyContext(), new())
            .AddDamageType("fire")
            .AddDamageType("cold");

        new GrantResistance().Execute(
            new RPGLEffect(),
            subevent,
            new JsonObject().LoadFromString("""
                {
                    "function": "grant_resistance"
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
                "resistance": true,
                "resistance_revoked": false,
                "vulnerability": false,
                "vulnerability_revoked": false
              },
              {
                "damage_type": "cold",
                "immunity": false,
                "immunity_revoked": false,
                "resistance": true,
                "resistance_revoked": false,
                "vulnerability": false,
                "vulnerability_revoked": false
              }
            ]
            """,
            (subevent as DamageAffinity).GetAffinities().PrettyPrint()
        );
    }

};
