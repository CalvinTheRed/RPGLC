using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class HealingRollTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares default")]
    public void PreparesDefault() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        HealingRoll healingRoll = new HealingRoll()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal("""[]""", healingRoll.GetHealing().ToString());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "prepares healing")]
    public void PreparesHealing() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        HealingRoll healingRoll = new HealingRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "healing": [
                        {
                            "bonus": 1,
                            "dice": [
                                { "size": 6, "determined": [ 3 ] }
                            ]
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal("""
            [
              {
                "bonus": 1,
                "dice": [
                  {
                    "determined": [ ],
                    "roll": 3,
                    "size": 6
                  }
                ]
              }
            ]
            """,
            healingRoll.GetHealing().PrettyPrint()
        );
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "rerolls healing dice")]
    public void RerollsHealingDice() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        HealingRoll healingRoll = new HealingRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "healing": [
                        {
                            "bonus": 1,
                            "dice": [
                                { "size": 6, "determined": [ 1, -1 ] },
                                { "size": 6, "determined": [ 3, 4, -1 ] },
                                { "size": 6, "determined": [ 6, -1 ] }
                            ]
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        healingRoll.RerollHealingDice(2, 5);

        Assert.Equal("""
            [
              {
                "bonus": 1,
                "dice": [
                  {
                    "determined": [
                      -1
                    ],
                    "roll": 1,
                    "size": 6
                  },
                  {
                    "determined": [
                      -1
                    ],
                    "roll": 4,
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
            """,
            healingRoll.GetHealing().PrettyPrint()
        );
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "sets healing dice")]
    public void SetsHealingDice() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        HealingRoll healingRoll = new HealingRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "healing": [
                        {
                            "bonus": 1,
                            "dice": [
                                { "size": 6, "determined": [ 1, -1 ] },
                                { "size": 6, "determined": [ 3, -1 ] },
                                { "size": 6, "determined": [ 6, -1 ] }
                            ]
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        healingRoll.SetHealingDice(6, 2, 5);

        Assert.Equal("""
            [
              {
                "bonus": 1,
                "dice": [
                  {
                    "determined": [
                      -1
                    ],
                    "roll": 1,
                    "size": 6
                  },
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
            """,
            healingRoll.GetHealing().PrettyPrint()
        );
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "maximizes healing dice")]
    public void MaximizesHealingDice() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        HealingRoll healingRoll = new HealingRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "healing": [
                        {
                            "bonus": 1,
                            "dice": [
                                { "size": 6, "determined": [ 1, -1 ] }
                            ]
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        healingRoll.MaximizeHealingDice();

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
                  }
                ]
              }
            ]
            """,
            healingRoll.GetHealing().PrettyPrint()
        );
    }

};
