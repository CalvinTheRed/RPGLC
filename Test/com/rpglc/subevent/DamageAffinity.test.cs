using com.rpglc.core;
using com.rpglc.runtime;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class DamageAffinityTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "defaults")]
    public void Defaults() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new DamageAffinity());

        // skip over inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Empty((subevent.subevent as DamageAffinity).GetAffinities().AsList());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "adds damage type")]
    public void AddsDamageType() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new DamageAffinity());

        // skip over inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);

        (subevent.subevent as DamageAffinity).AddDamageType("fire");
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
            """, (subevent.subevent as DamageAffinity).GetAffinities().PrettyPrint());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "does include damage type")]
    public void DoesIncludeDamageType() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new DamageAffinity());

        // skip over inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);

        (subevent.subevent as DamageAffinity).AddDamageType("fire");
        Assert.True((subevent.subevent as DamageAffinity).IncludesDamageType("fire"));
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "does not include damage type")]
    public void DoesNotIncludeDamageType() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new DamageAffinity());

        // skip over inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.False((subevent.subevent as DamageAffinity).IncludesDamageType("fire"));
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "grants immunity")]
    public void GrantsImmunity() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new DamageAffinity());

        // skip over inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);

        (subevent.subevent as DamageAffinity).AddDamageType("fire");
        (subevent.subevent as DamageAffinity).GrantImmunity("fire");
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
            """, (subevent.subevent as DamageAffinity).GetAffinities().PrettyPrint());
        Assert.True((subevent.subevent as DamageAffinity).IsImmune("fire"));
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "revokes immunity")]
    public void RevokesImmunity() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new DamageAffinity());

        // skip over inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);

        (subevent.subevent as DamageAffinity).AddDamageType("fire");
        (subevent.subevent as DamageAffinity).RevokeImmunity("fire");
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
            """, (subevent.subevent as DamageAffinity).GetAffinities().PrettyPrint());

        (subevent.subevent as DamageAffinity).GrantImmunity("fire");
        Assert.False((subevent.subevent as DamageAffinity).IsImmune("fire"));
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "grants resistance")]
    public void GrantsResistance() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new DamageAffinity());

        // skip over inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);

        (subevent.subevent as DamageAffinity).AddDamageType("fire");
        (subevent.subevent as DamageAffinity).GrantResistance("fire");
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
            """, (subevent.subevent as DamageAffinity).GetAffinities().PrettyPrint());
        Assert.True((subevent.subevent as DamageAffinity).IsResistant("fire"));
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "revokes resistance")]
    public void RevokesResistance() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new DamageAffinity());

        // skip over inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);

        (subevent.subevent as DamageAffinity).AddDamageType("fire");
        (subevent.subevent as DamageAffinity).RevokeResistance("fire");
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
            """, (subevent.subevent as DamageAffinity).GetAffinities().PrettyPrint());

        (subevent.subevent as DamageAffinity).GrantResistance("fire");
        Assert.False((subevent.subevent as DamageAffinity).IsResistant("fire"));
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "grants vulnerability")]
    public void GrantsVulnerability() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new DamageAffinity());

        // skip over inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);

        (subevent.subevent as DamageAffinity).AddDamageType("fire");
        (subevent.subevent as DamageAffinity).GrantVulnerability("fire");
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
            """, (subevent.subevent as DamageAffinity).GetAffinities().PrettyPrint());
        Assert.True((subevent.subevent as DamageAffinity).IsVulnerable("fire"));
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "revokes vulnerability")]
    public void RevokesVulnerability() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new DamageAffinity());

        // skip over inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);

        (subevent.subevent as DamageAffinity).AddDamageType("fire");
        (subevent.subevent as DamageAffinity).RevokeVulnerability("fire");
        Assert.Equal("""
            [
              {
                "damage_type": "fire",
                "immunity": false,
                "immunity_revoked": false,
                "resistance": false,
                "resistance_revoked": false,
                "vulnerability": false,
                "vulnerability_revoked": true
              }
            ]
            """, (subevent.subevent as DamageAffinity).GetAffinities().PrettyPrint());

        (subevent.subevent as DamageAffinity).GrantVulnerability("fire");
        Assert.False((subevent.subevent as DamageAffinity).IsVulnerable("fire"));
    }

};
