using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class AddObjectTagTest {

    [Fact(DisplayName = "adds object tag")]
    public void AddsObjectTag() {
        Subevent subevent = new GetObjectTags()
            .Prepare(new DummyContext(), new());

        new AddObjectTag().Execute(
            new RPGLEffect(),
            subevent,
            new JsonObject().LoadFromString("""
                {
                    "function": "add_object_tag",
                    "tag": "test_tag"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.Contains("test_tag", (subevent as GetObjectTags).ObjectTags());
    }

};
