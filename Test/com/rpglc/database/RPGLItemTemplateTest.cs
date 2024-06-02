using com.rpglc.core;
using com.rpglc.testutils;

namespace com.rpglc.database;

[OpenDatabaseConnection]
[CloseDatabaseConnection]
[Collection("Serial")]
public class RPGLItemTemplateTest {

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "creates new instance")]
    public void CreatesNewInstance() {
        long effectUuid = 123L;
        RPGLItem rpglItem = DBManager.QueryRPGLItemTemplateByDatapackId("test:dummy")
            .NewInstance(effectUuid);

        Assert.Equal(
            """{"author":"Calvin Withun"}""",
            rpglItem.GetMetadata().ToString()
        );
        Assert.Equal("Dummy Item", rpglItem.GetName());
        Assert.Equal("This item has no features.", rpglItem.GetDescription());
        Assert.Equal("test:dummy", rpglItem.GetDatapackId());
        Assert.Equal(effectUuid, rpglItem.GetUuid());
        Assert.Equal("""[]""", rpglItem.GetTags().ToString());
        Assert.Equal(1L, rpglItem.GetWeight());
        Assert.Equal(1L, rpglItem.GetCost());
        Assert.Equal("""[]""", rpglItem.GetEffects().ToString());
        Assert.Equal("""[]""", rpglItem.GetEvents().ToString());
        Assert.Equal("""[]""", rpglItem.GetResources().ToString());
    }

};
