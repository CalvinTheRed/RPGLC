﻿using com.rpglc.database;
using com.rpglc.subevent;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.core;

[AssignDatabase]
[Collection("Serial")]
[DieTestingMode]
[RPGLCInit]
public class RPGLResourceTest {

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [ExtraResourcesMock]
    [Fact(DisplayName = "refreshes")]
    [ResetCountersAfterTest]
    public void Refreshes() {
        RPGLResource rpglResource = RPGLFactory.NewResource("test:complex_resource");
        rpglResource.Exhaust(rpglResource.GetAvailableUses());
        DBManager.UpdateRPGLResource(rpglResource);
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1")
            .GiveResource(rpglResource);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        Subevent subevent = new DummySubevent();
        subevent.AddTag("refresh_resource");
        subevent.SetSource(rpglObject);
        subevent.Prepare(context, new());
        subevent.SetTarget(rpglObject);
        subevent.Invoke(context, new());

        rpglResource = DBManager.QueryRPGLResource(x => x.Uuid == rpglResource.GetUuid());

        Assert.Equal(0L, rpglResource.GetAvailableUses());
        Assert.Equal("""
            {
              "actor": "source",
              "chance": 1,
              "frequency": {
                "bonus": 2,
                "dice": [ ]
              },
              "frequency_countdown": 1,
              "subevent": "dummy_subevent",
              "tags": [
                "refresh_resource"
              ],
              "tries": {
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
            }
            """, rpglResource.SeekJsonObject("refresh_criterion[0]").PrettyPrint());

        subevent.Invoke(context, new());

        rpglResource = DBManager.QueryRPGLResource(x => x.Uuid == rpglResource.GetUuid());

        Assert.Equal(5L, rpglResource.GetAvailableUses());
        Assert.Equal("""
            {
              "actor": "source",
              "chance": 1,
              "frequency": {
                "bonus": 2,
                "dice": [ ]
              },
              "subevent": "dummy_subevent",
              "tags": [
                "refresh_resource"
              ],
              "tries": {
                "bonus": 4,
                "dice": [
                  {
                    "determined": [ ],
                    "roll": 1,
                    "size": 6
                  }
                ]
              }
            }
            """, rpglResource.SeekJsonObject("refresh_criterion[0]").PrettyPrint());
    }

};
