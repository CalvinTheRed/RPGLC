using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.function;

[Collection("Serial")]
public class AddVampirismTest {

    [Fact(DisplayName = "adds vampirism")]
    public void AddsVampirism() {
        Subevent subevent = new DummyVampiricSubevent()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "attack_ability": "str",
                    "attack_type": "melee"
                }
                """))
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

    [Fact(DisplayName = "adds default vampirism array")]
    public void AddsDefaultVampirismArray() {
        Subevent subevent = new DummyVampiricSubevent()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "attack_ability": "str",
                    "attack_type": "melee"
                }
                """))
            .Prepare(new DummyContext(), new());

        new AddVampirism().Execute(
            new RPGLEffect(),
            subevent,
            new JsonObject().LoadFromString("""
                {
                    "function": "add_vampirism"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.Equal("""
            [
              { }
            ]
            """,
            IVampiricSubevent.GetVampirism(subevent).PrettyPrint()
        );
    }

    [Fact(DisplayName = "adds default vampirism object")]
    public void AddsDefaultVampirismObject() {
        Subevent subevent = new DummyVampiricSubevent()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "attack_ability": "str",
                    "attack_type": "melee"
                }
                """))
            .Prepare(new DummyContext(), new());

        new AddVampirism().Execute(
            new RPGLEffect(),
            subevent,
            new JsonObject().LoadFromString("""
                {
                    "function": "add_vampirism",
                    "vampirism": [ ]
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.Equal("""
            [
              { }
            ]
            """,
            IVampiricSubevent.GetVampirism(subevent).PrettyPrint()
        );
    }

};
