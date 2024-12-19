using com.rpglc.core;
using com.rpglc.json;
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
    [Fact(DisplayName = "handles vampirism")]
    public void HandlesVampirism() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID)
            .SetHealthCurrent(0);

        DummyVampiricSubevent dummyVampiricSubevent = new DummyVampiricSubevent()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "vampirism": [
                        {
                            "damage_type": "necrotic",
                            "scale": {
                                "numerator": 1,
                                "denominator": 2,
                                "round_up": false
                            }
                        },
                        {
                            "scale": {
                                "numerator": 1,
                                "denominator": 1,
                                "round_up": false
                            }
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .SetTarget(rpglObject);
        
        IVampiricSubevent.HandleVampirism(
            dummyVampiricSubevent,
            new JsonObject().LoadFromString("""
                {
                    "slashing": 10,
                    "necrotic": 10
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.Equal(5 + 20, rpglObject.GetHealthCurrent());
    }

};
