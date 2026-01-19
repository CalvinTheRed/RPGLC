using com.rpglc.condition;
using com.rpglc.core;
using com.rpglc.subevent;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.condition;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.runtime;

[Collection("Serial")]
public class ConditionStateTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DummyCounterManager]
    [Fact(DisplayName = "advances condition state")]
    public void AdvancesConditionState() {
        Condition condition = new True().Clone();
        ConditionState conditionState = new(condition, new());
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent();
        RPGLContext context = new DummyContext();

        Assert.Equal((null, null, true), conditionState.Advance(rpglEffect, subevent, context));
    }

};
