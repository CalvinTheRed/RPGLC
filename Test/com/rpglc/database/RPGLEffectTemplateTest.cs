using com.rpglc.core;
using com.rpglc.testutils;

namespace com.rpglc.database;

[OpenDatabaseConnection]
[CloseDatabaseConnection]
[Collection("Serial")]
public class RPGLEffectTemplateTest {

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "creates new instance")]
    public void CreatesNewInstance() {
        string effectUuid = "uuid";
        RPGLEffect rpglEffect = DBManager.QueryRPGLEffectTemplateByDatapackId("test:dummy")
            .NewInstance(effectUuid);

        Assert.Equal(
            """{"author":"Calvin Withun"}""",
            rpglEffect.GetMetadata().ToString()
        );
        Assert.Equal("Dummy Effect", rpglEffect.GetName());
        Assert.Equal("This effect has no features.", rpglEffect.GetDescription());
        Assert.Equal("test:dummy", rpglEffect.GetDatapackId());
        Assert.Equal(effectUuid, rpglEffect.GetUuid());
        Assert.Equal("""{}""", rpglEffect.GetSubeventFilters().ToString());
    }

};
