﻿using com.rpglc.core;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;

namespace com.rpglc.data;

[Collection("Serial")]
public class RPGLEffectTemplateTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "creates new instance")]
    public void CreatesNewInstance() {
        string effectUuid = "uuid";
        RPGLEffect rpglEffect = RPGL.GetRPGLEffectTemplate("test:dummy")
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
        Assert.True(rpglEffect.GetAllowDuplicates());
        Assert.False(rpglEffect.GetOptional());
    }

};
