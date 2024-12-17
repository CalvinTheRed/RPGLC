using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[AssignDatabase]
[Collection("Serial")]
[RPGLInitTesting]
public class SavingThrowTest {

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares generated difficulty class")]
    public void PreparesGeneratedDifficultyClass() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");

        SavingThrow savingThrow = new SavingThrow()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "save_ability": "dex",
                    "difficulty_class_ability": "int"
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal("""[]""", savingThrow.json.GetJsonArray("damage").ToString());
        Assert.False(savingThrow.json.GetBool("use_origin_difficulty_class_ability"));
        Assert.Equal(8 + 2 + 0, savingThrow.GetDifficultyClass());
    }

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares assigned difficulty class")]
    public void PreparesAssignedDifficultyClass() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");

        SavingThrow savingThrow = new SavingThrow()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "save_ability": "dex",
                    "difficulty_class": 15
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal("""[]""", savingThrow.json.GetJsonArray("damage").ToString());
        Assert.False(savingThrow.json.GetBool("use_origin_difficulty_class_ability"));
        Assert.Equal(15, savingThrow.GetDifficultyClass());
    }

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares origin difficulty class")]
    public void PreparesOriginDifficultyClass() {
        RPGLObject originObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        originObject.GetAbilityScores().PutLong("int", 20L);

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1")
            .SetOriginObject(originObject.GetUuid());

        SavingThrow savingThrow = new SavingThrow()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "save_ability": "dex",
                    "difficulty_class_ability": "int",
                    "use_origin_difficulty_class_ability": true
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal("""[]""", savingThrow.json.GetJsonArray("damage").ToString());
        Assert.Equal(8 + 2 + 5, savingThrow.GetDifficultyClass());
    }

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "passes with half damage")]
    [DummyCounterManager]
    public void PassesWithHalfDamage() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");

        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SavingThrow savingThrow = new SavingThrow()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "save_ability": "dex",
                    "difficulty_class_ability": "int",
                    "damage": [
                        {
                            "damage_type": "fire",
                            "formula": "range",
                            "bonus": 10,
                            "dice": [ ]
                        }
                    ],
                    "damage_on_pass": "half",
                    "pass": [
                        {
                            "subevent": "dummy_subevent"
                        },
                        {
                            "subevent": "dummy_subevent"
                        }
                    ],
                    "fail": [
                        {
                            "subevent": "dummy_subevent"
                        }
                    ],
                    "determined": [ 20 ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(context, new())
            .SetTarget(rpglObject)
            .Invoke(context, new());

        Assert.Equal(2, DummySubevent.Counter);
        Assert.Equal(1000 - 5, rpglObject.GetHealthCurrent());
    }

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "passes with no damage")]
    [DummyCounterManager]
    public void PassesWithNoDamage() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");

        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SavingThrow savingThrow = new SavingThrow()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "save_ability": "dex",
                    "difficulty_class_ability": "int",
                    "damage": [
                        {
                            "damage_type": "fire",
                            "formula": "range",
                            "bonus": 10,
                            "dice": [ ]
                        }
                    ],
                    "damage_on_pass": "none",
                    "pass": [
                        {
                            "subevent": "dummy_subevent"
                        },
                        {
                            "subevent": "dummy_subevent"
                        }
                    ],
                    "fail": [
                        {
                            "subevent": "dummy_subevent"
                        }
                    ],
                    "determined": [ 20 ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(context, new())
            .SetTarget(rpglObject)
            .Invoke(context, new());

        Assert.Equal(2, DummySubevent.Counter);
        Assert.Equal(1000 - 0, rpglObject.GetHealthCurrent());
    }

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [Fact(DisplayName = "fails")]
    [DummyCounterManager]
    public void Fails() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");

        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SavingThrow savingThrow = new SavingThrow()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "save_ability": "dex",
                    "difficulty_class_ability": "int",
                    "damage": [
                        {
                            "damage_type": "fire",
                            "formula": "range",
                            "bonus": 10,
                            "dice": [ ]
                        }
                    ],
                    "pass": [
                        {
                            "subevent": "dummy_subevent"
                        }
                    ],
                    "fail": [
                        {
                            "subevent": "dummy_subevent"
                        },
                        {
                            "subevent": "dummy_subevent"
                        }
                    ],
                    "determined": [ 1 ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(context, new())
            .SetTarget(rpglObject)
            .Invoke(context, new());

        Assert.Equal(2, DummySubevent.Counter);
        Assert.Equal(1000 - 10, rpglObject.GetHealthCurrent());
    }

};
