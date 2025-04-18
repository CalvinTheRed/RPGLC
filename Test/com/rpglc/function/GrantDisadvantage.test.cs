using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.function;

[Collection("Serial")]
public class GrantDisadvantageTest {

    [Fact(DisplayName = "grants disadvantage")]
    public void GrantsDisadvantage() {
        Subevent subevent = new DummyRollSubevent()
            .Prepare(new DummyContext(), new());

        new GrantDisadvantage().Execute(
            new RPGLEffect(),
            subevent,
            new JsonObject().LoadFromString("""
                {
                    "function": "grant_disadvantage"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.True(subevent.json.GetBool("has_disadvantage"));
    }

};
