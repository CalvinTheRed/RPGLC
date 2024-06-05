using com.rpglc.database;
using com.rpglc.json;
using System.Reflection;

namespace com.rpglc.testutils.mocks;

public class ExtraClassesMock : DefaultMock
{

    public override void Before(MethodInfo methodUnderTest)
    {
        base.Before(methodUnderTest);

        DBManager.InsertRPGLClass(new JsonObject().LoadFromString("""
            {
                "metadata": {
                    "author": "Calvin Withun"
                },
                "name": "Starting Features Test Class",
                "description": "A class designed to test starting features.",
                "datapack_id": "test:class_with_starting_features",
                "starting_features": {
                    "effects": [
                        "test:dummy",
                        {
                            "name": "Effect Choice",
                            "count": 1,
                            "options": [
                                "does-not-exist",
                                "test:dummy",
                                "does-not-exist"
                            ]
                        }
                    ],
                    "events": [
                        "test:dummy"
                    ],
                    "resources": [
                        "test:dummy",
                        {
                            "count": 1,
                            "resource": "test:dummy"
                        }
                    ]
                },
                "features": { }
            }
            """));
        DBManager.InsertRPGLClass(new JsonObject().LoadFromString("""
            {
                "metadata": {
                    "author": "Calvin Withun"
                },
                "name": "Class With Leveled Features",
                "description": "A class designed to test leveling features.",
                "datapack_id": "test:class_with_leveled_features",
                "starting_features": { },
                "features": {
                    "1": {
                        "gain": {
                            "effects": [
                                "test:dummy",
                                {
                                    "name": "Effect Choice",
                                    "count": 1,
                                    "options": [
                                        "does-not-exist",
                                        "test:dummy",
                                        "does-not-exist"
                                    ]
                                }
                            ],
                            "events": [
                                "test:dummy"
                            ],
                            "resources": [
                                "test:dummy",
                                {
                                    "count": 1,
                                    "resource": "test:dummy"
                                }
                            ]
                        }
                    }
                }
            }
            """));
        DBManager.InsertRPGLClass(new JsonObject().LoadFromString("""
            {
                "metadata": {
                    "author": "Calvin Withun"
                },
                "name": "Nested Class",
                "description": "A class designed to be used as a nested class for testing.",
                "datapack_id": "test:nested_class",
                "starting_features": { },
                "features": { }
            }
            """));
        DBManager.InsertRPGLClass(new JsonObject().LoadFromString("""
            {
                "metadata": {
                    "author": "Calvin Withun"
                },
                "name": "Additional Nested Class",
                "description": "A class designed to be used as an additional nested class for testing.",
                "datapack_id": "test:additional_nested_class",
                "starting_features": { },
                "features": { }
            }
            """));
        DBManager.InsertRPGLClass(new JsonObject().LoadFromString("""
            {
                "metadata": {
                    "author": "Calvin Withun"
                },
                "name": "Class With Nested Class",
                "description": "A class with a nested class.",
                "datapack_id": "test:class_with_nested_class",
                "nested_classes": {
                    "test:nested_class": {
                        "scale": 1,
                        "round_up": true
                    }
                },
                "starting_features": { },
                "features": { }
            }
            """));

        DBManager.InsertRPGLObjectTemplate(new JsonObject().LoadFromString("""
            {
                "metadata": {
                    "author": "Calvin Withun"
                },
                "name": "Object with Nested Class and Additional Nested Class",
                "description": "This object has a nested class and an additional nested class.",
                "datapack_id": "test:object_with_nested_class_and_additional_nested_class",
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
                "classes": [
                    {
                        "additional_nested_classes": {
                            "test:additional_nested_class": {
                                "scale": 2,
                                "round_up": false
                            }
                        },
                        "id": "test:class_with_nested_class",
                        "level": 1,
                        "choices": { }
                    }
                ],
                "races": [ ],
                "effects": [ ],
                "events": [ ],
                "resources": [ ],
                "proficiency_bonus": 2
            }
            """));
    }

}
