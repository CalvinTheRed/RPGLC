using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[AssignDatabase]
[Collection("Serial")]
public class GrantImmunityTest {

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "grants particular immunity")]
    public void GrantsParticularImmunity() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        Subevent subevent = new DamageAffinity()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .AddDamageType("fire");

        new GrantImmunity().Execute(
            new RPGLEffect(),
            subevent,
            new JsonObject().LoadFromString("""
                {
                    "function": "grant_immunity",
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
                "immunity": true,
                "immunity_revoked": false,
                "resistance": false,
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
    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "grants blanket immunity")]
    public void GrantsBlanketImmunity() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        Subevent subevent = new DamageAffinity()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .AddDamageType("fire")
            .AddDamageType("cold");

        new GrantImmunity().Execute(
            new RPGLEffect(),
            subevent,
            new JsonObject().LoadFromString("""
                {
                    "function": "grant_immunity"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.Equal("""
            [
              {
                "damage_type": "fire",
                "immunity": true,
                "immunity_revoked": false,
                "resistance": false,
                "resistance_revoked": false,
                "vulnerability": false,
                "vulnerability_revoked": false
              },
              {
                "damage_type": "cold",
                "immunity": true,
                "immunity_revoked": false,
                "resistance": false,
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
