using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class GiveTemporaryHitPointsTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "prepares")]
    public void Prepares() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        GiveTemporaryHitPoints giveTemporaryHitPoints = new GiveTemporaryHitPoints()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "temporary_hit_points": [
                        {
                            "formula": "number",
                            "number": 10
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal("""
            [
              {
                "bonus": 10,
                "dice": [ ],
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

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "gives temporary hit points")]
    public void GivesTemporaryHitPoints() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);

        GiveTemporaryHitPoints giveTemporaryHitPoints = new GiveTemporaryHitPoints()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "temporary_hit_points": [
                        {
                            "formula": "number",
                            "number": 10
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .SetTarget(rpglObject)
            .Invoke(new DummyContext(), new());

        Assert.Equal(0 + 10, rpglObject.GetTemporaryHitPoints());
    }

};
