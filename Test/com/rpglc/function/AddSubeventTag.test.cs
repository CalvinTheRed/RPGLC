using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.function;

[Collection("Serial")]
public class AddSubeventTagTest {

    [Fact(DisplayName = "adds tag")]
    public void AddsBonus() {
        Subevent subevent = new DummySubevent()
            .Prepare(new DummyContext(), new());

        new AddSubeventTag().Execute(
            new RPGLEffect(),
            subevent,
            new JsonObject().LoadFromString("""
                {
                    "function": "add_subevent_tag",
                    "tag": "test_tag"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.True(subevent.HasTag("test_tag"));
    }

};
