using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class CalculateMaximumHitPointsTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "calculates maximum hit points")]
    public void CalculatesMaximumHitPoints() {
        long healthBase = 10L;
        long conScore = 12L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.SetHealthBase(healthBase);
        rpglObject.GetAbilityScores().PutLong("con", conScore);
        rpglObject.SetClasses(new JsonArray().LoadFromString("""
            [
                {
                  "additional_nested_classes": { },
                  "id": "test:dummy",
                  "level": 1,
                  "name": "Dummy"
                }
            ]
            """));
        RPGLContext context = new DummyContext()
            .Add(rpglObject);
        SubeventState subevent = new(new CalculateMaximumHitPoints()
            .SetSource(rpglObject)
            .SetTarget(rpglObject));

        // skip over inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateAbilityScore);
        Assert.False(result.completed);

        (result.subevent.subevent as CalculateAbilityScore)
            .SetBase(conScore);

        result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Equal(healthBase + 1L, (subevent.subevent as CalculateMaximumHitPoints).Get());
    }

};
