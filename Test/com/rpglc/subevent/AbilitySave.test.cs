using com.rpglc.core;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.core;
using com.rpglc.testutils;
using com.rpglc.json;
using com.rpglc.testutils.subevent;

namespace com.rpglc.subevent;

[Collection("Serial")]
[DieTestingMode]
[RPGLInitTesting]
public class AbilitySaveTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares set difficulty")]
    public void PreparesSetDifficulty() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        AbilitySave abilitySave = new AbilitySave()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "ability": "str",
                    "skill": "athletics",
                    "difficulty_class": 15
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.False(abilitySave.json.GetBool("use_origin_difficulty_class_ability"));
        Assert.Equal(15, abilitySave.json.GetLong("difficulty_class"));
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares calculated difficulty")]
    public void PreparesCalculatedDifficulty() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.InsertLong("ability_scores.str", 12L);
        AbilitySave abilitySave = new AbilitySave()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "ability": "str",
                    "skill": "athletics",
                    "difficulty_class_ability": "str"
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.False(abilitySave.json.GetBool("use_origin_difficulty_class_ability"));
        Assert.Equal(8 + 1 + 2, abilitySave.json.GetLong("difficulty_class"));
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares calculated difficulty as origin")]
    public void PreparesCalculatedDifficultyAsOrigin() {
        RPGLObject originObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        originObject.InsertLong("ability_scores.str", 12L);
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID)
            .SetOriginObject(originObject.GetUuid());
        AbilitySave abilitySave = new AbilitySave()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "ability": "str",
                    "skill": "athletics",
                    "difficulty_class_ability": "str",
                    "use_origin_difficulty_class_ability": true
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.True(abilitySave.json.GetBool("use_origin_difficulty_class_ability"));
        Assert.Equal(8 + 1 + 2, abilitySave.json.GetLong("difficulty_class"));
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DummyCounterManager]
    [Fact(DisplayName = "passes")]
    public void Passes() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        _ = new AbilitySave()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "ability": "str",
                    "skill": "athletics",
                    "difficulty_class": 15,
                    "determined": [ 20 ],
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
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .SetTarget(rpglObject)
            .Invoke(new DummyContext(), new());

        Assert.Equal(1, DummySubevent.Counter);
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DummyCounterManager]
    [Fact(DisplayName = "fails")]
    public void Fails() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        _ = new AbilitySave()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "ability": "str",
                    "skill": "athletics",
                    "difficulty_class": 15,
                    "determined": [ 1 ],
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
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .SetTarget(rpglObject)
            .Invoke(new DummyContext(), new());

        Assert.Equal(1, DummySubevent.Counter);
    }
}
