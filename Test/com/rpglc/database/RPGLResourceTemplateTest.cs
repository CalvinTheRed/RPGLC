using com.rpglc.core;
using com.rpglc.testutils;
using com.rpglc.testutils.mocks;

namespace com.rpglc.database;

[AssignDatabase]
[Collection("Serial")]
public class RPGLResourceTemplateTest {

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "creates new instance")]
    public void CreatesNewInstance() {
        string effectUuid = "uuid";
        RPGLResource rpglResource = DBManager.QueryRPGLResourceTemplateByDatapackId("test:dummy")
            .NewInstance(effectUuid);

        Assert.Equal(
            """{"author":"Calvin Withun"}""",
            rpglResource.GetMetadata().ToString()
        );
        Assert.Equal("Dummy Resource", rpglResource.GetName());
        Assert.Equal("This resource has no features.", rpglResource.GetDescription());
        Assert.Equal("test:dummy", rpglResource.GetDatapackId());
        Assert.Equal(effectUuid, rpglResource.GetUuid());
        Assert.Equal("""[]""", rpglResource.GetTags().ToString());
        Assert.Equal("""[]""", rpglResource.GetRefreshCriterion().ToString());
        Assert.Null(rpglResource.GetOriginItem());
        Assert.Equal(1L, rpglResource.GetAvailableUses());
        Assert.Equal(1L, rpglResource.GetMaximumUses());
        Assert.Equal(1L, rpglResource.GetPotency());
    }

};
