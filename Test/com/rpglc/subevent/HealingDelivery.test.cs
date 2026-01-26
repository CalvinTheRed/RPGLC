using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class HealingDeliveryTest {

    [Fact(DisplayName = "defaults")]
    public void Defaults() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new HealingDelivery());

        var result = subevent.Advance(context);
        Assert.Equal((null, false), result);
        Assert.Empty(subevent.subevent.json.GetJsonArray("healing").AsList());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "delivers healing (all)")]
    public void DeliversHealingAll() {
        long healing = 10L;
        long maximumHitPoints = 100L;
        long currentHitPoints = 50L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID)
            .SetHealthBase(maximumHitPoints)
            .SetHealthCurrent(currentHitPoints);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);
        SubeventState subevent = new(new HealingDelivery()
            .SetSource(rpglObject)
            .SetTarget(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "healing": [
                        {
                            "bonus": {{healing}},
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
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateMaximumHitPoints);
        Assert.False(result.completed);

        (result.subevent.subevent as CalculateMaximumHitPoints)
            .SetBase(maximumHitPoints);

        result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Equal(currentHitPoints + healing, rpglObject.GetHealthCurrent());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "delivers healing (some)")]
    public void DeliversHealingSome() {
        long healing = 100L;
        long maximumHitPoints = 100L;
        long currentHitPoints = 50L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID)
            .SetHealthBase(maximumHitPoints)
            .SetHealthCurrent(currentHitPoints);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);
        SubeventState subevent = new(new HealingDelivery()
            .SetSource(rpglObject)
            .SetTarget(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "healing": [
                        {
                            "bonus": {{healing}},
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
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateMaximumHitPoints);
        Assert.False(result.completed);

        (result.subevent.subevent as CalculateMaximumHitPoints)
            .SetBase(maximumHitPoints);

        result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Equal(maximumHitPoints, rpglObject.GetHealthCurrent());
    }

};
