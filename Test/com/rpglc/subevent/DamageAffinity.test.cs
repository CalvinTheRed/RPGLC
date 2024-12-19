using com.rpglc.core;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class DamageAffinityTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares")]
    public void Prepares() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DamageAffinity damageAffinity = new DamageAffinity()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal("""[]""", damageAffinity.GetAffinities().ToString());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "includes damage type")]
    public void IncludesDamageType() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DamageAffinity damageAffinity = new DamageAffinity()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .AddDamageType("fire");

        Assert.True(damageAffinity.IncludesDamageType("fire"));
        Assert.False(damageAffinity.IncludesDamageType("cold"));
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "adds damage type")]
    public void AddsDamageType() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DamageAffinity damageAffinity = new DamageAffinity()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .AddDamageType("fire")
            .AddDamageType("fire");

        Assert.Equal("""
            [
              {
                "damage_type": "fire",
                "immunity": false,
                "immunity_revoked": false,
                "resistance": false,
                "resistance_revoked": false,
                "vulnerability": false,
                "vulnerability_revoked": false
              }
            ]
            """,
            damageAffinity.GetAffinities().PrettyPrint());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "grants immunity")]
    public void GrantsImmunity() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DamageAffinity damageAffinity = new DamageAffinity()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .AddDamageType("fire")
            .GrantImmunity("fire");

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
            damageAffinity.GetAffinities().PrettyPrint());
        
        Assert.True(damageAffinity.IsImmune("fire"));
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "revokes immunity")]
    public void RevokesImmunity() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DamageAffinity damageAffinity = new DamageAffinity()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .AddDamageType("fire")
            .GrantImmunity("fire")
            .RevokeImmunity("fire");

        Assert.Equal("""
            [
              {
                "damage_type": "fire",
                "immunity": true,
                "immunity_revoked": true,
                "resistance": false,
                "resistance_revoked": false,
                "vulnerability": false,
                "vulnerability_revoked": false
              }
            ]
            """,
            damageAffinity.GetAffinities().PrettyPrint());

        Assert.False(damageAffinity.IsImmune("fire"));
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "grants resistance")]
    public void GrantsResistance() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DamageAffinity damageAffinity = new DamageAffinity()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .AddDamageType("fire")
            .GrantResistance("fire");

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
            damageAffinity.GetAffinities().PrettyPrint());

        Assert.True(damageAffinity.IsResistant("fire"));
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "revokes resistance")]
    public void RevokesResistance() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DamageAffinity damageAffinity = new DamageAffinity()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .AddDamageType("fire")
            .GrantResistance("fire")
            .RevokeResistance("fire");

        Assert.Equal("""
            [
              {
                "damage_type": "fire",
                "immunity": false,
                "immunity_revoked": false,
                "resistance": true,
                "resistance_revoked": true,
                "vulnerability": false,
                "vulnerability_revoked": false
              }
            ]
            """,
            damageAffinity.GetAffinities().PrettyPrint());

        Assert.False(damageAffinity.IsResistant("fire"));
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "grants vulnerability")]
    public void GrantsVulnerability() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DamageAffinity damageAffinity = new DamageAffinity()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .AddDamageType("fire")
            .GrantVulnerability("fire");

        Assert.Equal("""
            [
              {
                "damage_type": "fire",
                "immunity": false,
                "immunity_revoked": false,
                "resistance": false,
                "resistance_revoked": false,
                "vulnerability": true,
                "vulnerability_revoked": false
              }
            ]
            """,
            damageAffinity.GetAffinities().PrettyPrint());

        Assert.True(damageAffinity.IsVulnerable("fire"));
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "revokes vulnerability")]
    public void RevokesVulnerability() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DamageAffinity damageAffinity = new DamageAffinity()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .AddDamageType("fire")
            .GrantVulnerability("fire")
            .RevokeVulnerability("fire");

        Assert.Equal("""
            [
              {
                "damage_type": "fire",
                "immunity": false,
                "immunity_revoked": false,
                "resistance": false,
                "resistance_revoked": false,
                "vulnerability": true,
                "vulnerability_revoked": true
              }
            ]
            """,
            damageAffinity.GetAffinities().PrettyPrint());

        Assert.False(damageAffinity.IsVulnerable("fire"));
    }

};
