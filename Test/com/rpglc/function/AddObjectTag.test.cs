using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class AddObjectTagTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "adds object tag")]
    public void AddsObjectTag() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        Subevent subevent = new GetObjectTags()
            .SetSource(rpglObject)
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
