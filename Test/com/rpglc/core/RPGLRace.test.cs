using com.rpglc.json;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;

namespace com.rpglc.core;

[Collection("Serial")]
public class RPGLRaceTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [ExtraRacesMock]
    [Fact(DisplayName = "levels up RPGLObject")]
    public void LevelsUpRPGLObject() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLRace rpglRace = RPGL.GetRPGLRace("test:race_with_leveled_features");

        rpglRace.LevelUpRPGLObject(rpglObject, new JsonObject().LoadFromString("""
            {
                "Effect Choice": [ 1 ]
            }
            """), 1);

        List<RPGLEffect> effects = rpglObject.GetEffectObjects();
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
            RPGL.GetRPGLResource(resources.GetString(0)).GetDatapackId()
        );
        Assert.Equal(
            "test:dummy",
            RPGL.GetRPGLResource(resources.GetString(1)).GetDatapackId()
        );
    }

};
