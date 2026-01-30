using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
[DieTestingMode]
public class TemporaryHitPointRollTest {

    [Fact(DisplayName = "defaults")]
    public void Defaults() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new TemporaryHitPointRoll());

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Empty((subevent.subevent as TemporaryHitPointRoll).GetTemporaryHitPoints().AsList());
    }

    [Fact(DisplayName = "rolls temporary hit points")]
    public void RollsTemporaryHitPoints() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new TemporaryHitPointRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "temporary_hit_points": [
                        {
                            "bonus": 1,
                            "dice": [
                                { "size": 6, "determined": [ 3, -1 ] }
                            ]
                        }
                    ]
                }
                """)));

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Equal("""
            [
              {
                "bonus": 1,
                "dice": [
                  {
                    "determined": [
                      -1
                    ],
                    "roll": 3,
                    "size": 6
                  }
                ]
              }
            ]
            """, (subevent.subevent as TemporaryHitPointRoll).GetTemporaryHitPoints().PrettyPrint());
    }

    [Fact(DisplayName = "rerolls temporary hit point dice")]
    public void RerollsTemporaryHitPointDice() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new TemporaryHitPointRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "temporary_hit_points": [
                        {
                            "bonus": 1,
                            "dice": [
                                { "size": 6, "determined": [ 3, 6, -1 ] },
                                { "size": 6, "determined": [ 3, 6, -1 ] }
                            ]
                        }
                    ]
                }
                """)));

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);

        (subevent.subevent as TemporaryHitPointRoll).RerollTemporaryHitPointDice(1, 6);

        Assert.Equal("""
            [
              {
                "bonus": 1,
                "dice": [
                  {
                    "determined": [
                      -1
                    ],
                    "roll": 6,
                    "size": 6
                  },
                  {
                    "determined": [
                      -1
                    ],
                    "roll": 6,
                    "size": 6
                  }
                ]
              }
            ]
            """, (subevent.subevent as TemporaryHitPointRoll).GetTemporaryHitPoints().PrettyPrint());
    }

    [Fact(DisplayName = "maximizes temporary hit point dice")]
    public void MaximizesTemporaryHitPointDice() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new TemporaryHitPointRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "temporary_hit_points": [
                        {
                            "bonus": 1,
                            "dice": [
                                { "size": 6, "determined": [ 3, -1 ] },
                                { "size": 6, "determined": [ 3, -1 ] }
                            ]
                        }
                    ]
                }
                """)));

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);

        (subevent.subevent as TemporaryHitPointRoll).MaximizeTemporaryHitPointDice();

        Assert.Equal("""
            [
              {
                "bonus": 1,
                "dice": [
                  {
                    "determined": [
                      -1
                    ],
                    "roll": 6,
                    "size": 6
                  },
                  {
                    "determined": [
                      -1
                    ],
                    "roll": 6,
                    "size": 6
                  }
                ]
              }
            ]
            """, (subevent.subevent as TemporaryHitPointRoll).GetTemporaryHitPoints().PrettyPrint());
    }

};
