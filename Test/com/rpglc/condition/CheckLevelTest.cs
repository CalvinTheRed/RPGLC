using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils;
using com.rpglc.testutils.mocks;

namespace com.rpglc.condition;

[AssignDatabase]
[Collection("Serial")]
public class CheckLevelTest {

    [Fact(DisplayName = "condition mismatch")]
    public void ConditionMismatch() {
        bool result = new CheckLevel().Evaluate(new(), new DummySubevent(), new JsonObject().LoadFromString("""
            {
                "condition": "not-a-condition"
            }
            """), new DummyContext(), new());

        Assert.False(result);
    }

    [DefaultMock]
    [ExtraClassesMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "object level satisfied")]
    public void ObjectLevelSatisfied() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1")
            .LevelUp("test:dummy", new())
            .LevelUp("test:nested_class", new());

        bool result = new CheckLevel().Evaluate(
            new(),
            new DummySubevent().SetSource(rpglObject),
            new JsonObject().LoadFromString("""
                {
                    "condition": "check_level",
                    "object": {
                        "from": "subevent",
                        "object": "source"
                    },
                    "comparison": "=",
                    "compare_to": 2
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.True(result);
    }

    [DefaultMock]
    [ExtraClassesMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "object level not satisfied")]
    public void ObjectLevelNotSatisfied() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1")
            .LevelUp("test:dummy", new())
            .LevelUp("test:nested_class", new());

        bool result = new CheckLevel().Evaluate(
            new(),
            new DummySubevent().SetSource(rpglObject),
            new JsonObject().LoadFromString("""
                {
                    "condition": "check_level",
                    "object": {
                        "from": "subevent",
                        "object": "source"
                    },
                    "comparison": "!=",
                    "compare_to": 2
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.False(result);
    }

    [DefaultMock]
    [ExtraClassesMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "class level satisfied")]
    public void ClassLevelSatisfied() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1")
            .LevelUp("test:dummy", new())
            .LevelUp("test:nested_class", new());

        bool result = new CheckLevel().Evaluate(
            new(),
            new DummySubevent().SetSource(rpglObject),
            new JsonObject().LoadFromString("""
                {
                    "condition": "check_level",
                    "object": {
                        "from": "subevent",
                        "object": "source"
                    },
                    "class": "test:dummy",
                    "comparison": "=",
                    "compare_to": 1
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.True(result);
    }

    [DefaultMock]
    [ExtraClassesMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "class level not satisfied")]
    public void ClassLevelNotSatisfied() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1")
            .LevelUp("test:dummy", new())
            .LevelUp("test:nested_class", new());

        bool result = new CheckLevel().Evaluate(
            new(),
            new DummySubevent().SetSource(rpglObject),
            new JsonObject().LoadFromString("""
                {
                    "condition": "check_level",
                    "object": {
                        "from": "subevent",
                        "object": "source"
                    },
                    "class": "test:dummy",
                    "comparison": "!=",
                    "compare_to": 1
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.False(result);
    }

};
