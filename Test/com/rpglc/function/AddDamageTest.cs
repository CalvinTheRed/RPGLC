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
public class AddDamageTest {

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "adds damage")]
    public void AddsDamage() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        Subevent subevent = new DamageCollection()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        new AddDamage().Execute(
            new RPGLEffect(),
            subevent,
            new JsonObject().LoadFromString("""
                {
                    "function": "add_damage",
                    "damage": [
                        {
                            "damage_type": "fire",
                            "formula": "range",
                            "bonus": 0,
                            "dice": [
                                { "count": 1, "size": 6, "determined": [ 3 ] }
                            ]
                        },
                        {
                            "damage_type": "fire",
                            "formula": "range",
                            "bonus": 1,
                            "dice": [ ]
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
                "bonus": 0,
                "damage_type": "fire",
                "dice": [
                  {
                    "determined": [
                      3
                    ],
                    "size": 6
                  }
                ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              },
              {
                "bonus": 1,
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as DamageCollection).GetDamageCollection().PrettyPrint());
    }

};
