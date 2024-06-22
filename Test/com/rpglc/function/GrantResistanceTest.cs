using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[AssignDatabase]
[Collection("Serial")]
public class GrantResistanceTest {

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "grants particular resistance")]
    public void GrantsParticularResistance() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        Subevent subevent = new DamageAffinity()
            .SetSource(rpglObject)
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

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "grants blanket resistance")]
    public void GrantsBlanketResistance() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        Subevent subevent = new DamageAffinity()
            .SetSource(rpglObject)
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
