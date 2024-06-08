using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils;

namespace com.rpglc.condition;

[InitializeConditions]
public class InvertTest {

    [Fact(DisplayName = "inverts true")]
    public void InvertsTrue() {
        bool result = new Invert().Run(new(), new DummySubevent(), new JsonObject().LoadFromString("""
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
        bool result = new Invert().Run(new(), new DummySubevent(), new JsonObject().LoadFromString("""
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
