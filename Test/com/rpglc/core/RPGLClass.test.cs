using com.rpglc.json;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;

namespace com.rpglc.core;

[Collection("Serial")]
public class RPGLClassTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [ExtraClassesMock]
    [Fact(DisplayName = "grants starting features")]
    public void GrantsStartingFeatures() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLClass rpglClass = RPGLFactory.GetClass("test:class_with_starting_features");

        rpglClass.GrantStartingFeatures(rpglObject, new JsonObject().LoadFromString("""
            {
                "Effect Choice": [ 1 ]
            }
            """));

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

    [ClearRPGLAfterTest]
    [DefaultMock]
    [ExtraClassesMock]
    [Fact(DisplayName = "levels up RPGLObject")]
    public void LevelsUpRPGLObject() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
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
