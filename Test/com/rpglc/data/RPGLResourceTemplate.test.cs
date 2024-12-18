﻿using com.rpglc.core;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;

namespace com.rpglc.data;

[AssignDatabase]
[Collection("Serial")]
public class RPGLResourceTemplateTest {

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "creates new instance")]
    public void CreatesNewInstance() {
        string effectUuid = "uuid";
        RPGLResource rpglResource = RPGL.GetRPGLResourceTemplates()
            .Find(x => x.GetDatapackId() == "test:dummy")
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

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [ExtraResourcesMock]
    [Fact(DisplayName = "unpacks dice")]
    public void UnpacksDice() {
        string effectUuid = "uuid";
        RPGLResource rpglResource = RPGL.GetRPGLResourceTemplates()
            .Find(x => x.GetDatapackId() == "test:complex_resource")
            .NewInstance(effectUuid);

        Assert.Equal("""
            {
              "bonus": 4,
              "dice": [
                {
                  "determined": [
                    1
                  ],
                  "size": 6
                }
              ]
            }
            """, rpglResource.SeekJsonObject("refresh_criterion[0].tries").PrettyPrint());
    }

};