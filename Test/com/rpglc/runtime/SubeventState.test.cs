using com.rpglc.core;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.runtime;

[Collection("Serial")]
public class SubeventStateTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DummyCounterManager]
    [RPGLInitTesting]
    [Fact(DisplayName = "constructs")]
    public void Constructs() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new DummySubevent().SetSource(rpglObject));

        var result = subevent.Advance(context);
        Assert.Equal((null, false), result);
        Assert.Equal(1, subevent.stepIndex);
        Assert.Equal(1, DummySubevent.Counter);

        result = subevent.Advance(context);
        Assert.Equal((null, false), result);
        Assert.Equal(2, subevent.stepIndex);
        Assert.Equal(2, DummySubevent.Counter);
        Assert.Equal(SubeventState.Phase.Targeting, subevent.phase);

        // cannot advance without targets being assigned
        result = subevent.Advance(context);
        Assert.Equal((null, false), result);

        // set targets
        subevent.SetTargets([rpglObject]);

        // spawn subevent clone per target
        result = subevent.Advance(context);
        Assert.Equal("dummy_subevent", result.subevent.subevent.subeventId);
        Assert.Equal(rpglObject, result.subevent.subevent.GetTarget());
        Assert.Equal(SubeventState.Phase.Running, result.subevent.phase);
        Assert.True(result.completed);
    }

};
