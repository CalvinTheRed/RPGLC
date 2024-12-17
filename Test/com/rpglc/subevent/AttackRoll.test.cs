using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[AssignDatabase]
[Collection("Serial")]
[DieTestingMode]
[RPGLInitTesting]
public class AttackRollTest {

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares")]
    public void Prepares() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        AttackRoll attackRoll = new AttackRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "attack_ability": "str",
                    "attack_type": "melee"
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.False(attackRoll.json.GetBool("withhold_damage_modifier"));
        Assert.False(attackRoll.json.GetBool("use_origin_attack_ability"));
        Assert.True(attackRoll.HasTag("str"));
        Assert.True(attackRoll.HasTag("melee"));
        Assert.Equal("""[]""", attackRoll.json.GetJsonArray("damage").ToString());
        Assert.Equal("""[]""", attackRoll.json.GetJsonArray("vampirism").ToString());
    }

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "hits")]
    [DummyCounterManager]
    public void Hits() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        AttackRoll attackRoll = new AttackRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "attack_ability": "str",
                    "attack_type": "melee",
                    "determined": [ 19 ],
                    "damage": [
                        {
                            "damage_type": "fire",
                            "formula": "range",
                            "bonus": 1,
                            "dice": [
                                { "count": 1, "size": 6, "determined": [ 3 ] }
                            ]
                        }
                    ],
                    "hit": [
                        {
                            "subevent": "dummy_subevent"
                        },
                        {
                            "subevent": "dummy_subevent"
                        }
                    ],
                    "miss": [
                        {
                            "subevent": "dummy_subevent"
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .SetTarget(rpglObject)
            .Invoke(new DummyContext(), new());

        Assert.Equal(2, DummySubevent.Counter);

        Assert.Equal(1000 - 1 - 3, rpglObject.GetHealthCurrent());
    }

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "misses")]
    [DummyCounterManager]
    public void Misses() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        AttackRoll attackRoll = new AttackRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "attack_ability": "str",
                    "attack_type": "melee",
                    "determined": [ 2 ],
                    "damage": [
                        {
                            "damage_type": "fire",
                            "formula": "range",
                            "bonus": 1,
                            "dice": [
                                { "count": 1, "size": 6, "determined": [ 3 ] }
                            ]
                        }
                    ],
                    "hit": [
                        {
                            "subevent": "dummy_subevent"
                        }
                    ],
                    "miss": [
                        {
                            "subevent": "dummy_subevent"
                        },
                        {
                            "subevent": "dummy_subevent"
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .SetTarget(rpglObject)
            .Invoke(new DummyContext(), new());

        Assert.Equal(2, DummySubevent.Counter);

        Assert.Equal(1000, rpglObject.GetHealthCurrent());
    }

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "critically hits")]
    [DummyCounterManager]
    public void CriticallyHits() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        AttackRoll attackRoll = new AttackRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "attack_ability": "str",
                    "attack_type": "melee",
                    "determined": [ 20 ],
                    "damage": [
                        {
                            "damage_type": "fire",
                            "formula": "range",
                            "bonus": 1,
                            "dice": [
                                { "count": 1, "size": 6, "determined": [ 3 ] }
                            ]
                        }
                    ],
                    "hit": [
                        {
                            "subevent": "dummy_subevent"
                        },
                        {
                            "subevent": "dummy_subevent"
                        }
                    ],
                    "miss": [
                        {
                            "subevent": "dummy_subevent"
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .SetTarget(rpglObject)
            .Invoke(new DummyContext(), new());

        Assert.Equal(2, DummySubevent.Counter);

        Assert.Equal(1000 - 1 - 3 - 3, rpglObject.GetHealthCurrent());
    }

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "critically misses")]
    [DummyCounterManager]
    public void CriticallyMisses() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        AttackRoll attackRoll = (AttackRoll) new AttackRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "attack_ability": "str",
                    "attack_type": "melee",
                    "determined": [ 1 ],
                    "damage": [
                        {
                            "damage_type": "fire",
                            "formula": "range",
                            "bonus": 1,
                            "dice": [
                                { "count": 1, "size": 6, "determined": [ 3 ] }
                            ]
                        }
                    ],
                    "hit": [
                        {
                            "subevent": "dummy_subevent"
                        }
                    ],
                    "miss": [
                        {
                            "subevent": "dummy_subevent"
                        },
                        {
                            "subevent": "dummy_subevent"
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .AddBonus(new JsonObject().LoadFromString("""
                {
                    "bonus": 100,
                    "dice": [ ],
                    "scale": {
                        "numerator": 1,
                        "denominator": 1,
                        "round_up": false
                    }
                }
                """))
            .SetTarget(rpglObject)
            .Invoke(new DummyContext(), new());

        Assert.Equal(2, DummySubevent.Counter);

        Assert.Equal(1000, rpglObject.GetHealthCurrent());
    }

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "uses origin attack ability")]
    public void UsesOriginAttackAbility() {
        RPGLObject originObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        originObject.GetAbilityScores().PutLong("int", 20);

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1")
            .SetOriginObject(originObject.GetUuid());

        RPGLContext context = new DummyContext()
            .Add(originObject)
            .Add(rpglObject);

        AttackRoll attackRoll = new AttackRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "attack_ability": "int",
                    "attack_type": "melee",
                    "use_origin_attack_ability": true,
                    "determined": [ 19 ],
                    "damage": [
                        {
                            "damage_type": "fire",
                            "formula": "range",
                            "bonus": 1,
                            "dice": [ ]
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(context, new());

        Assert.Equal(5, attackRoll.GetBonus());

        attackRoll
            .SetTarget(rpglObject)
            .Invoke(context, new());

        Assert.Equal(1000 - 1 - 5, rpglObject.GetHealthCurrent());
    }

};
