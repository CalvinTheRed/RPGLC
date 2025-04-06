using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class DealDamageTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "prepares")]
    public void Prepares() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        DealDamage dealDamage = new DealDamage()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "formula": "dice",
                            "damage_type": "fire",
                            "dice": [
                                { "count": 1, "size": 6, "determined": [ 3, -1 ] }
                            ]
                        },
                        {
                            "formula": "number",
                            "damage_type": "fire",
                            "number": 10
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal("""
            [
              {
                "bonus": 0,
                "damage_type": "fire",
                "dice": [
                  {
                    "determined": [
                      -1
                    ],
                    "roll": 3,
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
                "bonus": 10,
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """,
            dealDamage.json.GetJsonArray("damage").PrettyPrint());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "deals damage")]
    public void DealsDamage() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        DealDamage dealDamage = new DealDamage()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "formula": "number",
                            "damage_type": "fire",
                            "number": 10
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .SetTarget(rpglObject)
            .Invoke(new DummyContext(), new());

        Assert.Equal(1000 - 10, rpglObject.GetHealthCurrent());
    }

};
