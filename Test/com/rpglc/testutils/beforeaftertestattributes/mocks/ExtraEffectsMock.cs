using com.rpglc.core;
using com.rpglc.data;
using com.rpglc.json;
using System.Reflection;
using Xunit.Sdk;

namespace com.rpglc.testutils.beforeaftertestattributes.mocks;

public class ExtraEffectsMock : BeforeAfterTestAttribute {

    public override void Before(MethodInfo methodUnderTest) {
        base.Before(methodUnderTest);

        RPGL.AddRPGLEffectTemplate(new RPGLEffectTemplate(new JsonObject().LoadFromString("""
            {
                "metadata": {
                    "author": "Calvin Withun"
                },
                "name": "Complex Effect",
                "description": "This effect has some testing features.",
                "datapack_id": "test:complex_effect",
                "allow_duplicates": true,
                "optional": false,
                "tags": [
                    "test_tag"
                ],
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
            """)));
        RPGL.AddRPGLEffectTemplate(new RPGLEffectTemplate(new JsonObject().LoadFromString("""
            {
                "metadata": {
                    "author": "Calvin Withun"
                },
                "name": "Damage Immunity",
                "description": "This effect grants its target immunity to damage.",
                "datapack_id": "test:damage_immunity",
                "allow_duplicates": true,
                "optional": false,
                "tags": [ ],
                "subevent_filters": {
                    "damage_affinity": [
                        {
                            "conditions": [
                                {
                                    "condition": "objects_match",
                                    "objects": [
                                        {
                                            "from": "effect",
                                            "object": "target",
                                            "as_origin": false
                                        },
                                        {
                                            "from": "subevent",
                                            "object": "target",
                                            "as_origin": false
                                        }
                                    ]
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
            """)));
        RPGL.AddRPGLEffectTemplate(new RPGLEffectTemplate(new JsonObject().LoadFromString("""
            {
                "metadata": {
                    "author": "Calvin Withun"
                },
                "name": "Damage Resistance",
                "description": "This effect grants its target resistance to damage.",
                "datapack_id": "test:damage_resistance",
                "allow_duplicates": true,
                "optional": false,
                "tags": [ ],
                "subevent_filters": {
                    "damage_affinity": [
                        {
                            "conditions": [
                                {
                                    "condition": "objects_match",
                                    "objects": [
                                        {
                                            "from": "effect",
                                            "object": "target",
                                            "as_origin": false
                                        },
                                        {
                                            "from": "subevent",
                                            "object": "target",
                                            "as_origin": false
                                        }
                                    ]
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
            """)));
        RPGL.AddRPGLEffectTemplate(new RPGLEffectTemplate(new JsonObject().LoadFromString("""
            {
                "metadata": {
                    "author": "Calvin Withun"
                },
                "name": "Damage Vulnerability",
                "description": "This effect grants its target vulnerability to damage.",
                "datapack_id": "test:damage_vulnerability",
                "allow_duplicates": true,
                "optional": false,
                "tags": [ ],
                "subevent_filters": {
                    "damage_affinity": [
                        {
                            "conditions": [
                                {
                                    "condition": "objects_match",
                                    "objects": [
                                        {
                                            "from": "effect",
                                            "object": "target",
                                            "as_origin": false
                                        },
                                        {
                                            "from": "subevent",
                                            "object": "target",
                                            "as_origin": false
                                        }
                                    ]
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
            """)));
        RPGL.AddRPGLEffectTemplate(new RPGLEffectTemplate(new JsonObject().LoadFromString("""
            {
                "metadata": {
                    "author": "Calvin Withun"
                },
                "name": "No Crits",
                "description": "This effect prevents critical hits from dealing critical damage in context.",
                "datapack_id": "test:no_crits",
                "allow_duplicates": true,
                "optional": false,
                "tags": [ ],
                "subevent_filters": {
                    "critical_damage_confirmation": [
                        {
                            "conditions": [ ],
                            "functions": [
                                {
                                    "function": "suppress_critical_damage"
                                }
                            ]
                        }
                    ]
                }
            }
            """)));
    }

};
