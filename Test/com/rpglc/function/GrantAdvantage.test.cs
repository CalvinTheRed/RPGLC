using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.function;

[Collection("Serial")]
public class GrantAdvantageTest {

    [Fact(DisplayName = "grants advantage")]
    public void GrantsAdvantage() {
        Subevent subevent = new DummyRollSubevent()
            .Prepare(new DummyContext(), new());

        new GrantAdvantage().Execute(
            new RPGLEffect(),
            subevent,
            new JsonObject().LoadFromString("""
                {
                    "function": "grant_advantage"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.True(subevent.json.GetBool("has_advantage"));
    }

};
