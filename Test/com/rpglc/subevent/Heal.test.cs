using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class HealTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "heals")]
    public void Heals() {
        long healing = 10L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID)
            .SetHealthCurrent(0L);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SubeventState subevent = new(new Heal()
            .SetSource(rpglObject)
            .SetTarget(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "healing": [
                        {
                            "formula": "number",
                            "number": {{healing}}
                        }
                    ]
                }
                """)));

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is HealingCollection);
        Assert.True(result.subevent.subevent.GetTags().Contains("base_healing_collection"));
        Assert.False(result.completed);

        SubeventState dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is HealingRoll);
        Assert.True(result.subevent.subevent.GetTags().Contains("base_healing_roll"));
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
                "bonus": {{healing}},
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, subevent.subevent.json.GetJsonArray("healing").PrettyPrint());

        subevent.SetTargets([rpglObject]);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is HealingCollection);
        Assert.True(result.subevent.subevent.GetTags().Contains("target_healing_collection"));
        Assert.False(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);
        (dependency.subevent as HealingCollection).AddHealing(new JsonObject().LoadFromString($$"""
            {
                "bonus": {{healing}},
                "dice": [ ],
                "scale": {
                    "numerator": 1,
                    "denominator": 1,
                    "round_up": false
                }
            }
            """));

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is HealingRoll);
        Assert.True(result.subevent.subevent.GetTags().Contains("target_healing_roll"));
        Assert.False(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is HealingDelivery);
        Assert.True(result.completed);
        Assert.Equal($$"""
            [
              {
                "bonus": {{healing}},
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              },
              {
                "bonus": {{healing}},
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, subevent.subevent.json.GetJsonArray("healing").PrettyPrint());
    }

};
