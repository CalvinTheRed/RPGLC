using com.rpglc.core;
using com.rpglc.testutils;

namespace com.rpglc.database;

[SetupDatabase]
[Collection("Serial")]
public class RPGLEventTemplateTest {

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "creates new instance")]
    public void CreatesNewInstance() {
        RPGLEvent rpglEvent = DBManager.QueryRPGLEventTemplateByDatapackId("test:dummy")
            .NewInstance();

        Assert.Equal(
            """{"author":"Calvin Withun"}""",
            rpglEvent.GetMetadata().ToString()
        );
        Assert.Equal("Dummy Event", rpglEvent.GetName());
        Assert.Equal("This event has no features.", rpglEvent.GetDescription());
        Assert.Equal("test:dummy", rpglEvent.GetDatapackId());
        Assert.Equal("""{}""", rpglEvent.GetAreaOfEffect().ToString());
        Assert.Equal("""[]""", rpglEvent.GetCost().ToString());
        Assert.Equal("""[]""", rpglEvent.GetSubevents().ToString());
    }

};
