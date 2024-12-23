﻿using com.rpglc.core;
using com.rpglc.testutils.beforeaftertestattributes.mocks;

namespace com.rpglc.data;

[Collection("Serial")]
public class RPGLEventTemplateTest {

    [DefaultMock]
    [Fact(DisplayName = "creates new instance")]
    public void CreatesNewInstance() {
        RPGLEvent rpglEvent = RPGL.GetRPGLEventTemplate("test:dummy")
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

    [ExtraEventsMock]
    [Fact(DisplayName = "assigns cost")]
    public void AssignsCost() {
        RPGLEvent rpglEvent = RPGL.GetRPGLEventTemplate("test:complex_event")
            .NewInstance();

        Assert.Equal(
            """
            [
              {
                "count": 1,
                "minimum_potency": 1,
                "resource_tags": [
                  "dummy"
                ],
                "scale": [
                  {
                    "field": "subevents[0].scalable_field",
                    "magnitude": 2
                  }
                ]
              }
            ]
            """,
            rpglEvent.GetCost().PrettyPrint()
        );
    }

};
