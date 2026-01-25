using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class CalculateDifficultyClassTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "uses generated difficulty class")]
    public void UsesGeneratedDifficultyClass() {
        long wisScore = 12L;
        long proficiencyBonus = 2;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.GetAbilityScores().PutLong("wis", wisScore);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);
        SubeventState subevent = new(new CalculateDifficultyClass()
            .SetSource(rpglObject)
            .SetTarget(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "difficulty_class_ability": "wis"
                }
                """)));

        // skip over inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        // picks correct fork
        _ = subevent.Advance(context);
        Assert.Equal(8, subevent.subevent.subeventSteps.Count);

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateAbilityScore);
        Assert.False(result.completed);

        (result.subevent.subevent as CalculateAbilityScore)
            .SetBase(wisScore)
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "bonuses": [ ]
                }
                """));

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateProficiencyBonus);
        Assert.False(result.completed);
        Assert.Equal(8L + 1L, (subevent.subevent as CalculateDifficultyClass).Get());

        (result.subevent.subevent as CalculateProficiencyBonus)
            .SetBase(proficiencyBonus)
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "bonuses": [ ]
                }
                """));

        result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Equal(8L + 1L + 2L, (subevent.subevent as CalculateDifficultyClass).Get());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "uses assigned difficulty class")]
    public void UsesAssignedDifficultyClass() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new CalculateDifficultyClass()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "difficulty_class": 15
                }
                """)));

        // skip over inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        // picks correct fork
        _ = subevent.Advance(context);
        Assert.Equal(6, subevent.subevent.subeventSteps.Count);

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Equal(15L, (subevent.subevent as CalculateDifficultyClass).Get());
    }

};
