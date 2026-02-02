using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
[DieTestingMode]
public class GiveTemporaryHitPointsTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "gives temporary hit points")]
    public void GivesTemporaryHitPoints() {
        long temporaryHitPoints = 10L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID)
            .SetHealthCurrent(0L);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SubeventState subevent = new(new GiveTemporaryHitPoints()
            .SetSource(rpglObject)
            .SetTarget(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "temporary_hit_points": [
                        {
                            "formula": "number",
                            "number": {{temporaryHitPoints}}
                        }
                    ]
                }
                """)));

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is TemporaryHitPointCollection);
        Assert.True(result.subevent.subevent.GetTags().Contains("base_temporary_hit_point_collection"));
        Assert.False(result.completed);

        SubeventState dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is TemporaryHitPointRoll);
        Assert.True(result.subevent.subevent.GetTags().Contains("base_temporary_hit_point_roll"));
        Assert.False(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);
        Assert.Equal(SubeventState.Phase.Targeting, subevent.phase);
        Assert.Equal($$"""
            [
              {
                "bonus": {{temporaryHitPoints}},
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, subevent.subevent.json.GetJsonArray("temporary_hit_points").PrettyPrint());

        subevent.SetTargets([rpglObject]);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is TemporaryHitPointCollection);
        Assert.True(result.subevent.subevent.GetTags().Contains("target_temporary_hit_point_collection"));
        Assert.False(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);
        (dependency.subevent as TemporaryHitPointCollection).AddTemporaryHitPoints(new JsonObject().LoadFromString($$"""
            {
                "bonus": {{temporaryHitPoints}},
                "dice": [ ],
                "scale": {
                    "numerator": 1,
                    "denominator": 1,
                    "round_up": false
                }
            }
            """));

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is TemporaryHitPointRoll);
        Assert.True(result.subevent.subevent.GetTags().Contains("target_temporary_hit_point_roll"));
        Assert.False(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is TemporaryHitPointDelivery);
        Assert.True(result.completed);
        Assert.Equal($$"""
            [
              {
                "bonus": {{temporaryHitPoints}},
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              },
              {
                "bonus": {{temporaryHitPoints}},
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, subevent.subevent.json.GetJsonArray("temporary_hit_points").PrettyPrint());
    }

};
