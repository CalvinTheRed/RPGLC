using com.rpglc.core;
using com.rpglc.function;
using com.rpglc.subevent;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;
using com.rpglc.testutils.function;
using com.rpglc.testutils.subevent;

namespace com.rpglc.runtime;

[Collection("Serial")]
public class FunctionStateTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DummyCounterManager]
    [Fact(DisplayName = "advances function state")]
    public void AdvancesFunctionState() {
        Function function = new DummyFunction().Clone();
        FunctionState functionState = new(function, new());
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent();
        RPGLContext context = new DummyContext();

        Assert.Equal((null, false), functionState.AdvanceState(rpglEffect, subevent, context));
        Assert.Equal(1, DummyFunction.Counter);

        Assert.Equal((null, false), functionState.AdvanceState(rpglEffect, subevent, context));
        Assert.Equal(2, DummyFunction.Counter);

        Assert.Equal((null, true), functionState.AdvanceState(rpglEffect, subevent, context));
        Assert.Equal(3, DummyFunction.Counter);
    }

};
