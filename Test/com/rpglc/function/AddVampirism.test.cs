using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.function;

[AssignDatabase]
[Collection("Serial")]
public class AddVampirismTest {

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "adds vampirism")]
    public void AddsBonus() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        Subevent subevent = new DummyVampiricSubevent()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "attack_ability": "str",
                    "attack_type": "melee"
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        new AddVampirism().Execute(
            new RPGLEffect(),
            subevent,
            new JsonObject().LoadFromString("""
                {
                    "function": "add_vampirism",
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
                """),
            new DummyContext(),
            new()
        );

        Assert.Equal("""
            [
              {
                "damage_type": "necrotic",
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """,
            IVampiricSubevent.GetVampirism(subevent).PrettyPrint()
        );
    }

};
