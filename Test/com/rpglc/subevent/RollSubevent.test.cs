using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.subevent;

[Collection("Serial")]
[DieTestingMode]
public class RollSubeventTest {

    [Fact(DisplayName = "sets defaults")]
    public void SetsDefaults() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new DummyRollSubevent());

        // skip over inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal((null, false), result);

        Assert.False(subevent.subevent.json.GetBool("has_advantage"));
        Assert.False(subevent.subevent.json.GetBool("has_disadvantage"));
        Assert.Empty(subevent.subevent.json.GetJsonArray("determined").AsList());
    }

    [Fact(DisplayName = "rolls with advantage")]
    public void RollsWithAdvantage() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new DummyRollSubevent().JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "determined": [ 5, 10, -1 ]
            }
            """)));

        // skip over inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal((null, false), result);

        (subevent.subevent as DummyRollSubevent).GrantAdvantage();

        Assert.True(subevent.subevent.json.GetBool("has_advantage"));
        Assert.False(subevent.subevent.json.GetBool("has_disadvantage"));
        Assert.True((subevent.subevent as DummyRollSubevent).IsAdvantageRoll());
        Assert.False((subevent.subevent as DummyRollSubevent).IsDisadvantageRoll());
        Assert.False((subevent.subevent as DummyRollSubevent).IsNormalRoll());

        (subevent.subevent as DummyRollSubevent).Roll();

        Assert.Equal(10L, (subevent.subevent as DummyRollSubevent).Get());
        Assert.Equal(-1L, subevent.subevent.json.GetJsonArray("determined").GetLong(0));
    }

    [Fact(DisplayName = "rolls with disadvantage")]
    public void RollsWithDisadvantage() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new DummyRollSubevent().JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "determined": [ 5, 10, -1 ]
            }
            """)));

        // skip over inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal((null, false), result);

        (subevent.subevent as DummyRollSubevent).GrantDisadvantage();

        Assert.False(subevent.subevent.json.GetBool("has_advantage"));
        Assert.True(subevent.subevent.json.GetBool("has_disadvantage"));
        Assert.False((subevent.subevent as DummyRollSubevent).IsAdvantageRoll());
        Assert.True((subevent.subevent as DummyRollSubevent).IsDisadvantageRoll());
        Assert.False((subevent.subevent as DummyRollSubevent).IsNormalRoll());

        (subevent.subevent as DummyRollSubevent).Roll();

        Assert.Equal(5L, (subevent.subevent as DummyRollSubevent).Get());
        Assert.Equal(-1L, subevent.subevent.json.GetJsonArray("determined").GetLong(0));
    }

    [Fact(DisplayName = "rolls with both advantage and disadvantage")]
    public void RollsWithBothAdvantageAndDisadvantage() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new DummyRollSubevent().JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "determined": [ 10, -1 ]
            }
            """)));

        // skip over inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal((null, false), result);

        (subevent.subevent as DummyRollSubevent).GrantAdvantage();
        (subevent.subevent as DummyRollSubevent).GrantDisadvantage();

        Assert.True(subevent.subevent.json.GetBool("has_advantage"));
        Assert.True(subevent.subevent.json.GetBool("has_disadvantage"));
        Assert.False((subevent.subevent as DummyRollSubevent).IsAdvantageRoll());
        Assert.False((subevent.subevent as DummyRollSubevent).IsDisadvantageRoll());
        Assert.True((subevent.subevent as DummyRollSubevent).IsNormalRoll());

        (subevent.subevent as DummyRollSubevent).Roll();

        Assert.Equal(10L, (subevent.subevent as DummyRollSubevent).Get());
        Assert.Equal(-1L, subevent.subevent.json.GetJsonArray("determined").GetLong(0));
    }

    [Fact(DisplayName = "rolls with neither advantage nor disadvantage")]
    public void RollsWithNeitherAdvantageNorDisadvantage() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new DummyRollSubevent().JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "determined": [ 10, -1 ]
            }
            """)));

        // skip over inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal((null, false), result);

        Assert.False(subevent.subevent.json.GetBool("has_advantage"));
        Assert.False(subevent.subevent.json.GetBool("has_disadvantage"));
        Assert.False((subevent.subevent as DummyRollSubevent).IsAdvantageRoll());
        Assert.False((subevent.subevent as DummyRollSubevent).IsDisadvantageRoll());
        Assert.True((subevent.subevent as DummyRollSubevent).IsNormalRoll());

        (subevent.subevent as DummyRollSubevent).Roll();

        Assert.Equal(10L, (subevent.subevent as DummyRollSubevent).Get());
        Assert.Equal(-1L, subevent.subevent.json.GetJsonArray("determined").GetLong(0));
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "uses proficiency (expertise)")]
    public void UsesProficiencyExpertise() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);
        SubeventState subevent = new(new DummyRollSubevent()
            .SetSource(rpglObject)
            .SetTarget(rpglObject)
            .GrantExpertise());

        // skip over inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);
        Assert.Equal(SubeventState.Phase.Targeting, subevent.phase);
        subevent.SetTargets([rpglObject]);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateProficiencyBonus);
        Assert.False(result.completed);

        result.subevent.subevent.JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "base": 2,
                "bonuses": [ ]
            }
            """));

        result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = true }, result);
        Assert.Equal("""
            [
              {
                "bonus": 2,
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 2,
                  "round_up": false
                }
              }
            ]
            """, (subevent.subevent as DummyRollSubevent).GetBonuses().PrettyPrint());
        Assert.Equal(4L, (subevent.subevent as DummyRollSubevent).Get());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "uses proficiency (proficiency)")]
    public void UsesProficiencyProficiency() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);
        SubeventState subevent = new(new DummyRollSubevent()
            .SetSource(rpglObject)
            .SetTarget(rpglObject)
            .GrantProficiency());

        // skip over inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);
        Assert.Equal(SubeventState.Phase.Targeting, subevent.phase);
        subevent.SetTargets([rpglObject]);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateProficiencyBonus);
        Assert.False(result.completed);

        result.subevent.subevent.JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "base": 2,
                "bonuses": [ ]
            }
            """));

        result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = true }, result);
        Assert.Equal("""
            [
              {
                "bonus": 2,
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent.subevent as DummyRollSubevent).GetBonuses().PrettyPrint());
        Assert.Equal(2L, (subevent.subevent as DummyRollSubevent).Get());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "uses proficiency (half proficiency)")]
    public void UsesProficiencyHalfProficiency() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);
        SubeventState subevent = new(new DummyRollSubevent()
            .SetSource(rpglObject)
            .SetTarget(rpglObject)
            .GrantHalfProficiency());

        // skip over inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);
        Assert.Equal(SubeventState.Phase.Targeting, subevent.phase);
        subevent.SetTargets([rpglObject]);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateProficiencyBonus);
        Assert.False(result.completed);

        result.subevent.subevent.JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "base": 2,
                "bonuses": [ ]
            }
            """));

        result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = true }, result);
        Assert.Equal("""
            [
              {
                "bonus": 2,
                "dice": [ ],
                "scale": {
                  "denominator": 2,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent.subevent as DummyRollSubevent).GetBonuses().PrettyPrint());
        Assert.Equal(1L, (subevent.subevent as DummyRollSubevent).Get());
    }

};
