using com.rpglc.core;
using com.rpglc.testutils;
using com.rpglc.testutils.mocks;

namespace com.rpglc.database;

[AssignDatabase]
[Collection("Serial")]
public class RPGLItemTemplateTest {

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "creates new instance")]
    public void CreatesNewInstance() {
        string effectUuid = "uuid";
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
