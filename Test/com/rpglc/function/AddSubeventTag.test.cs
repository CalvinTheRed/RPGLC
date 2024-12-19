using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class AddSubeventTagTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "adds tag")]
    public void AddsBonus() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        Subevent subevent = new DummySubevent()
            .SetSource(rpglObject)
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
