using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class VampiricSubeventTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "heals from vampiric damage")]
    public void HealsFromVampiricDamage() {
        long damage = 10L;

        RPGLObject source = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID)
            .SetHealthCurrent(0L);
        RPGLObject target = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext()
            .Add(source)
            .Add(target);

        SubeventState subevent = new(new DummyVampiricSubevent()
            .SetSource(source)
            .SetTarget(target)
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "vampirism": [
                        {
                            "damage_type": "necrotic",
                            "scale": {
                                "numerator": 1,
                                "denominator": 1,
                                "round_up": false
                            }
                        }
                    ]
                }
                """)));

        var result = subevent.Advance(context);
        (result.subevent.subevent as DamageDelivery).JoinSubeventData(new JsonObject().LoadFromString($$"""
            {
                "damage": {
                    "necrotic": {{damage}},
                    "fire": 100
                }
            }
            """));

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is HealingCollection);
        Assert.False(result.completed);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is HealingRoll);
        Assert.False(result.completed);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is HealingDelivery);
        Assert.True(result.completed);
        Assert.Equal(damage, (result.subevent.subevent as HealingDelivery).GetHealing());
    }

};
