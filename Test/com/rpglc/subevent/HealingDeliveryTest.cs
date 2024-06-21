using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[AssignDatabase]
[Collection("Serial")]
public class HealingDeliveryTest {

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares")]
    public void PreparesDefault() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        HealingDelivery healingDelivery = new HealingDelivery()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal("""[]""", healingDelivery.json.GetJsonArray("healing").ToString());
    }

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "maximizes healing dice")]
    public void MaximizesTypelessDamageDice() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        HealingDelivery healingDelivery = new HealingDelivery()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
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
                }
                """))
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

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "gets healing")]
    public void GetsHealing() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        HealingDelivery healingDelivery = new HealingDelivery()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "healing": [
                        {
                            "bonus": 1,
                            "dice": [
                                { "size": 6, "determined": [ 3 ] },
                                { "size": 6, "determined": [ 3 ] }
                            ]
                        },
                        {
                            "bonus": 1,
                            "dice": [
                                { "size": 6, "determined": [ 3 ] }
                            ]
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        healingDelivery.MaximizeHealingDice();

        Assert.Equal(1 + 3 + 3 + 1 + 3, healingDelivery.GetHealing());
    }

};
