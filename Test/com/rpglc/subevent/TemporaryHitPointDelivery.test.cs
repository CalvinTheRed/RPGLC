using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[AssignDatabase]
[Collection("Serial")]
public class TemporaryHitPointDeliveryTest {

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares")]
    public void Prepares() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        TemporaryHitPointDelivery temporaryHitPointDelivery = new TemporaryHitPointDelivery()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal("""[]""", temporaryHitPointDelivery.json.GetJsonArray("temporary_hit_points").ToString());
    }

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "maximizes temporary hit point dice")]
    public void MaximizesTemporaryHitPointDice() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        TemporaryHitPointDelivery temporaryHitPointDelivery = new TemporaryHitPointDelivery()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "temporary_hit_points": [
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

        temporaryHitPointDelivery.MaximizeTemporaryHitPointDice();

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
            temporaryHitPointDelivery.json.GetJsonArray("temporary_hit_points").PrettyPrint()
        );
    }

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "gets temporary hit points")]
    public void GetsTemporaryHitPoints() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        TemporaryHitPointDelivery temporaryHitPointDelivery = new TemporaryHitPointDelivery()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "temporary_hit_points": [
                        {
                            "bonus": 1,
                            "dice": [
                                { "roll": 3, "size": 6, "determined": [ ] },
                                { "roll": 3, "size": 6, "determined": [ ] }
                            ]
                        },
                        {
                            "bonus": 1,
                            "dice": [
                                { "roll": 3, "size": 6, "determined": [ ] }
                            ]
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal(1 + 3 + 3 + 1 + 3, temporaryHitPointDelivery.GetTemporaryHitPoints());
    }

};
