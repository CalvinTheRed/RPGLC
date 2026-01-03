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
        DummyFunction dummyFunction = new DummyFunction();
        FunctionState functionState = new(null, new DummySubevent(), dummyFunction, new(), new DummyContext());

        Assert.Null(functionState.AdvanceState());
        Assert.Equal(1, DummyFunction.Counter);

        Assert.Null(functionState.AdvanceState());
        Assert.Equal(2, DummyFunction.Counter);

        Assert.Null(functionState.AdvanceState());
        Assert.Equal(3, DummyFunction.Counter);
    }

};
