using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class CalculateArmorClassTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "calculates default")]
    public void CalculatesDefault() {
        long dexScore = 12L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.GetAbilityScores().PutLong("dex", dexScore);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);
        SubeventState subevent = new(new CalculateArmorClass()
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
            .SetBase(dexScore);

        result = subevent.Advance(context);
        Assert.Equal((null, true), result);

        Assert.Equal(11L, (subevent.subevent as CalculateArmorClass).Get());
    }

};
