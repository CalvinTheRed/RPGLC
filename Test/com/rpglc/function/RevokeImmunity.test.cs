using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class RevokeImmunityTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "revokes particular immunity")]
    public void RevokesParticularImmunity() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        Subevent subevent = new DamageAffinity()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .AddDamageType("fire");

        new RevokeImmunity().Execute(
            new RPGLEffect(),
            subevent,
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
            (subevent as DamageAffinity).GetAffinities().PrettyPrint()
        );
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "revokes blanket immunity")]
    public void RevokesBlanketImmunity() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        Subevent subevent = new DamageAffinity()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .AddDamageType("fire")
            .AddDamageType("cold");

        new RevokeImmunity().Execute(
            new RPGLEffect(),
            subevent,
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
            (subevent as DamageAffinity).GetAffinities().PrettyPrint()
        );
    }

};
