using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;
using com.rpglc.testutils;
using com.rpglc.testutils.subevent;
using com.rpglc.subevent;

namespace com.rpglc.condition;

[Collection("Serial")]
public class CheckSkillTest {

    [Fact(DisplayName = "condition mismatch")]
    public void ConditionMismatch() {
        bool result = new CheckSkill().Evaluate(new(), new DummySubevent(), new JsonObject().LoadFromString("""
            {
                "condition": "not-a-condition"
            }
            """), new DummyContext(), new());

        Assert.False(result);
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "check uses same skill")]
    public void CheckUsesSameSkill() {
        bool result = new CheckSkill().Evaluate(
            new(),
            new AbilityCheck().JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "skill": "athletics"
                }
                """)),
            new JsonObject().LoadFromString("""
                {
                    "condition": "check_skill",
                    "skill": "athletics"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.True(result);
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "check uses different skill")]
    public void CheckUsesDifferentSkill() {
        bool result = new CheckSkill().Evaluate(
            new(),
            new AbilityCheck().JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "skill": "not-athletics"
                }
                """)),
            new JsonObject().LoadFromString("""
                {
                    "condition": "check_skill",
                    "skill": "athletics"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.False(result);
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "check uses no skill")]
    public void CheckUsesNoSkill() {
        bool result = new CheckSkill().Evaluate(
            new(),
            new AbilityCheck(),
            new JsonObject().LoadFromString("""
                {
                    "condition": "check_skill",
                    "skill": "athletics"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.False(result);
    }

};
