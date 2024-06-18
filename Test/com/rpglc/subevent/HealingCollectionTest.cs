using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.testutils;
using com.rpglc.testutils.mocks;

namespace com.rpglc.subevent;

[AssignDatabase]
[Collection("Serial")]
public class HealingCollectionTest {

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "prepares default")]
    public void PreparesDefault() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        HealingCollection healingCollection = new HealingCollection()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal("""[]""", healingCollection.GetHealingCollection().ToString());
    }

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "prepares healing")]
    public void PreparesHealing() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        HealingCollection healingCollection = new HealingCollection()
            .JoinSubeventData(new JsonObject()
                /*{
                    "healing": [
                        {
                            "formula": "range",
                            "bonus": 1,
                            "dice": [
                                { "count": 1, "size": 6, "determined": [ 3 ] }
                            ],
                        }
                    ]
                }*/
                .PutJsonArray("healing", new JsonArray()
                    .AddJsonObject(new JsonObject()
                        .PutString("formula", "range")
                        .PutLong("bonus", 1L)
                        .PutJsonArray("dice", new JsonArray()
                            .AddJsonObject(new JsonObject()
                                .PutLong("count", 1L)
                                .PutLong("size", 6L)
                                .PutJsonArray("determined", new JsonArray()
                                    .AddLong(3)
                                )
                            )
                        )
                    )
                )
            )
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal("""
            [
              {
                "bonus": 1,
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
            healingCollection.GetHealingCollection().PrettyPrint()
        );
    }

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "adds healing")]
    public void AddsHealing() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        HealingCollection healingCollection = new HealingCollection()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .AddHealing(CalculationSubevent.ProcessBonusJson(
                new(),
                new DummySubevent(),
                new JsonObject()
                    /*{
                        "damage": [
                            {
                                "formula": "range",
                                "bonus": 1,
                                "dice": [
                                    { "count": 1, "size": 6, "determined": [ 3 ] }
                                ],
                            }
                        ]
                    }*/
                    .PutString("formula", "range")
                    .PutLong("bonus", 1L)
                    .PutJsonArray("dice", new JsonArray()
                        .AddJsonObject(new JsonObject()
                            .PutLong("count", 1L)
                            .PutLong("size", 6L)
                            .PutJsonArray("determined", new JsonArray()
                                .AddLong(3)
                            )
                        )
                    ),
                new DummyContext()
            )
        );

        Assert.Equal("""
            [
              {
                "bonus": 1,
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
            healingCollection.GetHealingCollection().PrettyPrint()
        );
    }

};
