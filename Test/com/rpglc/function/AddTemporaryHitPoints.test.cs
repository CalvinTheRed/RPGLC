using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class AddTemporaryHitPointsTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "adds temporary hit points")]
    public void AddsTemporaryHitPoints() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        Subevent subevent = new TemporaryHitPointCollection()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        new AddTemporaryHitPoints().Execute(
            new RPGLEffect(),
            subevent,
            new JsonObject().LoadFromString("""
                {
                    "function": "add_temporary_hit_points",
                    "temporary_hit_points": [
                        {
                            "formula": "dice",
                            "dice": [
                                { "count": 1, "size": 6, "determined": [ 3 ] }
                            ]
                        },
                        {
                            "formula": "number",
                            "number": 1
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
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent as TemporaryHitPointCollection).GetTemporaryHitPointCollection().PrettyPrint());
    }

};
