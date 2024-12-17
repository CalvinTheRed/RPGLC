using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[AssignDatabase]
[Collection("Serial")]
public class TemporaryHitPointRollTest {

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares default")]
    public void PreparesDefault() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        TemporaryHitPointRoll temporaryHitPointRoll = new TemporaryHitPointRoll()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal("""[]""", temporaryHitPointRoll.GetTemporaryHitPoints().ToString());
    }

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "prepares temporary hit points")]
    public void PreparesTemporaryHitPoints() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        TemporaryHitPointRoll temporaryHitPointRoll = new TemporaryHitPointRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "temporary_hit_points": [
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
            temporaryHitPointRoll.GetTemporaryHitPoints().PrettyPrint()
        );
    }

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "rerolls temporary hit point dice")]
    public void RerollsTemporaryHitPointDice() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        TemporaryHitPointRoll temporaryHitPointRoll = new TemporaryHitPointRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "temporary_hit_points": [
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

        temporaryHitPointRoll.RerollTemporaryHitPointDice(2, 5);

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
            temporaryHitPointRoll.GetTemporaryHitPoints().PrettyPrint()
        );
    }

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "sets temporary hit point dice")]
    public void SetsTemporaryHitPointDice() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        TemporaryHitPointRoll temporaryHitPointRoll = new TemporaryHitPointRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "temporary_hit_points": [
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

        temporaryHitPointRoll.SetTemporaryHitPointDice(6, 2, 5);

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
            temporaryHitPointRoll.GetTemporaryHitPoints().PrettyPrint()
        );
    }

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "maximizes temporary hit point dice")]
    public void MaximizesTemporaryHitPointDice() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        TemporaryHitPointRoll temporaryHitPointRoll = new TemporaryHitPointRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "temporary_hit_points": [
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

        temporaryHitPointRoll.MaximizeTemporaryHitPointDice();

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
            temporaryHitPointRoll.GetTemporaryHitPoints().PrettyPrint()
        );
    }

};
