using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class TemporaryHitPointDeliveryTest {

    [Fact(DisplayName = "defaults")]
    public void Defaults() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new TemporaryHitPointDelivery());

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Empty(subevent.subevent.json.GetJsonArray("temporary_hit_points").AsList());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "delivers temporary hit points")]
    public void DeliversTemporaryHitPointsAll() {
        long temporaryHitPoints = 10L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);
        SubeventState subevent = new(new TemporaryHitPointDelivery()
            .SetSource(rpglObject)
            .SetTarget(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "temporary_hit_points": [
                        {
                            "bonus": {{temporaryHitPoints}},
                            "dice": [ ],
                            "scale": {
                                "numerator": 1,
                                "denominator": 1,
                                "round_up": false
                            }
                        }
                    ]
                }
                """)));

        // skip over preparatory step
        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Equal(temporaryHitPoints, rpglObject.GetTemporaryHitPoints());
    }

};
