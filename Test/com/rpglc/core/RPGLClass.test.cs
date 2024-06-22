using com.rpglc.database;
using com.rpglc.json;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;

namespace com.rpglc.core;

[AssignDatabase]
[Collection("Serial")]
public class RPGLClassTest {

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [ExtraClassesMock]
    [Fact(DisplayName = "grants starting features")]
    public void GrantsStartingFeatures() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        RPGLClass rpglClass = RPGLFactory.GetClass("test:class_with_starting_features");

        rpglClass.GrantStartingFeatures(rpglObject, new JsonObject().LoadFromString("""
            {
                "Effect Choice": [ 1 ]
            }
            """));
        rpglObject = DBManager.QueryRPGLObject(x => x.UserId == "Player 1");
        Assert.NotNull(rpglObject);

        List<RPGLEffect> effects = DBManager.QueryRPGLEffects(x => x.Target == rpglObject.GetUuid());
        Assert.Equal(2, effects.Count);
        Assert.Equal("test:dummy", effects[0].GetDatapackId());
        Assert.Equal("test:dummy", effects[1].GetDatapackId());

        JsonArray events = rpglObject.GetEvents();
        Assert.Equal(1, events.Count());
        Assert.Equal("test:dummy", events.GetString(0));

        JsonArray resources = rpglObject.GetResources();
        Assert.Equal(2, resources.Count());
        Assert.Equal(
            "test:dummy",
            DBManager.QueryRPGLResource(x => x.Uuid == resources.GetString(0)).GetDatapackId()
        );
        Assert.Equal(
            "test:dummy",
            DBManager.QueryRPGLResource(x => x.Uuid == resources.GetString(1)).GetDatapackId()
        );
    }

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [ExtraClassesMock]
    [Fact(DisplayName = "levels up RPGLObject")]
    public void LevelsUpRPGLObject() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        RPGLClass rpglClass = RPGLFactory.GetClass("test:class_with_leveled_features");

        Assert.Equal(0, rpglObject.GetLevel("test:class_with_leveled_features"));
        rpglClass.LevelUpRPGLObject(rpglObject, new JsonObject().LoadFromString("""
            {
                "Effect Choice": [ 1 ]
            }
            """));
        Assert.Equal(1, rpglObject.GetLevel("test:class_with_leveled_features"));
        Assert.Equal(
            """[{"additional_nested_classes":{},"id":"test:class_with_leveled_features","level":1,"name":"Class With Leveled Features"}]""",
            rpglObject.GetClasses().ToString()
        );
    }

};
