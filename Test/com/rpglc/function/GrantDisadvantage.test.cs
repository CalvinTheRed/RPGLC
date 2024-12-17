using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.function;

[AssignDatabase]
[Collection("Serial")]
public class GrantDisadvantageTest {

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "grants disadvantage")]
    public void GrantsDisadvantage() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        Subevent subevent = new DummyRollSubevent()
            .SetSource(rpglObject)
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
