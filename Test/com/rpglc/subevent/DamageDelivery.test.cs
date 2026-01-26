using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
[RPGLInitTesting]
public class DamageDeliveryTest {

    [Fact(DisplayName = "defaults")]
    public void Defaults() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new DamageDelivery());

        var result = subevent.Advance(context);
        Assert.Equal((null, false), result);
        Assert.Empty(subevent.subevent.json.GetJsonArray("damage").AsList());
        Assert.Equal("all", subevent.subevent.json.GetString("damage_proportion"));
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "delivers damage (all)")]
    public void DeliversDamageAll() {
        long damage = 10L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);
        SubeventState subevent = new(new DamageDelivery()
            .SetSource(rpglObject)
            .SetTarget(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": {{damage}},
                            "dice": [ ],
                            "scale": {
                                "numerator": 1,
                                "denominator": 1,
                                "round_up": false
                            }
                        }
                    ],
                    "damage_proportion": "all"
                }
                """)));

        // skip over preparatory step
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageAffinity);
        Assert.False(result.completed);

        // use default affinities

        result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Equal(1000L - damage, rpglObject.GetHealthCurrent());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "delivers damage (half)")]
    public void DeliversDamageHalf() {
        long damage = 10L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);
        SubeventState subevent = new(new DamageDelivery()
            .SetSource(rpglObject)
            .SetTarget(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": {{damage}},
                            "dice": [ ],
                            "scale": {
                                "numerator": 1,
                                "denominator": 1,
                                "round_up": false
                            }
                        }
                    ],
                    "damage_proportion": "half"
                }
                """)));

        // skip over preparatory step
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageAffinity);
        Assert.False(result.completed);

        // use default affinities

        result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Equal(1000L - (damage / 2), rpglObject.GetHealthCurrent());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "delivers damage (none)")]
    public void DeliversDamageNone() {
        long damage = 10L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);
        SubeventState subevent = new(new DamageDelivery()
            .SetSource(rpglObject)
            .SetTarget(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": {{damage}},
                            "dice": [ ],
                            "scale": {
                                "numerator": 1,
                                "denominator": 1,
                                "round_up": false
                            }
                        }
                    ],
                    "damage_proportion": "none"
                }
                """)));

        // skip over preparatory step
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageAffinity);
        Assert.False(result.completed);

        // use default affinities

        result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Equal(1000L - 0L, rpglObject.GetHealthCurrent());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "delivers damage (resisted)")]
    public void DeliversDamageResisted() {
        long damage = 10L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);
        SubeventState subevent = new(new DamageDelivery()
            .SetSource(rpglObject)
            .SetTarget(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": {{damage}},
                            "dice": [ ],
                            "scale": {
                                "numerator": 1,
                                "denominator": 1,
                                "round_up": false
                            }
                        }
                    ],
                    "damage_proportion": "all"
                }
                """)));

        // skip over preparatory step
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageAffinity);
        Assert.False(result.completed);

        (result.subevent.subevent as DamageAffinity)
            .GrantResistance("fire");

        result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Equal(1000L - (damage / 2), rpglObject.GetHealthCurrent());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "delivers damage (vulnerable)")]
    public void DeliversDamageVulnerable() {
        long damage = 10L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);
        SubeventState subevent = new(new DamageDelivery()
            .SetSource(rpglObject)
            .SetTarget(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": {{damage}},
                            "dice": [ ],
                            "scale": {
                                "numerator": 1,
                                "denominator": 1,
                                "round_up": false
                            }
                        }
                    ],
                    "damage_proportion": "all"
                }
                """)));

        // skip over preparatory step
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageAffinity);
        Assert.False(result.completed);

        (result.subevent.subevent as DamageAffinity)
            .GrantVulnerability("fire");

        result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Equal(1000L - (damage * 2), rpglObject.GetHealthCurrent());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "delivers damage (immune)")]
    public void DeliversDamageImmune() {
        long damage = 10L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);
        SubeventState subevent = new(new DamageDelivery()
            .SetSource(rpglObject)
            .SetTarget(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": {{damage}},
                            "dice": [ ],
                            "scale": {
                                "numerator": 1,
                                "denominator": 1,
                                "round_up": false
                            }
                        }
                    ],
                    "damage_proportion": "all"
                }
                """)));

        // skip over preparatory step
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageAffinity);
        Assert.False(result.completed);

        (result.subevent.subevent as DamageAffinity)
            .GrantImmunity("fire");

        result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Equal(1000L - 0L, rpglObject.GetHealthCurrent());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "delivers damage (resisted, halved)")]
    public void DeliversDamageResistedHalved() {
        long damage = 10L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);
        SubeventState subevent = new(new DamageDelivery()
            .SetSource(rpglObject)
            .SetTarget(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": {{damage}},
                            "dice": [ ],
                            "scale": {
                                "numerator": 1,
                                "denominator": 1,
                                "round_up": false
                            }
                        }
                    ],
                    "damage_proportion": "half"
                }
                """)));

        // skip over preparatory step
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageAffinity);
        Assert.False(result.completed);

        (result.subevent.subevent as DamageAffinity)
            .GrantResistance("fire");

        result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Equal(1000L - (damage / 2 / 2), rpglObject.GetHealthCurrent());
    }

};
