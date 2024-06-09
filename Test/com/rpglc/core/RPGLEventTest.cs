using com.rpglc.database;
using com.rpglc.testutils;
using com.rpglc.testutils.mocks;

namespace com.rpglc.core;

[AssignDatabase]
[Collection("Serial")]
public class RPGLEventTest {

    [DefaultMock]
    [ExtraEventsMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "too few resources")]
    public void TooFewResources() {
        RPGLEvent rpglEvent = RPGLFactory.NewEvent("test:complex_event");
        Assert.False(rpglEvent.ResourcesMatchCost([]));
    }

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "too many resources")]
    public void TooManyResources() {
        RPGLEvent rpglEvent = RPGLFactory.NewEvent("test:dummy");
        RPGLResource rpglResource = RPGLFactory.NewResource("test:dummy");
        DBManager.UpdateRPGLResource(rpglResource);
        Assert.False(rpglEvent.ResourcesMatchCost([rpglResource]));
    }

    [DefaultMock]
    [ExtraEventsMock]
    [ExtraResourcesMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "insufficient potency")]
    public void InsufficientPotency() {
        RPGLEvent rpglEvent = RPGLFactory.NewEvent("test:complex_event");
        RPGLResource rpglResource = RPGLFactory.NewResource("test:complex_resource");
        rpglResource.SetPotency(0L);
        DBManager.UpdateRPGLResource(rpglResource);
        Assert.False(rpglEvent.ResourcesMatchCost([ rpglResource ]));
    }

    [DefaultMock]
    [ExtraEventsMock]
    [ExtraResourcesMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "sufficient resources")]
    public void SufficientResources() {
        RPGLEvent rpglEvent = RPGLFactory.NewEvent("test:complex_event");
        RPGLResource rpglResource = RPGLFactory.NewResource("test:complex_resource");
        rpglResource.SetPotency(1L);
        DBManager.UpdateRPGLResource(rpglResource);
        Assert.True(rpglEvent.ResourcesMatchCost([rpglResource]));
    }

    [DefaultMock]
    [ExtraEventsMock]
    [ExtraResourcesMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "scales")]
    public void Scales() {
        RPGLEvent rpglEvent = RPGLFactory.NewEvent("test:complex_event");
        RPGLResource rpglResource = RPGLFactory.NewResource("test:complex_resource");
        rpglEvent.Scale([rpglResource]);
        Assert.Equal(0 + 2, rpglEvent.SeekInt("subevents[0].scalable_field"));
    }

};
