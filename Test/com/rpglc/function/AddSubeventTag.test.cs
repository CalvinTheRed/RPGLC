using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.function;

[Collection("Serial")]
public class AddSubeventTagTest {

    [Fact(DisplayName = "adds tag")]
    public void AddsTag() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummySubevent();

        AddSubeventTag function = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "add_subevent_tag",
                "tag": "test_tag"
            }
            """);

        FunctionState.StateData result;

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal("""["dummy_subevent","test_tag"]""", subevent.GetTags().ToString());
    }

};
