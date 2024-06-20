using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils;

namespace com.rpglc.condition;

[RPGLCInit]
[Collection("Serial")]
public class InvertTest {

    [Fact(DisplayName = "condition mismatch")]
    public void ConditionMismatch() {
        bool result = new Invert().Evaluate(new(), new DummySubevent(), new JsonObject().LoadFromString("""
            {
                "condition": "not-a-condition"
            }
            """), new DummyContext(), new());

        Assert.False(result);
    }

    [Fact(DisplayName = "inverts true")]
    public void InvertsTrue() {
        bool result = new Invert().Evaluate(new(), new DummySubevent(), new JsonObject().LoadFromString("""
            {
                "condition": "invert",
                "invert": {
                    "condition": "true"
                }
            }
            """), new DummyContext(), new());

        Assert.False(result);
    }

    [Fact(DisplayName = "inverts false")]
    public void InvertsFalse() {
        bool result = new Invert().Evaluate(new(), new DummySubevent(), new JsonObject().LoadFromString("""
            {
                "condition": "invert",
                "invert": {
                    "condition": "false"
                }
            }
            """), new DummyContext(), new());

        Assert.True(result);
    }

};
