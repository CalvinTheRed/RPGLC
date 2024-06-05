using com.rpglc.database;
using com.rpglc.json;
using com.rpglc.testutils;
using com.rpglc.core;
using com.rpglc.testutils.mocks;

namespace com.rpglc.core;

[AssignDatabase]
[Collection("Serial")]
public class FeatureManagerTest {

    private readonly JsonObject features = new JsonObject().LoadFromString("""
        {
            "gain": {
                "effects": [
                    "test:dummy",
                    {
                        "name": "Gained Effect",
                        "count": 1,
                        "options": [
                            "does-not-exist",
                            "test:dummy",
                            "does-not-exist"
                        ]
                    }
                ],
                "events": [
                    "test:dummy",
                    "test:dummy"
                ],
                "resources": [
                    "test:dummy",
                    {
                        "resource": "test:dummy",
                        "count": 1
                    }
                ]
            },
            "lose": {
                "effects": [
                    "test:dummy",
                    "test:dummy"
                ],
                "events": [
                    "test:dummy",
                    "test:dummy"
                ],
                "resources": [
                    "test:dummy",
                    {
                        "resource": "test:dummy",
                        "count": 1
                    }
                ]
            }
        }
        """);

    private readonly JsonObject choices = new JsonObject().LoadFromString("""
        {
            "Gained Effect": [ 1 ]
        }
        """);

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "gains and loses effects")]
    public void GainsAndLosesEffects() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        
        // gains effects
        FeatureManager.GrantGainedEffects(
            rpglObject,
            features.GetJsonObject("gain"),
            choices
        );
        List<RPGLEffect> effects = DBManager.QueryRPGLEffects(x => 
            x.Target == rpglObject.GetUuid()
        );
        Assert.Equal(2, effects.Count);
        Assert.Equal("test:dummy", effects[0].GetDatapackId());
        Assert.Equal("test:dummy", effects[1].GetDatapackId());

        // loses effects
        FeatureManager.RevokeLostEffects(
            rpglObject,
            features.GetJsonObject("lose")
        );
        effects = DBManager.QueryRPGLEffects(x =>
            x.Target == rpglObject.GetUuid()
        );
        Assert.Equal(0, effects.Count);
    }

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "gains and loses events")]
    public void GainsAndLosesEvents() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");

        // gains events
        FeatureManager.GrantGainedEvents(
            rpglObject,
            features.GetJsonObject("gain")
        );
        Assert.NotNull(rpglObject);
        JsonArray events = rpglObject.GetEvents();
        Assert.Equal(2, events.Count());
        Assert.Equal("test:dummy", events.GetString(0));
        Assert.Equal("test:dummy", events.GetString(1));

        // loses events
        FeatureManager.RevokeLostEvents(
            rpglObject,
            features.GetJsonObject("lose")
        );
        Assert.NotNull(rpglObject);
        events = rpglObject.GetEvents();
        Assert.Equal(0, events.Count());
    }

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "gains and loses resources")]
    public void GainsAndLosesResources() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");

        // gains resources
        FeatureManager.GrantGainedResources(
            rpglObject,
            features.GetJsonObject("gain")
        );
        rpglObject = DBManager.QueryRPGLObject(x => x.UserId == "Player 1");
        Assert.NotNull(rpglObject);
        JsonArray resources = rpglObject.GetResources();
        Assert.Equal(2, resources.Count());
        string datapackId = DBManager.QueryRPGLResource(x => x.Uuid == resources.GetString(0)).GetDatapackId();
        Assert.Equal("test:dummy", datapackId);
        datapackId = DBManager.QueryRPGLResource(x => x.Uuid == resources.GetString(1)).GetDatapackId();
        Assert.Equal("test:dummy", datapackId);

        // loses resources
        FeatureManager.RevokeLostResources(
            rpglObject,
            features.GetJsonObject("lose")
        );
        Assert.NotNull(rpglObject);
        resources = rpglObject.GetResources();
        Assert.Equal(0, resources.Count());
    }

};
