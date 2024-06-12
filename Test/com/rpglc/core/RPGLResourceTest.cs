using com.rpglc.database;
using com.rpglc.subevent;
using com.rpglc.testutils;
using com.rpglc.testutils.mocks;

namespace com.rpglc.core;

[AssignDatabase]
[InitializeSubevents]
[DieTestingMode]
[Collection("Serial")]
public class RPGLResourceTest {

    [DefaultMock]
    [ExtraResourcesMock]
    [ResetCountersAfterTest]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "refreshes")]
    public void Refreshes() {
        RPGLResource rpglResource = RPGLFactory.NewResource("test:complex_resource")
            .SetAvailableUses(0L);
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
