using com.rpglc.json;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;

namespace com.rpglc.core;

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

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "gains and loses effects")]
    public void GainsAndLosesEffects() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        List<RPGLEffect> effects = rpglObject.GetEffectObjects();

        // gains effects
        FeatureManager.GrantGainedEffects(
            rpglObject,
            features.GetJsonObject("gain"),
            choices
        );
        effects = rpglObject.GetEffectObjects();
        Assert.Equal(2, effects.Count);
        Assert.Equal("test:dummy", effects[0].GetDatapackId());
        Assert.Equal("test:dummy", effects[1].GetDatapackId());

        // loses effects
        FeatureManager.RevokeLostEffects(
            rpglObject,
            features.GetJsonObject("lose")
        );
        effects = rpglObject.GetEffectObjects();
        Assert.Empty(effects);
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "gains and loses events")]
    public void GainsAndLosesEvents() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);

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

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "gains and loses resources")]
    public void GainsAndLosesResources() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);

        // gains resources
        FeatureManager.GrantGainedResources(
            rpglObject,
            features.GetJsonObject("gain")
        );
        JsonArray resources = rpglObject.GetResources();
        Assert.Equal(2, resources.Count());
        string datapackId = RPGL.GetRPGLResource(resources.GetString(0)).GetDatapackId();
        Assert.Equal("test:dummy", datapackId);
        datapackId = RPGL.GetRPGLResource(resources.GetString(1)).GetDatapackId();
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
