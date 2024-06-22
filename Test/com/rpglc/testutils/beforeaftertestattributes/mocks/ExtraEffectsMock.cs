using com.rpglc.database;
using com.rpglc.json;
using System.Reflection;
using Xunit.Sdk;

namespace com.rpglc.testutils.beforeaftertestattributes.mocks;

public class ExtraEffectsMock : BeforeAfterTestAttribute
{

    public override void Before(MethodInfo methodUnderTest)
    {
        base.Before(methodUnderTest);

        DBManager.InsertRPGLEffectTemplate(new JsonObject().LoadFromString("""
            {
                "metadata": {
                    "author": "Calvin Withun"
                },
                "name": "Complex Effect",
                "description": "This effect has some testing features.",
                "datapack_id": "test:complex_effect",
                "tags": [ ],
                "subevent_filters": {
                    "dummy_subevent": [
                        {
                            "conditions": [
                                {
                                    "condition": "true"
                                }
                            ],
                            "functions": [
                                {
                                    "function": "dummy_function"
                                }
                            ]
                        }
                    ]
                }
            }
            """));

        DBManager.InsertRPGLEffectTemplate(new JsonObject().LoadFromString("""
            {
                "metadata": {
                    "author": "Calvin Withun"
                },
                "name": "Damage Immunity",
                "description": "This effect grants its target immunity to damage.",
                "datapack_id": "test:damage_immunity",
                "tags": [ ],
                "subevent_filters": {
                    "damage_affinity": [
                        {
                            "conditions": [
                                {
                                    "condition": "objects_match",
                                    "effect": "target",
                                    "subevent": "target"
                                }
                            ],
                            "functions": [
                                {
                                    "function": "grant_immunity"
                                }
                            ]
                        }
                    ]
                }
            }
            """));

        DBManager.InsertRPGLEffectTemplate(new JsonObject().LoadFromString("""
            {
                "metadata": {
                    "author": "Calvin Withun"
                },
                "name": "Damage Resistance",
                "description": "This effect grants its target resistance to damage.",
                "datapack_id": "test:damage_resistance",
                "tags": [ ],
                "subevent_filters": {
                    "damage_affinity": [
                        {
                            "conditions": [
                                {
                                    "condition": "objects_match",
                                    "effect": "target",
                                    "subevent": "target"
                                }
                            ],
                            "functions": [
                                {
                                    "function": "grant_resistance"
                                }
                            ]
                        }
                    ]
                }
            }
            """));

        DBManager.InsertRPGLEffectTemplate(new JsonObject().LoadFromString("""
            {
                "metadata": {
                    "author": "Calvin Withun"
                },
                "name": "Damage Vulnerability",
                "description": "This effect grants its target vulnerability to damage.",
                "datapack_id": "test:damage_vulnerability",
                "tags": [ ],
                "subevent_filters": {
                    "damage_affinity": [
                        {
                            "conditions": [
                                {
                                    "condition": "objects_match",
                                    "effect": "target",
                                    "subevent": "target"
                                }
                            ],
                            "functions": [
                                {
                                    "function": "grant_vulnerability"
                                }
                            ]
                        }
                    ]
                }
            }
            """));

    }

};
