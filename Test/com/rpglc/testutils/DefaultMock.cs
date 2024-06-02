using com.rpglc.database;
using com.rpglc.json;
using System.Reflection;
using Xunit.Sdk;

namespace com.rpglc.testutils;

public class DefaultMock : BeforeAfterTestAttribute
{

    public override void Before(MethodInfo methodUnderTest)
    {
        base.Before(methodUnderTest);

        DBManager.InsertRPGLClass(new JsonObject().LoadFromString("""
            {
                "metadata": {
                    "author": "Calvin Withun"
                },
                "name": "Dummy Class",
                "description": "This class has no features.",
                "datapack_id": "test:dummy",
                "nested_classes": { },
                "features": { }
            }
            """));
        DBManager.InsertRPGLRace(new JsonObject().LoadFromString("""
            {
                "metadata": {
                    "author": "Calvin Withun"
                },
                "name": "Dummy Race",
                "description": "This race has no features.",
                "ability_score_bonuses": { },
                "features": { }
            }
            
            """));
        DBManager.InsertRPGLEffectTemplate(new JsonObject().LoadFromString("""
            {
                "metadata": {
                    "author": "Calvin Withun"
                },
                "name": "Dummy Effect",
                "description": "This effect has no features.",
                "datapack_id": "test:dummy",
                "subevent_filters": { }
            }
            """));
        DBManager.InsertRPGLEventTemplate(new JsonObject().LoadFromString("""
            {
                "metadata": {
                    "author": "Calvin Withun"
                },
                "name": "Dummy Event",
                "description": "This event has no features.",
                "datapack_id": "test:dummy",
                "area_of_effect": { },
                "cost": [ ],
                "subevents": [ ]
            }
            """));
        DBManager.InsertRPGLItemTemplate(new JsonObject().LoadFromString("""
            {
                "metadata": {
                    "author": "Calvin Withun"
                },
                "name": "Dummy Item",
                "description": "This item has no features.",
                "datapack_id": "test:dummy",
                "tags": [ ],
                "weight": 1,
                "cost": 1,
                "effects": [ ],
                "events": [ ],
                "resources": [ ]
            }
            """));
        DBManager.InsertRPGLObjectTemplate(new JsonObject().LoadFromString("""
            {
                "metadata": { },
                "name": "Dummy Object",
                "description": "This object has no features.",
                "datapack_id": "test:dummy",
                "tags": [ ],
                "ability_scores": {
                    "str": 10,
                    "dex": 10,
                    "con": 10,
                    "int": 10,
                    "wis": 10,
                    "cha": 10
                },
                "health_base": 1000,
                "health_current": 1000,
                "health_temporary": 0,
                "equipped_items": { },
                "inventory": [ ],
                "classes": [ ],
                "races": [ ],
                "effects": [ ],
                "events": [ ],
                "resources": [ ],
                "proficiency_bonus": 2
            }
            """));
        DBManager.InsertRPGLResourceTemplate(new JsonObject().LoadFromString("""
            {
                "metadata": { },
                "name": "Dummy Resource",
                "description": "This resource has no features.",
                "datapack_id": "test:dummy",
                "tags": [ ],
                "potency": 1,
                "refresh_criterion": [ ]
            }
            """));
    }

};
