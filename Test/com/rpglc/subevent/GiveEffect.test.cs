using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class GiveEffectTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "gives effect")]
    public void GivesEffect() {
        RPGLObject source = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLObject target = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        GiveEffect giveEffect = new GiveEffect()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "effect": "test:dummy"
                }
                """))
            .SetSource(source)
            .Prepare(new DummyContext(), new())
            .SetTarget(target)
            .Invoke(new DummyContext(), new());

        Assert.Single(target.GetEffectObjects());
    }
};
