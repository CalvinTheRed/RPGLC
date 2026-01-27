using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class DealDamageTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "deals damage (not vampiric)")]
    public void DealsDamageNotVampiric() {
        long damage = 10L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SubeventState subevent = new(new DealDamage()
            .SetSource(rpglObject)
            .SetTarget(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "formula": "number",
                            "number": {{damage}}
                        }
                    ]
                }
                """)));

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageCollection);
        Assert.True(result.subevent.subevent.GetTags().Contains("base_damage_collection"));
        Assert.False(result.completed);

        SubeventState dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageRoll);
        Assert.True(result.subevent.subevent.GetTags().Contains("base_damage_roll"));
        Assert.False(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);
        Assert.Equal(SubeventState.Phase.Targeting, subevent.phase);
        Assert.Equal($$"""
            [
              {
                "bonus": {{damage}},
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, subevent.subevent.json.GetJsonArray("damage").PrettyPrint());

        subevent.SetTargets([rpglObject]);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageCollection);
        Assert.True(result.subevent.subevent.GetTags().Contains("target_damage_collection"));
        Assert.False(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);
        (dependency.subevent as DamageCollection).AddDamage(new JsonObject().LoadFromString($$"""
            {
                "bonus": {{damage}},
                "damage_type": "cold",
                "dice": [ ],
                "scale": {
                    "numerator": 1,
                    "denominator": 1,
                    "round_up": false
                }
            }
            """));

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageRoll);
        Assert.True(result.subevent.subevent.GetTags().Contains("target_damage_roll"));
        Assert.False(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageDelivery);
        Assert.True(result.completed);
        Assert.Equal($$"""
            [
              {
                "bonus": {{damage}},
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              },
              {
                "bonus": {{damage}},
                "damage_type": "cold",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, subevent.subevent.json.GetJsonArray("damage").PrettyPrint());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "deals damage (vampiric)")]
    public void DealsDamageVampiric() {
        long damage = 10L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SubeventState subevent = new(new DealDamage()
            .SetSource(rpglObject)
            .SetTarget(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "formula": "number",
                            "number": {{damage}}
                        }
                    ],
                    "vampirism": [
                        {
                            "damage_type": "necrotic",
                            "scale": {
                                "numerator": 1,
                                "denominator": 1,
                                "round_up": false
                            }
                        }
                    ]
                }
                """)));

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageCollection);
        Assert.True(result.subevent.subevent.GetTags().Contains("base_damage_collection"));
        Assert.False(result.completed);

        SubeventState dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageRoll);
        Assert.True(result.subevent.subevent.GetTags().Contains("base_damage_roll"));
        Assert.False(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);
        Assert.Equal(SubeventState.Phase.Targeting, subevent.phase);
        Assert.Equal($$"""
            [
              {
                "bonus": {{damage}},
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, subevent.subevent.json.GetJsonArray("damage").PrettyPrint());

        subevent.SetTargets([rpglObject]);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageCollection);
        Assert.True(result.subevent.subevent.GetTags().Contains("target_damage_collection"));
        Assert.False(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);
        (dependency.subevent as DamageCollection).AddDamage(new JsonObject().LoadFromString($$"""
            {
                "bonus": {{damage}},
                "damage_type": "cold",
                "dice": [ ],
                "scale": {
                    "numerator": 1,
                    "denominator": 1,
                    "round_up": false
                }
            }
            """));

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageRoll);
        Assert.True(result.subevent.subevent.GetTags().Contains("target_damage_roll"));
        Assert.False(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageDelivery);
        Assert.False(result.completed);
        Assert.Equal($$"""
            [
              {
                "bonus": {{damage}},
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              },
              {
                "bonus": {{damage}},
                "damage_type": "cold",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, subevent.subevent.json.GetJsonArray("damage").PrettyPrint());

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is HealingCollection);
        Assert.True(result.subevent.subevent.GetTags().Contains("vampiric"));

        // remainder of state flow covered by VampiricSubevent.test.cs
    }

};
