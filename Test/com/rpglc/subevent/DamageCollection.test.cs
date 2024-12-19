using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class DamageCollectionTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares default")]
    public void PreparesDefault() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DamageCollection damageCollection = new DamageCollection()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal("""[]""", damageCollection.GetDamageCollection().ToString());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares damage")]
    public void PreparesDamage() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DamageCollection damageCollection = new DamageCollection()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "formula": "range",
                            "bonus": 1,
                            "dice": [
                                { "count": 1, "size": 6, "determined": [ 3 ] }
                            ],
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal("""
            [
              {
                "bonus": 1,
                "damage_type": "fire",
                "dice": [
                  {
                    "determined": [
                      3
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
            """,
            damageCollection.GetDamageCollection().PrettyPrint()
        );
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "adds damage")]
    public void AddsDamage() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DamageCollection damageCollection = new DamageCollection()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .AddDamage(CalculationSubevent.ProcessBonusJson(
                new(),
                new DummySubevent(),
                new JsonObject().LoadFromString("""
                    {
                        "damage_type": "fire",
                        "formula": "range",
                        "bonus": 1,
                        "dice": [
                            { "count": 1, "size": 6, "determined": [ 3 ] }
                        ],
                    }
                    """),
                new DummyContext()
            ).PutString("damage_type", "fire")
        );

        Assert.Equal("""
            [
              {
                "bonus": 1,
                "damage_type": "fire",
                "dice": [
                  {
                    "determined": [
                      3
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
            """,
            damageCollection.GetDamageCollection().PrettyPrint()
        );
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "includes damage type")]
    public void IncludesDamageType() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DamageCollection damageCollection = new DamageCollection()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "formula": "range",
                            "bonus": 1,
                            "dice": [
                                { "count": 1, "size": 6, "determined": [ 3 ] }
                            ],
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.True(damageCollection.IncludesDamageType("fire"));
        Assert.False(damageCollection.IncludesDamageType("cold"));
    }

};
