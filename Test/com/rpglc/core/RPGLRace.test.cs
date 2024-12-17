using com.rpglc.json;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;

namespace com.rpglc.core;

[AssignDatabase]
[Collection("Serial")]
public class RPGLRaceTest {

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [ExtraRacesMock]
    [Fact(DisplayName = "levels up RPGLObject")]
    public void LevelsUpRPGLObject() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        RPGLRace rpglRace = RPGLFactory.GetRace("test:race_with_leveled_features");

        rpglRace.LevelUpRPGLObject(rpglObject, new JsonObject().LoadFromString("""
            {
                "Effect Choice": [ 1 ]
            }
            """), 1);

        List<RPGLEffect> effects = RPGL.GetRPGLEffects().FindAll(x => x.GetTarget() == rpglObject.GetUuid());
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
            RPGL.GetRPGLResources().Find(x => x.GetUuid() == resources.GetString(0)).GetDatapackId()
        );
        Assert.Equal(
            "test:dummy",
            RPGL.GetRPGLResources().Find(x => x.GetUuid() == resources.GetString(1)).GetDatapackId()
        );
    }

};
