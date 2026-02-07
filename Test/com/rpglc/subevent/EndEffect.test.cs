using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class EndEffectTest {

    [Fact(DisplayName = "defaults")]
    public void Defaults() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new EndEffect());

        var result = subevent.Advance(context);
        Assert.Equal((null, false), result);
        Assert.Equal("*", subevent.subevent.json.GetString("effect"));
        Assert.Equal("[ ]", subevent.subevent.json.GetJsonArray("effect_tags").PrettyPrint());
        Assert.Equal("{ }", subevent.subevent.json.GetJsonObject("effect_source").PrettyPrint());
        Assert.Equal("{ }", subevent.subevent.json.GetJsonObject("effect_target").PrettyPrint());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "ends effect by id")]
    public void EndsEffectById() {
        RPGLEffect rpglEffect = RPGLFactory.NewEffect("test:dummy");
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID)
            .AddEffect(rpglEffect);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);
        SubeventState subevent = new(new EndEffect()
            .SetSource(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "effect": "test:dummy"
                }
                """)));

        var result = subevent.Advance(context);
        Assert.Equal((null, false), result);
        Assert.Equal("test:dummy", subevent.subevent.json.GetString("effect"));
        Assert.Equal(SubeventState.Phase.Targeting, subevent.phase);
        subevent.SetTargets([rpglObject]);

        result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Empty(RPGL.GetRPGLEffects());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "ends effect by tag")]
    public void EndsEffectByTag() {
        RPGLEffect rpglEffect = (RPGLEffect) RPGLFactory.NewEffect("test:dummy")
            .AddTag("test_tag");
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID)
            .AddEffect(rpglEffect);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);
        SubeventState subevent = new(new EndEffect()
            .SetSource(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "effect_tags": [
                        "test_tag"
                    ]
                }
                """)));

        var result = subevent.Advance(context);
        Assert.Equal((null, false), result);
        Assert.True(subevent.subevent.json.GetJsonArray("effect_tags").Contains("test_tag"));
        Assert.Equal(SubeventState.Phase.Targeting, subevent.phase);
        subevent.SetTargets([rpglObject]);

        result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Empty(RPGL.GetRPGLEffects());
    }
    // TODO add tests for ending effect by effect source
};
