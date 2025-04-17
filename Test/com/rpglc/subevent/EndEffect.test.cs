using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class EndEffectTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [ExtraClassesMock]
    [ExtraEffectsMock]
    [ExtraObjectsMock]
    [Fact(DisplayName = "prepares")]
    public void Prepares() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);

        EndEffect endEffect = new EndEffect()
            .Prepare(new DummyContext(), new());

        Assert.Equal("*", endEffect.json.GetString("effect"));
        Assert.Equal("{ }", endEffect.json.GetJsonObject("effect_source").PrettyPrint());
        Assert.Equal("[ ]", endEffect.json.GetJsonArray("effect_tags").PrettyPrint());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [ExtraClassesMock]
    [ExtraEffectsMock]
    [ExtraObjectsMock]
    [Fact(DisplayName = "ends effect by id")]
    public void EndsEffectById() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:complex_object", TestUtils.USER_ID);

        Assert.True(rpglObject.GetEffectObjects().Any(e => e.GetDatapackId() == "test:dummy"));

        EndEffect endEffect = new EndEffect()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "effect": "test:dummy"
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .SetTarget(rpglObject)
            .Invoke(new DummyContext(), new());

        Assert.False(rpglObject.GetEffectObjects().Any(e => e.GetDatapackId() == "test:dummy"));
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [ExtraClassesMock]
    [ExtraEffectsMock]
    [ExtraObjectsMock]
    [Fact(DisplayName = "ends effect by tag")]
    public void EndsEffectByTag() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:complex_object", TestUtils.USER_ID);

        Assert.True(rpglObject.GetEffectObjects().Any(e => e.GetDatapackId() == "test:complex_effect"));

        EndEffect endEffect = new EndEffect()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "effect_tags": [
                        "test_tag"
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .SetTarget(rpglObject)
            .Invoke(new DummyContext(), new());

        Assert.False(rpglObject.GetEffectObjects().Any(e => e.GetDatapackId() == "test:complex_effect"));
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "ends effect by source")]
    public void EndsEffectBySource() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLEffect rpglEffect = RPGLFactory.NewEffect("test:dummy")
            .SetSource(rpglObject.GetUuid());
        rpglObject.AddEffect(rpglEffect);

        Assert.True(rpglObject.GetEffectObjects().Any(e => e.GetDatapackId() == "test:dummy"));

        EndEffect endEffect = new EndEffect()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "effect_source": {
                        "from": "subevent",
                        "object": "source",
                        "as_origin": false
                    }
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .SetTarget(rpglObject)
            .Invoke(new DummyContext(), new());

        Assert.False(rpglObject.GetEffectObjects().Any(e => e.GetDatapackId() == "test:dummy"));
    }
};
