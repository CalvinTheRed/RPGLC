using com.rpglc.core;
using com.rpglc.database;
using com.rpglc.json;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[AssignDatabase]
[Collection("Serial")]
public class GiveTemporaryHitPointsTest {

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "prepares")]
    public void Prepares() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        GiveTemporaryHitPoints giveTemporaryHitPoints = new GiveTemporaryHitPoints()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "temporary_hit_points": [
                        {
                            "formula": "range",
                            "bonus": 1,
                            "dice": [
                                { "count": 1, "size": 6, "determined": [ 3, -1 ] }
                            ]
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
                "dice": [
                  {
                    "determined": [
                      -1
                    ],
                    "roll": 3,
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
            giveTemporaryHitPoints.json.GetJsonArray("temporary_hit_points").PrettyPrint());
    }

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "gives temporary hit points")]
    public void GivesTemporaryHitPoints() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");

        GiveTemporaryHitPoints giveTemporaryHitPoints = new GiveTemporaryHitPoints()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "temporary_hit_points": [
                        {
                            "formula": "range",
                            "bonus": 1,
                            "dice": [
                                { "count": 1, "size": 6, "determined": [ 3, -1 ] }
                            ]
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .SetTarget(rpglObject)
            .Invoke(new DummyContext(), new());

        rpglObject = DBManager.QueryRPGLObject(x => x._id == rpglObject.GetId());

        Assert.Equal(0 + 1 + 3, rpglObject.GetTemporaryHitPoints());
    }

};
