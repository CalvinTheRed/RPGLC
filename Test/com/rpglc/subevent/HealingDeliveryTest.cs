using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.testutils;
using com.rpglc.testutils.mocks;

namespace com.rpglc.subevent;

[AssignDatabase]
[Collection("Serial")]
public class HealingDeliveryTest {

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "prepares")]
    public void PreparesDefault() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        HealingDelivery healingDelivery = new HealingDelivery()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal("""[]""", healingDelivery.json.GetJsonArray("healing").ToString());
    }

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [DieTestingMode]
    [Fact(DisplayName = "maximizes healing dice")]
    public void MaximizesTypelessDamageDice() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        HealingDelivery healingDelivery = new HealingDelivery()
            .JoinSubeventData(new JsonObject()
                /*{
                    "healing": [
                        {
                            "bonus": 1,
                            "dice": [
                                { "size": 6, "determined": [ -1 ] }
                            ]
                        },
                        {
                            "bonus": 1,
                            "dice": [
                                { "size": 6, "determined": [ -1 ] }
                            ]
                        }
                    ]
                }*/
                .PutJsonArray("healing", new JsonArray()
                    .AddJsonObject(new JsonObject()
                        .PutLong("bonus", 1)
                        .PutJsonArray("dice", new JsonArray()
                            .AddJsonObject(new JsonObject()
                                .PutLong("size", 6)
                                .PutJsonArray("determined", new JsonArray()
                                    .AddLong(-1)
                                )
                            )
                        )
                    )
                    .AddJsonObject(new JsonObject()
                        .PutLong("bonus", 1)
                        .PutJsonArray("dice", new JsonArray()
                            .AddJsonObject(new JsonObject()
                                .PutLong("size", 6)
                                .PutJsonArray("determined", new JsonArray()
                                    .AddLong(-1)
                                )
                            )
                        )
                    )
                )
            )
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        healingDelivery.MaximizeHealingDice();

        Assert.Equal("""
            [
              {
                "bonus": 1,
                "dice": [
                  {
                    "determined": [
                      -1
                    ],
                    "roll": 6,
                    "size": 6
                  }
                ]
              },
              {
                "bonus": 1,
                "dice": [
                  {
                    "determined": [
                      -1
                    ],
                    "roll": 6,
                    "size": 6
                  }
                ]
              }
            ]
            """,
            healingDelivery.json.GetJsonArray("healing").PrettyPrint()
        );
    }

};
