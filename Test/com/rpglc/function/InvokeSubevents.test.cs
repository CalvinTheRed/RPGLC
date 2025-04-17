using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.function;

[Collection("Serial")]
[RPGLInitTesting]
public class InvokeSubeventsTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "invokes subevents")]
    public void InvokesSubevents() {
        RPGLObject actingObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLObject respondingObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);

        RPGLContext context = new DummyContext()
            .Add(actingObject)
            .Add(respondingObject);

        RPGLEffect rpglEffect = RPGLFactory.NewEffect("test:dummy")
            .SetSource(respondingObject.GetUuid())
            .SetTarget(respondingObject.GetUuid());

        Subevent subevent = new DummySubevent()
            .SetSource(actingObject)
            .SetTarget(respondingObject);

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "invoke_subevents",
                "subevents": [
                    {
                        "subevent": "deal_damage",
                        "damage": [
                            {
                                "formula": "number",
                                "damage_type": "fire",
                                "number": 10
                            }
                        ]
                    }
                ],
                "source": {
                    "from": "effect",
                    "object": "target",
                    "as_origin": false
                },
                "targets": [
                    {
                        "from": "subevent",
                        "object": "source",
                        "as_origin": false
                    }
                ]
            }
            """);

        new InvokeSubevents().Execute(rpglEffect, subevent, functionJson, context, new());

        Assert.Equal(1000 - 10, actingObject.GetHealthCurrent());
    }

};
