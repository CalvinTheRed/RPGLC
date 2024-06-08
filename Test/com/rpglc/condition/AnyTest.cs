using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils;

namespace com.rpglc.condition;

[InitializeConditions]
[Collection("Serial")]
public class AnyTest {

    [Fact(DisplayName = "all true")]
    public void AllTrue() {
        bool result = new Any().Run(new(), new DummySubevent(), new JsonObject().LoadFromString("""
            {
                "condition": "any",
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
        bool result = new Any().Run(new(), new DummySubevent(), new JsonObject().LoadFromString("""
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

        Assert.True(result);
    }

    [Fact(DisplayName = "none true")]
    public void NoneTrue() {
        bool result = new Any().Run(new(), new DummySubevent(), new JsonObject().LoadFromString("""
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
        bool result = new Any().Run(new(), new DummySubevent(), new JsonObject().LoadFromString("""
            {
                "condition": "any",
                "conditions": [ ]
            }
            """), new DummyContext(), new());

        Assert.False(result);
    }

};
