using com.rpglc.core;
using com.rpglc.subevent;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;
using com.rpglc.testutils.function;
using com.rpglc.testutils.subevent;
using static com.rpglc.runtime.RPGLEffectState;

namespace com.rpglc.runtime;

[Collection("Serial")]
public class RPGLEffectStateTest {

    [ClearRPGLAfterTest]
    [ExtraEffectsMock]
    [RPGLInitTesting]
    [Fact(DisplayName = "constructs")]
    public void Constructs() {
        RPGLEffectState rpglEffectState = new(RPGLFactory.NewEffect("test:complex_effect"));
        Assert.Single(rpglEffectState.subeventFilters);
        List<SubeventFilterBehavior> subeventFilter = rpglEffectState.subeventFilters["dummy_subevent"];
        Assert.Single(subeventFilter);
        SubeventFilterBehavior subeventFilterBehavior = subeventFilter[0];
        Assert.Single(subeventFilterBehavior.conditions);
        ConditionState conditionState = subeventFilterBehavior.conditions.Peek();
        Assert.Equal("all", conditionState.conditionJson.GetString("condition"));
        Assert.Single(subeventFilterBehavior.functions);
        FunctionState functionState = subeventFilterBehavior.functions.Peek();
        Assert.Equal("dummy_function", functionState.functionJson.GetString("function"));
    }

    [ClearRPGLAfterTest]
    [DummyCounterManager]
    [ExtraEffectsMock]
    [RPGLInitTesting]
    [Fact(DisplayName = "resolves without input")]
    public void ResolvesWithoutInput() {
        RPGLEffectState rpglEffectState = new(RPGLFactory.NewEffect("test:complex_effect"));
        Subevent subevent = new DummySubevent();
        RPGLContext context = new DummyContext();

        // push true to top of conditions stack
        var result = rpglEffectState.Advance(subevent, context);
        Assert.Equal(RPGLEffectState.Phase.EvaluatingConditions, rpglEffectState.phase);
        Assert.Equal((null, true), result);
        Assert.Equal("true", rpglEffectState
            .subeventFilters[subevent.GetSubeventId()][rpglEffectState.subeventFilterIndex]
            .conditions
            .Peek()
            .conditionJson
            .GetString("condition")
        );

        // complete true and fall back to all
        result = rpglEffectState.Advance(subevent, context);
        Assert.Equal(RPGLEffectState.Phase.EvaluatingConditions, rpglEffectState.phase);
        Assert.Equal((null, true), result);
        Assert.Equal("all", rpglEffectState
            .subeventFilters[subevent.GetSubeventId()][rpglEffectState.subeventFilterIndex]
            .conditions
            .Peek()
            .conditionJson
            .GetString("condition")
        );

        // complete first step of all
        result = rpglEffectState.Advance(subevent, context);
        Assert.Equal(RPGLEffectState.Phase.EvaluatingConditions, rpglEffectState.phase);
        Assert.Equal((null, true), result);
        Assert.Equal("all", rpglEffectState
            .subeventFilters[subevent.GetSubeventId()][rpglEffectState.subeventFilterIndex]
            .conditions
            .Peek()
            .conditionJson
            .GetString("condition")
        );

        // push next true to top of conditions stack
        result = rpglEffectState.Advance(subevent, context);
        Assert.Equal(RPGLEffectState.Phase.EvaluatingConditions, rpglEffectState.phase);
        Assert.Equal((null, true), result);
        Assert.Equal("true", rpglEffectState
            .subeventFilters[subevent.GetSubeventId()][rpglEffectState.subeventFilterIndex]
            .conditions
            .Peek()
            .conditionJson
            .GetString("condition")
        );

        // complete second true and fall back to all
        result = rpglEffectState.Advance(subevent, context);
        Assert.Equal(RPGLEffectState.Phase.EvaluatingConditions, rpglEffectState.phase);
        Assert.Equal((null, true), result);
        Assert.Equal("all", rpglEffectState
            .subeventFilters[subevent.GetSubeventId()][rpglEffectState.subeventFilterIndex]
            .conditions
            .Peek()
            .conditionJson
            .GetString("condition")
        );

        // complete second step of all and proceed to function execution
        result = rpglEffectState.Advance(subevent, context);
        Assert.Equal(RPGLEffectState.Phase.ExecutingFunctions, rpglEffectState.phase);
        Assert.Equal((null, true), result);
        Assert.Equal("dummy_function", rpglEffectState
            .subeventFilters[subevent.GetSubeventId()][rpglEffectState.subeventFilterIndex]
            .functions
            .Peek()
            .functionJson
            .GetString("function")
        );
        Assert.Equal(0, DummyFunction.Counter);

        // first step of function
        result = rpglEffectState.Advance(subevent, context);
        Assert.Equal(RPGLEffectState.Phase.ExecutingFunctions, rpglEffectState.phase);
        Assert.Equal((null, true), result);
        Assert.Equal("dummy_function", rpglEffectState
            .subeventFilters[subevent.GetSubeventId()][rpglEffectState.subeventFilterIndex]
            .functions
            .Peek()
            .functionJson
            .GetString("function")
        );
        Assert.Equal(1, DummyFunction.Counter);

        // second step of function
        result = rpglEffectState.Advance(subevent, context);
        Assert.Equal(RPGLEffectState.Phase.ExecutingFunctions, rpglEffectState.phase);
        Assert.Equal((null, true), result);
        Assert.Equal("dummy_function", rpglEffectState
            .subeventFilters[subevent.GetSubeventId()][rpglEffectState.subeventFilterIndex]
            .functions
            .Peek()
            .functionJson
            .GetString("function")
        );
        Assert.Equal(2, DummyFunction.Counter);

        // third step of function
        result = rpglEffectState.Advance(subevent, context);
        Assert.Equal(RPGLEffectState.Phase.ExecutingFunctions, rpglEffectState.phase);
        Assert.Equal((null, true), result);
        Assert.Empty(rpglEffectState
            .subeventFilters[subevent.GetSubeventId()][rpglEffectState.subeventFilterIndex]
            .functions
        );
        Assert.Equal(3, DummyFunction.Counter);

        // effect is completed
        result = rpglEffectState.Advance(subevent, context);
        Assert.Equal(RPGLEffectState.Phase.Completed, rpglEffectState.phase);
        Assert.Equal((null, true), result);
    }

