using com.rpglc.database;
using com.rpglc.json;
using com.rpglc.testutils;
using com.rpglc.testutils.mocks;

namespace com.rpglc.core;

[AssignDatabase]
[Collection("Serial")]
public class RPGLRaceTest {

    [DefaultMock]
    [ExtraRacesMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "levels up RPGLObject")]
    public void LevelsUpRPGLObject() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        RPGLRace rpglRace = RPGLFactory.GetRace("test:race_with_leveled_features");

        rpglRace.LevelUpRPGLObject(rpglObject, new JsonObject().LoadFromString("""
            {
                "Effect Choice": [ 1 ]
            }
            """), 1);

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

};
