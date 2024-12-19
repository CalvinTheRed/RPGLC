using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.condition;

[Collection("Serial")]
public class CheckAbilityScoreTest {

    [Fact(DisplayName = "condition mismatch")]
    public void ConditionMismatch() {
        bool result = new CheckAbilityScore().Evaluate(new(), new DummySubevent(), new JsonObject().LoadFromString("""
            {
                "condition": "not-a-condition"
            }
            """), new DummyContext(), new());

        Assert.False(result);
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "comparison satisfied")]
    public void ComparisonSatisfied() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");

        bool result = new CheckAbilityScore().Evaluate(
            new(),
            new DummySubevent().SetSource(rpglObject),
            new JsonObject().LoadFromString("""
                {
                    "condition": "check_ability_score",
                    "object": {
                        "from": "subevent",
                        "object": "source"
                    },
                    "ability": "str",
                    "comparison": "=",
                    "compare_to": 10
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.True(result);
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "comparison not satisfied")]
    public void ComparisonNotSatisfied() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");

        bool result = new CheckAbilityScore().Evaluate(
            new(),
            new DummySubevent().SetSource(rpglObject),
            new JsonObject().LoadFromString("""
                {
                    "condition": "check_ability_score",
                    "object": {
                        "from": "subevent",
                        "object": "source"
                    },
                    "ability": "str",
                    "comparison": "!=",
                    "compare_to": 10
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.False(result);
    }

};