    [ClearRPGLAfterTest]
    [DummyCounterManager]
    [ExtraEffectsMock]
    [RPGLInitTesting]
    [Fact(DisplayName = "does resolve with input")]
    public void DoesResolveWithInput() {
        RPGLEffectState rpglEffectState = new(RPGLFactory.NewEffect("test:complex_effect").SetOptional(true));
        Subevent subevent = new DummySubevent();
        RPGLContext context = new DummyContext();

        // push true to top of conditions stack
        var result = rpglEffectState.Advance(subevent, context);
        Assert.Equal(RPGLEffectState.Phase.EvaluatingConditions, rpglEffectState.phase);
        Assert.Equal((null, true), result);
        Assert.Equal("true", rpglEffectState
            .subeventFilters[subevent.GetSubeventId()][rpglEffectState.subeventFilterIndex]
            .conditions
            .Peek()
            .conditionJson
            .GetString("condition")
        );

        // complete true and fall back to all
        result = rpglEffectState.Advance(subevent, context);
        Assert.Equal(RPGLEffectState.Phase.EvaluatingConditions, rpglEffectState.phase);
        Assert.Equal((null, true), result);
        Assert.Equal("all", rpglEffectState
            .subeventFilters[subevent.GetSubeventId()][rpglEffectState.subeventFilterIndex]
            .conditions
            .Peek()
            .conditionJson
            .GetString("condition")
        );

        // complete first step of all
        result = rpglEffectState.Advance(subevent, context);
        Assert.Equal(RPGLEffectState.Phase.EvaluatingConditions, rpglEffectState.phase);
        Assert.Equal((null, true), result);
        Assert.Equal("all", rpglEffectState
            .subeventFilters[subevent.GetSubeventId()][rpglEffectState.subeventFilterIndex]
            .conditions
            .Peek()
            .conditionJson
            .GetString("condition")
        );

        // push next true to top of conditions stack
        result = rpglEffectState.Advance(subevent, context);
        Assert.Equal(RPGLEffectState.Phase.EvaluatingConditions, rpglEffectState.phase);
        Assert.Equal((null, true), result);
        Assert.Equal("true", rpglEffectState
            .subeventFilters[subevent.GetSubeventId()][rpglEffectState.subeventFilterIndex]
            .conditions
            .Peek()
            .conditionJson
            .GetString("condition")
        );

        // complete second true and fall back to all
        result = rpglEffectState.Advance(subevent, context);
        Assert.Equal(RPGLEffectState.Phase.EvaluatingConditions, rpglEffectState.phase);
        Assert.Equal((null, true), result);
        Assert.Equal("all", rpglEffectState
            .subeventFilters[subevent.GetSubeventId()][rpglEffectState.subeventFilterIndex]
            .conditions
            .Peek()
            .conditionJson
            .GetString("condition")
        );

        // complete second step of all and proceed to user input
        result = rpglEffectState.Advance(subevent, context);
        Assert.Equal(RPGLEffectState.Phase.RequestingUserInput, rpglEffectState.phase);
        Assert.Equal((null, false), result);

        // fail to advance without user input
        result = rpglEffectState.Advance(subevent, context);
        Assert.Equal(RPGLEffectState.Phase.RequestingUserInput, rpglEffectState.phase);
        Assert.Equal((null, false), result);

        // provide user input
        rpglEffectState.SetUserInput(true);

        // proceed into first step of function
        result = rpglEffectState.Advance(subevent, context);
        Assert.Equal(RPGLEffectState.Phase.ExecutingFunctions, rpglEffectState.phase);
        Assert.Equal((null, true), result);
        Assert.Equal("dummy_function", rpglEffectState
            .subeventFilters[subevent.GetSubeventId()][rpglEffectState.subeventFilterIndex]
            .functions
            .Peek()
            .functionJson
            .GetString("function")
        );
        Assert.Equal(1, DummyFunction.Counter);

        // second step of function
        result = rpglEffectState.Advance(subevent, context);
        Assert.Equal(RPGLEffectState.Phase.ExecutingFunctions, rpglEffectState.phase);
        Assert.Equal((null, true), result);
        Assert.Equal("dummy_function", rpglEffectState
            .subeventFilters[subevent.GetSubeventId()][rpglEffectState.subeventFilterIndex]
            .functions
            .Peek()
            .functionJson
            .GetString("function")
        );
        Assert.Equal(2, DummyFunction.Counter);

        // third step of function
        result = rpglEffectState.Advance(subevent, context);
        Assert.Equal(RPGLEffectState.Phase.ExecutingFunctions, rpglEffectState.phase);
        Assert.Equal((null, true), result);
        Assert.Empty(rpglEffectState
            .subeventFilters[subevent.GetSubeventId()][rpglEffectState.subeventFilterIndex]
            .functions
        );
        Assert.Equal(3, DummyFunction.Counter);

        // effect is completed
        result = rpglEffectState.Advance(subevent, context);
        Assert.Equal(RPGLEffectState.Phase.Completed, rpglEffectState.phase);
        Assert.Equal((null, true), result);
    }

