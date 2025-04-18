using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class SpawnObjectTest {

    [ClearRPGLAfterTest]
    [Fact(DisplayName = "prepares")]
    public void Prepares() {
        SpawnObject spawnObject = new SpawnObject().Prepare(new DummyContext(), new());

        Assert.Equal("{ }", spawnObject.json.GetJsonObject("controlled_by").PrettyPrint());
        Assert.Equal("[ ]", spawnObject.json.GetJsonArray("object_bonuses").PrettyPrint());
        Assert.Equal("[ ]", spawnObject.json.GetJsonArray("extra_effects").PrettyPrint());
        Assert.Equal("[ ]", spawnObject.json.GetJsonArray("extra_events").PrettyPrint());
        Assert.Equal("[ ]", spawnObject.json.GetJsonArray("extra_tags").PrettyPrint());
        Assert.False(spawnObject.json.GetBool("use_origin_proficiency"));
        Assert.False(spawnObject.json.GetBool("proxy"));
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "spawns object")]
    public void SpawnsObject() {
        RPGLObject spawnerObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        JsonArray spawnCoordinates = new JsonArray()
            .AddLong(10)
            .AddLong(0)
            .AddLong(10);
        SpawnObject spawnObject = new SpawnObject()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "object": "test:dummy",
                    "controlled_by": {
                        "from": "subevent",
                        "object": "source",
                        "as_origin": false
                    },
                }
                """))
            .SetSource(spawnerObject)
            .Prepare(new DummyContext(), spawnCoordinates)
            .SetTarget(spawnerObject)
            .Invoke(new DummyContext(), spawnCoordinates);

        List<RPGLObject> newObjects = [.. RPGL.GetRPGLObjects().FindAll(o => o.GetUuid() != spawnerObject.GetUuid())];
        Assert.Single(newObjects);
        RPGLObject spawnedObject = newObjects[0];
        Assert.Equal(spawnerObject.GetUserId(), spawnedObject.GetUserId());
        Assert.Equal("[10,0,10]", spawnedObject.GetPosition().ToString());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "spawns object with bonuses")]
    public void SpawnsObjectWithBonuses() {
        RPGLObject spawnerObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        SpawnObject spawnObject = new SpawnObject()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "object": "test:dummy",
                    "object_bonuses": [
                        {
                            "field": "health_base",
                            "bonus": 5
                        }
                    ]
                }
                """))
            .SetSource(spawnerObject)
            .Prepare(new DummyContext(), new())
            .SetTarget(spawnerObject)
            .Invoke(new DummyContext(), new());

        RPGLObject spawnedObject = RPGL.GetRPGLObjects().FindAll(o => o.GetUuid() != spawnerObject.GetUuid()).First();
        Assert.Equal(1000 + 5, spawnedObject.GetHealthBase());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "spawns object with extra effects")]
    public void SpawnsObjectWithExtraEffects() {
        RPGLObject spawnerObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        SpawnObject spawnObject = new SpawnObject()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "object": "test:dummy",
                    "extra_effects": [
                        "test:dummy"
                    ]
                }
                """))
            .SetSource(spawnerObject)
            .Prepare(new DummyContext(), new())
            .SetTarget(spawnerObject)
            .Invoke(new DummyContext(), new());

        RPGLObject spawnedObject = RPGL.GetRPGLObjects().FindAll(o => o.GetUuid() != spawnerObject.GetUuid()).First();
        List<RPGLEffect> spawnedObjectEffects = spawnedObject.GetEffectObjects();
        Assert.Single(spawnedObjectEffects);
        Assert.Equal("test:dummy", spawnedObjectEffects[0].GetDatapackId());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "spawns object with extra events")]
    public void SpawnsObjectWithExtraEvents() {
        RPGLObject spawnerObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        SpawnObject spawnObject = new SpawnObject()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "object": "test:dummy",
                    "extra_events": [
                        "test:dummy"
                    ]
                }
                """))
            .SetSource(spawnerObject)
            .Prepare(new DummyContext(), new())
            .SetTarget(spawnerObject)
            .Invoke(new DummyContext(), new());

        RPGLObject spawnedObject = RPGL.GetRPGLObjects().FindAll(o => o.GetUuid() != spawnerObject.GetUuid()).First();
        List<RPGLEvent> spawnedObjectEvents = spawnedObject.GetEventObjects(new DummyContext());
        Assert.Single(spawnedObjectEvents);
        Assert.Equal("test:dummy", spawnedObjectEvents[0].GetDatapackId());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "spawns object with extra tags")]
    public void SpawnsObjectWithExtraTags() {
        RPGLObject spawnerObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        SpawnObject spawnObject = new SpawnObject()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "object": "test:dummy",
                    "extra_tags": [
                        "test_tag"
                    ]
                }
                """))
            .SetSource(spawnerObject)
            .Prepare(new DummyContext(), new())
            .SetTarget(spawnerObject)
            .Invoke(new DummyContext(), new());

        RPGLObject spawnedObject = RPGL.GetRPGLObjects().FindAll(o => o.GetUuid() != spawnerObject.GetUuid()).First();
        List<object> spawnedObjectTags = spawnedObject.GetTags().AsList();
        Assert.Single(spawnedObjectTags);
        Assert.Equal("test_tag", spawnedObjectTags[0]);
    }

};
