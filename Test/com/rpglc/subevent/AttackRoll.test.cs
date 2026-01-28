using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.subevent;

[Collection("Serial")]
[DieTestingMode]
[RPGLInitTesting]
public class AttackRollTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "hits (does not deal critical damage)")]
    public void HitsDoesNotDealCriticalDamage() {
        long strScore = 12L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.GetAbilityScores().PutLong("str", strScore);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SubeventState subevent = new(new AttackRoll()
            .SetSource(rpglObject)
            .SetTarget(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "ability": "str",
                    "damage": [
                        {
                            "damage_type": "fire",
                            "formula": "dice",
                            "dice": [
                                { "count": 1, "size": 6, "determined": [ 3, -1 ] }
                            ]
                        }
                    ],
                    "determined": [ 19, -1 ],
                    "hit": [
                        {
                            "subevent": "dummy_subevent"
                        }
                    ]
                }
                """)));

        var result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);
        Assert.False(subevent.subevent.json.GetBool("use_origin_ability"));
        Assert.Empty(subevent.subevent.json.GetJsonArray("vampirism").AsList());
        Assert.Equal(20L, subevent.subevent.json.GetLong("critical_hit_threshold"));
        Assert.False(subevent.subevent.json.GetBool("crit_on_hit"));

        // skip calculationsubevent default
        _ = subevent.Advance(context);
        
        // skip advancebase
        _ = subevent.Advance(context);
        
        // advancebonuses
        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateAbilityScore);
        Assert.False(result.completed);

        (result.subevent.subevent as CalculateAbilityScore)
            .SetBase(strScore);
        
        _ = subevent.Advance(context);
        Assert.Equal("""
            [
              {
                "bonus": 1,
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent.subevent as AttackRoll).GetBonuses().PrettyPrint());

        // advanceminimum
        _ = subevent.Advance(context);

        // skip rollsubevent defaults
        _ = subevent.Advance(context);

        // enter targeting phase
        _ = subevent.Advance(context);
        Assert.Equal(SubeventState.Phase.Targeting, subevent.phase);

        subevent.SetTargets([rpglObject]);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateCriticalHitThreshold);
        Assert.False(result.completed);

        SubeventState dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);
        Assert.Equal(19L, (subevent.subevent as AttackRoll).GetBase());

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateArmorClass);
        Assert.False(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageCollection);
        Assert.False(result.completed);
        Assert.True(result.subevent.subevent.GetTags().Contains("base_damage_collection"));

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageCollection);
        Assert.False(result.completed);
        Assert.True(result.subevent.subevent.GetTags().Contains("target_damage_collection"));

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);
        Assert.Equal("""
            [
              {
                "bonus": 0,
                "damage_type": "fire",
                "dice": [
                  {
                    "determined": [
                      3,
                      -1
                    ],
                    "size": 6
                  }
                ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, subevent.subevent.json.GetJsonArray("damage").PrettyPrint());

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageRoll);
        Assert.False(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageDelivery);
        Assert.False(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DummySubevent);
        Assert.True(result.completed);

        Assert.Equal(1000L - 3L, rpglObject.GetHealthCurrent());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "hits (does deal critical damage)")]
    public void HitsDoesDealCriticalDamage() {
        long strScore = 12L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.GetAbilityScores().PutLong("str", strScore);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SubeventState subevent = new(new AttackRoll()
            .SetCritOnHit()
            .SetSource(rpglObject)
            .SetTarget(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "ability": "str",
                    "damage": [
                        {
                            "damage_type": "fire",
                            "formula": "dice",
                            "dice": [
                                { "count": 1, "size": 6, "determined": [ 3, -1 ] }
                            ]
                        }
                    ],
                    "determined": [ 19, -1 ]
                }
                """)));

        var result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);
        Assert.False(subevent.subevent.json.GetBool("use_origin_ability"));
        Assert.Empty(subevent.subevent.json.GetJsonArray("vampirism").AsList());
        Assert.Equal(20L, subevent.subevent.json.GetLong("critical_hit_threshold"));
        Assert.True(subevent.subevent.json.GetBool("crit_on_hit"));

        // skip calculationsubevent default
        _ = subevent.Advance(context);

        // skip advancebase
        _ = subevent.Advance(context);

        // advancebonuses
        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateAbilityScore);
        Assert.False(result.completed);

        (result.subevent.subevent as CalculateAbilityScore)
            .SetBase(strScore);

        _ = subevent.Advance(context);
        Assert.Equal("""
            [
              {
                "bonus": 1,
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent.subevent as AttackRoll).GetBonuses().PrettyPrint());

        // advanceminimum
        _ = subevent.Advance(context);

        // skip rollsubevent defaults
        _ = subevent.Advance(context);

        // enter targeting phase
        _ = subevent.Advance(context);
        Assert.Equal(SubeventState.Phase.Targeting, subevent.phase);

        subevent.SetTargets([rpglObject]);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateCriticalHitThreshold);
        Assert.False(result.completed);

        SubeventState dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);
        Assert.Equal(19L, (subevent.subevent as AttackRoll).GetBase());

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateArmorClass);
        Assert.False(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CriticalDamageConfirmation);
        Assert.False(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageCollection);
        Assert.False(result.completed);
        Assert.True(result.subevent.subevent.GetTags().Contains("base_damage_collection"));

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageCollection);
        Assert.False(result.completed);
        Assert.True(result.subevent.subevent.GetTags().Contains("target_damage_collection"));

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);
        
        result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageCollection);
        Assert.False(result.completed);
        Assert.True(result.subevent.subevent.GetTags().Contains("critical_damage_collection"));

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);
        Assert.Equal("""
            [
              {
                "bonus": 0,
                "damage_type": "fire",
                "dice": [
                  {
                    "determined": [
                      3,
                      -1
                    ],
                    "size": 6
                  },
                  {
                    "determined": [
                      3,
                      -1
                    ],
                    "size": 6
                  }
                ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, subevent.subevent.json.GetJsonArray("damage").PrettyPrint());

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageRoll);
        Assert.False(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageDelivery);
        Assert.True(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        Assert.Equal(1000L - 3L - 3L, rpglObject.GetHealthCurrent());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "crits (does deal critical damage)")]
    public void CritsDoesDealCriticalDamage() {
        long strScore = 12L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.GetAbilityScores().PutLong("str", strScore);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SubeventState subevent = new(new AttackRoll()
            .SetCritOnHit()
            .SetSource(rpglObject)
            .SetTarget(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "ability": "str",
                    "damage": [
                        {
                            "damage_type": "fire",
                            "formula": "dice",
                            "dice": [
                                { "count": 1, "size": 6, "determined": [ 3, -1 ] }
                            ]
                        }
                    ],
                    "determined": [ 20, -1 ]
                }
                """)));

        var result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);
        Assert.False(subevent.subevent.json.GetBool("use_origin_ability"));
        Assert.Empty(subevent.subevent.json.GetJsonArray("vampirism").AsList());
        Assert.Equal(20L, subevent.subevent.json.GetLong("critical_hit_threshold"));
        Assert.True(subevent.subevent.json.GetBool("crit_on_hit"));

        // skip calculationsubevent default
        _ = subevent.Advance(context);

        // skip advancebase
        _ = subevent.Advance(context);

        // advancebonuses
        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateAbilityScore);
        Assert.False(result.completed);

        (result.subevent.subevent as CalculateAbilityScore)
            .SetBase(strScore);

        _ = subevent.Advance(context);
        Assert.Equal("""
            [
              {
                "bonus": 1,
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent.subevent as AttackRoll).GetBonuses().PrettyPrint());

        // advanceminimum
        _ = subevent.Advance(context);

        // skip rollsubevent defaults
        _ = subevent.Advance(context);

        // enter targeting phase
        _ = subevent.Advance(context);
        Assert.Equal(SubeventState.Phase.Targeting, subevent.phase);

        subevent.SetTargets([rpglObject]);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateCriticalHitThreshold);
        Assert.False(result.completed);

        SubeventState dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);
        Assert.Equal(20L, (subevent.subevent as AttackRoll).GetBase());

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CriticalDamageConfirmation);
        Assert.False(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageCollection);
        Assert.False(result.completed);
        Assert.True(result.subevent.subevent.GetTags().Contains("base_damage_collection"));

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageCollection);
        Assert.False(result.completed);
        Assert.True(result.subevent.subevent.GetTags().Contains("target_damage_collection"));

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageCollection);
        Assert.False(result.completed);
        Assert.True(result.subevent.subevent.GetTags().Contains("critical_damage_collection"));

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);
        Assert.Equal("""
            [
              {
                "bonus": 0,
                "damage_type": "fire",
                "dice": [
                  {
                    "determined": [
                      3,
                      -1
                    ],
                    "size": 6
                  },
                  {
                    "determined": [
                      3,
                      -1
                    ],
                    "size": 6
                  }
                ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, subevent.subevent.json.GetJsonArray("damage").PrettyPrint());

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageRoll);
        Assert.False(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageDelivery);
        Assert.True(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        Assert.Equal(1000L - 3L - 3L, rpglObject.GetHealthCurrent());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "crits (does not deal critical damage)")]
    public void CritsDoesNotDealCriticalDamage() {
        long strScore = 12L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.GetAbilityScores().PutLong("str", strScore);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SubeventState subevent = new(new AttackRoll()
            .SetCritOnHit()
            .SetSource(rpglObject)
            .SetTarget(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "ability": "str",
                    "damage": [
                        {
                            "damage_type": "fire",
                            "formula": "dice",
                            "dice": [
                                { "count": 1, "size": 6, "determined": [ 3, -1 ] }
                            ]
                        }
                    ],
                    "determined": [ 20, -1 ]
                }
                """)));

        var result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);
        Assert.False(subevent.subevent.json.GetBool("use_origin_ability"));
        Assert.Empty(subevent.subevent.json.GetJsonArray("vampirism").AsList());
        Assert.Equal(20L, subevent.subevent.json.GetLong("critical_hit_threshold"));
        Assert.True(subevent.subevent.json.GetBool("crit_on_hit"));

        // skip calculationsubevent default
        _ = subevent.Advance(context);

        // skip advancebase
        _ = subevent.Advance(context);

        // advancebonuses
        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateAbilityScore);
        Assert.False(result.completed);

        (result.subevent.subevent as CalculateAbilityScore)
            .SetBase(strScore);

        _ = subevent.Advance(context);
        Assert.Equal("""
            [
              {
                "bonus": 1,
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent.subevent as AttackRoll).GetBonuses().PrettyPrint());

        // advanceminimum
        _ = subevent.Advance(context);

        // skip rollsubevent defaults
        _ = subevent.Advance(context);

        // enter targeting phase
        _ = subevent.Advance(context);
        Assert.Equal(SubeventState.Phase.Targeting, subevent.phase);

        subevent.SetTargets([rpglObject]);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateCriticalHitThreshold);
        Assert.False(result.completed);

        SubeventState dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);
        Assert.Equal(20L, (subevent.subevent as AttackRoll).GetBase());

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CriticalDamageConfirmation);
        Assert.False(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);
        (dependency.subevent as CriticalDamageConfirmation).SuppressCriticalDamage();

        result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageCollection);
        Assert.False(result.completed);
        Assert.True(result.subevent.subevent.GetTags().Contains("base_damage_collection"));

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageCollection);
        Assert.False(result.completed);
        Assert.True(result.subevent.subevent.GetTags().Contains("target_damage_collection"));

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);
        Assert.Equal("""
            [
              {
                "bonus": 0,
                "damage_type": "fire",
                "dice": [
                  {
                    "determined": [
                      3,
                      -1
                    ],
                    "size": 6
                  }
                ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, subevent.subevent.json.GetJsonArray("damage").PrettyPrint());

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageRoll);
        Assert.False(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageDelivery);
        Assert.True(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        Assert.Equal(1000L - 3L, rpglObject.GetHealthCurrent());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "misses")]
    public void Misses() {
        long strScore = 12L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.GetAbilityScores().PutLong("str", strScore);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SubeventState subevent = new(new AttackRoll()
            .SetSource(rpglObject)
            .SetTarget(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "ability": "str",
                    "damage": [
                        {
                            "damage_type": "fire",
                            "formula": "dice",
                            "dice": [
                                { "count": 1, "size": 6, "determined": [ 3, -1 ] }
                            ]
                        }
                    ],
                    "determined": [ 2, -1 ],
                    "miss": [
                        {
                            "subevent": "dummy_subevent"
                        }
                    ]
                }
                """)));

        var result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);
        Assert.False(subevent.subevent.json.GetBool("use_origin_ability"));
        Assert.Empty(subevent.subevent.json.GetJsonArray("vampirism").AsList());
        Assert.Equal(20L, subevent.subevent.json.GetLong("critical_hit_threshold"));
        Assert.False(subevent.subevent.json.GetBool("crit_on_hit"));

        // skip calculationsubevent default
        _ = subevent.Advance(context);

        // skip advancebase
        _ = subevent.Advance(context);

        // advancebonuses
        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateAbilityScore);
        Assert.False(result.completed);

        (result.subevent.subevent as CalculateAbilityScore)
            .SetBase(strScore);

        _ = subevent.Advance(context);
        Assert.Equal("""
            [
              {
                "bonus": 1,
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent.subevent as AttackRoll).GetBonuses().PrettyPrint());

        // advanceminimum
        _ = subevent.Advance(context);

        // skip rollsubevent defaults
        _ = subevent.Advance(context);

        // enter targeting phase
        _ = subevent.Advance(context);
        Assert.Equal(SubeventState.Phase.Targeting, subevent.phase);

        subevent.SetTargets([rpglObject]);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateCriticalHitThreshold);
        Assert.False(result.completed);

        SubeventState dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);
        Assert.Equal(2L, (subevent.subevent as AttackRoll).GetBase());

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateArmorClass);
        Assert.False(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DummySubevent);
        Assert.True(result.completed);

        Assert.Equal(1000L, rpglObject.GetHealthCurrent());
    }

};