    [ClearRPGLAfterTest]
    [DummyCounterManager]
    [ExtraEffectsMock]
    [RPGLInitTesting]
    [Fact(DisplayName = "does not resolve with input")]
    public void DoesNotResolveWithInput() {
        RPGLEffectState rpglEffectState = new(RPGLFactory.NewEffect("test:complex_effect").SetOptional(true));
        Subevent subevent = new DummySubevent();
        RPGLContext context = new DummyContext();

        // push true to top of conditions stack
        var result = rpglEffectState.Advance(subevent, context);
        Assert.Equal(RPGLEffectState.Phase.EvaluatingConditions, rpglEffectState.phase);
        Assert.Equal((null, true), result);
        Assert.Equal("true", rpglEffectState
            .subeventFilters[subevent.GetSubeventId()][rpglEffectState.subeventFilterIndex]
            .conditions
            .Peek()
            .conditionJson
            .GetString("condition")
        );

        // complete true and fall back to all
        result = rpglEffectState.Advance(subevent, context);
        Assert.Equal(RPGLEffectState.Phase.EvaluatingConditions, rpglEffectState.phase);
        Assert.Equal((null, true), result);
        Assert.Equal("all", rpglEffectState
            .subeventFilters[subevent.GetSubeventId()][rpglEffectState.subeventFilterIndex]
            .conditions
            .Peek()
            .conditionJson
            .GetString("condition")
        );

        // complete first step of all
        result = rpglEffectState.Advance(subevent, context);
        Assert.Equal(RPGLEffectState.Phase.EvaluatingConditions, rpglEffectState.phase);
        Assert.Equal((null, true), result);
        Assert.Equal("all", rpglEffectState
            .subeventFilters[subevent.GetSubeventId()][rpglEffectState.subeventFilterIndex]
            .conditions
            .Peek()
            .conditionJson
            .GetString("condition")
        );

        // push next true to top of conditions stack
        result = rpglEffectState.Advance(subevent, context);
        Assert.Equal(RPGLEffectState.Phase.EvaluatingConditions, rpglEffectState.phase);
        Assert.Equal((null, true), result);
        Assert.Equal("true", rpglEffectState
            .subeventFilters[subevent.GetSubeventId()][rpglEffectState.subeventFilterIndex]
            .conditions
            .Peek()
            .conditionJson
            .GetString("condition")
        );

        // complete second true and fall back to all
        result = rpglEffectState.Advance(subevent, context);
        Assert.Equal(RPGLEffectState.Phase.EvaluatingConditions, rpglEffectState.phase);
        Assert.Equal((null, true), result);
        Assert.Equal("all", rpglEffectState
            .subeventFilters[subevent.GetSubeventId()][rpglEffectState.subeventFilterIndex]
            .conditions
            .Peek()
            .conditionJson
            .GetString("condition")
        );

        // complete second step of all and proceed to user input
        result = rpglEffectState.Advance(subevent, context);
        Assert.Equal(RPGLEffectState.Phase.RequestingUserInput, rpglEffectState.phase);
        Assert.Equal((null, false), result);

        // fail to advance without user input
        result = rpglEffectState.Advance(subevent, context);
        Assert.Equal(RPGLEffectState.Phase.RequestingUserInput, rpglEffectState.phase);
        Assert.Equal((null, false), result);

        // provide user input
        rpglEffectState.SetUserInput(false);

        // complete effect due to user refusing to apply effect
        result = rpglEffectState.Advance(subevent, context);
        Assert.Equal(RPGLEffectState.Phase.Completed, rpglEffectState.phase);
        Assert.Equal((null, true), result);
    }

};
