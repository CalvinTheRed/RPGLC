using com.rpglc.core;
using com.rpglc.data;
using com.rpglc.json;
using System.Reflection;
using Xunit.Sdk;

namespace com.rpglc.testutils.beforeaftertestattributes.mocks;

public class DefaultMock : BeforeAfterTestAttribute {

    public override void Before(MethodInfo methodUnderTest) {
        base.Before(methodUnderTest);

        RPGL.AddRPGLEffectTemplate(new RPGLEffectTemplate(new JsonObject().LoadFromString("""
            {
                "metadata": {
                    "author": "Calvin Withun"
                },
                "name": "Dummy Effect",
                "description": "This effect has no features.",
                "datapack_id": "test:dummy",
                "tags": [ ],
                "subevent_filters": { }
            }
            """)));
        RPGL.AddRPGLEventTemplate(new RPGLEventTemplate(new JsonObject().LoadFromString("""
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
            """)));
        RPGL.AddRPGLItemTemplate(new RPGLItemTemplate(new JsonObject().LoadFromString("""
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
                "effects": { },
                "events": { },
                "resources": { }
            }
            """)));
        RPGL.AddRPGLObjectTemplate(new RPGLObjectTemplate(new JsonObject().LoadFromString("""
            {
                "metadata": {
                    "author": "Calvin Withun"
                },
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
                "health_temporary": {
                    "count": 0,
                    "rider_effects": [ ]
                },
                "equipped_items": { },
                "inventory": [ ],
                "classes": [ ],
                "races": [ ],
                "effects": [ ],
                "events": [ ],
                "resources": [ ],
                "proficiency_bonus": 2
            }
            """)));
        RPGL.AddRPGLResourceTemplate(new RPGLResourceTemplate(new JsonObject().LoadFromString("""
            {
                "metadata": {
                    "author": "Calvin Withun"
                },
                "name": "Dummy Resource",
                "description": "This resource has no features.",
                "datapack_id": "test:dummy",
                "tags": [ ],
                "potency": 1,
                "refresh_criterion": [ ]
            }
            """)));
        RPGL.AddRPGLClass(new RPGLClass(new JsonObject().LoadFromString("""
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
                """).AsDict()));
        RPGL.AddRPGLRace(new RPGLRace(new JsonObject().LoadFromString("""
            {
                "metadata": {
                    "author": "Calvin Withun"
                },
                "name": "Dummy Race",
                "description": "This race has no features.",
                "datapack_id": "test:dummy",
                "ability_score_bonuses": { },
                "features": { }
            }
            """).AsDict()));
    }

};
