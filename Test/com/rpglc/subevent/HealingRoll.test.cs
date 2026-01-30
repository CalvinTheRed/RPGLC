using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
[DieTestingMode]
public class HealingRollTest {

    [Fact(DisplayName = "defaults")]
    public void Defaults() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new HealingRoll());

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Empty((subevent.subevent as HealingRoll).GetHealing().AsList());
    }

    [Fact(DisplayName = "rolls healing")]
    public void RollsHealing() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new HealingRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "healing": [
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
            """, (subevent.subevent as HealingRoll).GetHealing().PrettyPrint());
    }

    [Fact(DisplayName = "rerolls healing dice")]
    public void RerollsHealingDice() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new HealingRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "healing": [
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

        (subevent.subevent as HealingRoll).RerollHealingDice(1, 6);

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
            """, (subevent.subevent as HealingRoll).GetHealing().PrettyPrint());
    }

    [Fact(DisplayName = "maximizes healing dice")]
    public void MaximizesHealingDice() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new HealingRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "healing": [
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

        (subevent.subevent as HealingRoll).MaximizeHealingDice();

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
            """, (subevent.subevent as HealingRoll).GetHealing().PrettyPrint());
    }

};
