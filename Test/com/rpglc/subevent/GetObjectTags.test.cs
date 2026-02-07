using com.rpglc.core;
using com.rpglc.runtime;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class GetObjectTagsTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "defaults")]
    public void Defaults() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID)
            .AddTag("test_tag") as RPGLObject;
        RPGLContext context = new DummyContext()
            .Add(rpglObject);
        SubeventState subevent = new(new GetObjectTags()
            .SetSource(rpglObject)
            .SetTarget(rpglObject));

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        List<string> events = (subevent.subevent as GetObjectTags).ObjectTags();
        Assert.Single(events);
        Assert.Equal("test_tag", events[0]);
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "adds object tag")]
    public void AddsObjectTag() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID)
            .SetTags(new()) as RPGLObject;
        RPGLContext context = new DummyContext()
            .Add(rpglObject);
        SubeventState subevent = new(new GetObjectTags()
            .SetSource(rpglObject)
            .SetTarget(rpglObject));

        var result = subevent.Advance(context);
        (subevent.subevent as GetObjectTags).AddObjectTag("test_tag");
        Assert.Equal((null, true), result);
        List<string> events = (subevent.subevent as GetObjectTags).ObjectTags();
        Assert.Single(events);
        Assert.Equal("test_tag", events[0]);
    }

};
