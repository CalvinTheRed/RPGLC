using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.core;

namespace com.rpglc.condition;

[Collection("Serial")]
[RPGLCInit]
public class AllTest {

    [Fact(DisplayName = "condition mismatch")]
    public void ConditionMismatch() {
        bool result = new All().Evaluate(new(), new DummySubevent(), new JsonObject().LoadFromString("""
            {
                "condition": "not-a-condition"
            }
            """), new DummyContext(), new());

        Assert.False(result);
    }

    [Fact(DisplayName = "all true")]
    public void AllTrue() {
        bool result = new All().Evaluate(new(), new DummySubevent(), new JsonObject().LoadFromString("""
            {
                "condition": "all",
                "conditions": [
                    {
                        "condition": "true"
                    },
                    {
                        "condition": "true"
                    }
                ]
            }
            """), new DummyContext(), new());

        Assert.True(result);
    }

    [Fact(DisplayName = "some true")]
    public void SomeTrue() {
        bool result = new All().Evaluate(new(), new DummySubevent(), new JsonObject().LoadFromString("""
            {
                "condition": "any",
                "conditions": [
                    {
                        "condition": "true"
                    },
                    {
                        "condition": "false"
                    }
                ]
            }
            """), new DummyContext(), new());

        Assert.False(result);
    }

    [Fact(DisplayName = "none true")]
    public void NoneTrue() {
        bool result = new All().Evaluate(new(), new DummySubevent(), new JsonObject().LoadFromString("""
            {
                "condition": "any",
                "conditions": [
                    {
                        "condition": "false"
                    },
                    {
                        "condition": "false"
                    }
                ]
            }
            """), new DummyContext(), new());

        Assert.False(result);
    }

    [Fact(DisplayName = "no nested conditions")]
    public void NoNestedConditions() {
        bool result = new All().Evaluate(new(), new DummySubevent(), new JsonObject().LoadFromString("""
            {
                "condition": "all",
                "conditions": [ ]
            }
            """), new DummyContext(), new());

        Assert.False(result);
    }

};
