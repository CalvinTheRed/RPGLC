using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
[DieTestingMode]
public class GiveEffectTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "does give effect")]
    public void DoesGiveEffect() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SubeventState subevent = new(new GiveEffect()
            .SetSource(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "effect": "test:dummy"
                }
                """)));

        var result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);
        Assert.Equal(SubeventState.Phase.Targeting, subevent.phase);
        subevent.SetTargets([rpglObject]);

        result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = true }, result);
        List<RPGLEffect> rpglEffects = RPGL.GetRPGLEffects();
        Assert.Single(rpglEffects);
        Assert.Equal(rpglObject.GetUuid(), rpglEffects[0].GetTarget());
        Assert.Equal("test:dummy", rpglEffects[0].GetDatapackId());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "does not give effect")]
    public void DoesNotGiveEffect() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLEffect rpglEffect = RPGLFactory.NewEffect("test:dummy", rpglObject.GetUuid(), rpglObject.GetUuid());
        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SubeventState subevent = new(new GiveEffect()
            .SetSource(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "effect": "test:dummy"
                }
                """)));

        var result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);
        Assert.Equal(SubeventState.Phase.Targeting, subevent.phase);
        subevent.SetTargets([rpglObject]);

        result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = true }, result);
        List<RPGLEffect> rpglEffects = RPGL.GetRPGLEffects();
        Assert.Single(rpglEffects);
        Assert.Equal(rpglEffect.GetUuid(), rpglEffects[0].GetUuid());
    }

};
