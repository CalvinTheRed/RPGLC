using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class AddObjectTagTest {

    [Fact(DisplayName = "adds object tag")]
    public void AddsObjectTag() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new GetObjectTags().JoinSubeventData(new JsonObject().LoadFromString(
            """{ "object_tags": [ ] }"""
        ));

        AddObjectTag addObjectTag = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "add_object_tag",
                "tag": "test_tag"
            }
            """);

        FunctionState.StateData result;

        result = addObjectTag.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        List<string> objectTags = (subevent as GetObjectTags).ObjectTags();
        Assert.Single(objectTags);
        Assert.Contains("test_tag", objectTags);
    }

};
