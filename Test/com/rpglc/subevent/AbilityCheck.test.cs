using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
[DieTestingMode]
public class AbilityCheckTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares")]
    public void Prepares() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        AbilityCheck abilityCheck = new AbilityCheck()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.False(abilityCheck.json.GetBool("has_expertise"));
        Assert.False(abilityCheck.json.GetBool("has_proficiency"));
        Assert.False(abilityCheck.json.GetBool("has_half_proficiency"));

        Assert.False(abilityCheck.HasExpertise());
        Assert.False(abilityCheck.HasProficiency());
        Assert.False(abilityCheck.HasHalfProficiency());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "grants expertise")]
    public void GrantsExpertise() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        AbilityCheck abilityCheck = new AbilityCheck()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .GrantHalfProficiency()
            .GrantProficiency()
            .GrantExpertise();

        Assert.True(abilityCheck.HasExpertise());
        Assert.False(abilityCheck.HasProficiency());
        Assert.False(abilityCheck.HasHalfProficiency());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "grants proficiency")]
    public void GrantsProficiency() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        AbilityCheck abilityCheck = new AbilityCheck()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .GrantHalfProficiency()
            .GrantProficiency();

        Assert.False(abilityCheck.HasExpertise());
        Assert.True(abilityCheck.HasProficiency());
        Assert.False(abilityCheck.HasHalfProficiency());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "grants half proficiency")]
    public void GrantsHalfProficiency() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        AbilityCheck abilityCheck = new AbilityCheck()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .GrantHalfProficiency();

        Assert.False(abilityCheck.HasExpertise());
        Assert.False(abilityCheck.HasProficiency());
        Assert.True(abilityCheck.HasHalfProficiency());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "runs without proficiency")]
    public void RunsWithoutProficiency() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.InsertLong("ability_scores.str", 12);
        AbilityCheck abilityCheck = new AbilityCheck()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "ability": "str",
                    "determined": [ 10 ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .SetTarget(rpglObject)
            .Invoke(new DummyContext(), new());

        Assert.Equal(10+1, abilityCheck.Get());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "runs with expertise")]
    public void RunsWithExpertise() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.InsertLong("ability_scores.str", 12);
        AbilityCheck abilityCheck = new AbilityCheck()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "ability": "str",
                    "determined": [ 10 ],
                    "has_expertise": true,
                    "has_proficiency": true,
                    "has_half_proficiency": true
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .SetTarget(rpglObject)
            .Invoke(new DummyContext(), new());

        Assert.Equal(10 + 1 + (2 * 2), abilityCheck.Get());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "runs with proficiency")]
    public void RunsWithProficiency() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.InsertLong("ability_scores.str", 12);
        AbilityCheck abilityCheck = new AbilityCheck()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "ability": "str",
                    "determined": [ 10 ],
                    "has_proficiency": true,
                    "has_half_proficiency": true
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .SetTarget(rpglObject)
            .Invoke(new DummyContext(), new());

        Assert.Equal(10 + 1 + (1 * 2), abilityCheck.Get());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "runs with half proficiency")]
    public void RunsWithHalfProficiency() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.InsertLong("ability_scores.str", 12);
        AbilityCheck abilityCheck = new AbilityCheck()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "ability": "str",
                    "determined": [ 10 ],
                    "has_half_proficiency": true
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .SetTarget(rpglObject)
            .Invoke(new DummyContext(), new());

        Assert.Equal(10 + 1 + (0.5 * 2), abilityCheck.Get());
    }

}