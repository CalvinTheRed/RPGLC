using com.rpglc.database;
using com.rpglc.json;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;

namespace com.rpglc.core;

[AssignDatabase]
[Collection("Serial")]
public class RPGLEventTest {

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [ExtraEventsMock]
    [Fact(DisplayName = "too few resources")]
    public void TooFewResources() {
        RPGLEvent rpglEvent = RPGLFactory.NewEvent("test:complex_event");
        Assert.False(rpglEvent.ResourcesSatisfyCost([]));
    }

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "too many resources")]
    public void TooManyResources() {
        RPGLEvent rpglEvent = RPGLFactory.NewEvent("test:dummy");
        RPGLResource rpglResource = RPGLFactory.NewResource("test:dummy");
        DBManager.UpdateRPGLResource(rpglResource);
        Assert.True(rpglEvent.ResourcesSatisfyCost([rpglResource]));
    }

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [ExtraEventsMock]
    [ExtraResourcesMock]
    [Fact(DisplayName = "insufficient potency")]
    public void InsufficientPotency() {
        RPGLEvent rpglEvent = RPGLFactory.NewEvent("test:complex_event");
        RPGLResource rpglResource = RPGLFactory.NewResource("test:complex_resource");
        rpglResource.SetPotency(0L);
        DBManager.UpdateRPGLResource(rpglResource);
        Assert.False(rpglEvent.ResourcesSatisfyCost([ rpglResource ]));
    }

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [ExtraEventsMock]
    [ExtraResourcesMock]
    [Fact(DisplayName = "sufficient resources")]
    public void SufficientResources() {
        RPGLEvent rpglEvent = RPGLFactory.NewEvent("test:complex_event");
        RPGLResource rpglResource = RPGLFactory.NewResource("test:complex_resource");
        rpglResource.SetPotency(1L);
        DBManager.UpdateRPGLResource(rpglResource);
        Assert.True(rpglEvent.ResourcesSatisfyCost([rpglResource]));
    }

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "sufficient resources mixed order")]
    public void SufficientResourcesMixedOrder() {
        RPGLEvent rpglEvent = RPGLFactory.NewEvent("test:dummy")
            .SetCost(new JsonArray().LoadFromString("""
                [
                    {
                        "resource_tags": [ "resource_1" ],
                        "count": 1,
                        "minimum_potency": 0,
                        "scale": [ ]
                    },
                    {
                        "resource_tags": [ "resource_2" ],
                        "count": 1,
                        "minimum_potency": 0,
                        "scale": [ ]
                    }
                ]
                """));

        RPGLResource rpglResource1 = (RPGLResource) RPGLFactory.NewResource("test:dummy")
            .SetTags(new JsonArray().AddString("resource_1"));
        RPGLResource rpglResource2 = (RPGLResource) RPGLFactory.NewResource("test:dummy")
            .SetTags(new JsonArray().AddString("resource_2"));

        DBManager.UpdateRPGLResource(rpglResource1);
        DBManager.UpdateRPGLResource(rpglResource2);
        Assert.True(rpglEvent.ResourcesSatisfyCost([rpglResource2, rpglResource1]));
    }

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [ExtraEventsMock]
    [ExtraResourcesMock]
    [Fact(DisplayName = "scales")]
    public void Scales() {
        RPGLEvent rpglEvent = RPGLFactory.NewEvent("test:complex_event");
        RPGLResource rpglResource = RPGLFactory.NewResource("test:complex_resource");
        rpglEvent.Scale([rpglResource]);
        Assert.Equal(0 + 2, rpglEvent.SeekLong("subevents[0].scalable_field"));
    }

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "scales mixed order")]
    public void ScalesMixedOrder() {
        RPGLEvent rpglEvent = (RPGLEvent) RPGLFactory.NewEvent("test:dummy")
            .SetCost(new JsonArray().LoadFromString("""
                [
                    {
                        "resource_tags": [ "resource_1" ],
                        "count": 1,
                        "minimum_potency": 0,
                        "scale": [
                            {
                                "field": "scalable_field",
                                "magnitude": 1
                            }
                        ]
                    },
                    {
                        "resource_tags": [ "resource_2" ],
                        "count": 1,
                        "minimum_potency": 0,
                        "scale": [
                            {
                                "field": "scalable_field",
                                "magnitude": 1
                            }
                        ]
                    }
                ]
                """))
            .PutLong("scalable_field", 0L);

        RPGLResource rpglResource1 = (RPGLResource) RPGLFactory.NewResource("test:dummy")
            .SetPotency(2L)
            .SetTags(new JsonArray().AddString("resource_1"));
        RPGLResource rpglResource2 = (RPGLResource) RPGLFactory.NewResource("test:dummy")
            .SetPotency(2L)
            .SetTags(new JsonArray().AddString("resource_2"));

        DBManager.UpdateRPGLResource(rpglResource1);
        DBManager.UpdateRPGLResource(rpglResource2);
        rpglEvent.Scale([rpglResource2, rpglResource1]);
        Assert.Equal(0 + 2 + 2, rpglEvent.GetLong("scalable_field"));
    }

};
