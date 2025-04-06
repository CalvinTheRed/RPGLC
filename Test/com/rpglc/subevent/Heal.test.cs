using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class HealTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "prepares")]
    public void Prepares() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        Heal heal = new Heal()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "healing": [
                        {
                            "formula": "number",
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
                "bonus": 10,
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """,
            heal.json.GetJsonArray("healing").PrettyPrint());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "heals")]
    public void Heals() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID)
            .SetHealthCurrent(0L);

        Heal heal = new Heal()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "healing": [
                        {
                            "formula": "number",
                            "number": 10
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .SetTarget(rpglObject)
            .Invoke(new DummyContext(), new());

        Assert.Equal(0 + 10, rpglObject.GetHealthCurrent());
    }

};
