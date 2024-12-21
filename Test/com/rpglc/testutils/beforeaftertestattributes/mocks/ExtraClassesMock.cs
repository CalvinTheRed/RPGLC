using com.rpglc.core;
using com.rpglc.json;
using System.Reflection;
using Xunit.Sdk;

namespace com.rpglc.testutils.beforeaftertestattributes.mocks;

public class ExtraClassesMock : BeforeAfterTestAttribute {

    public override void Before(MethodInfo methodUnderTest) {
        base.Before(methodUnderTest);
        
        RPGL.AddRPGLClass(new RPGLClass(new JsonObject().LoadFromString("""
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
                """).AsDict()));
        RPGL.AddRPGLClass(new RPGLClass(new JsonObject().LoadFromString("""
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
                """).AsDict()));
        RPGL.AddRPGLClass(new RPGLClass(new JsonObject().LoadFromString("""
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
                """).AsDict()));
        RPGL.AddRPGLClass(new RPGLClass(new JsonObject().LoadFromString("""
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
                """).AsDict()));
        RPGL.AddRPGLClass(new RPGLClass(new JsonObject().LoadFromString("""
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
                """).AsDict()));
    }

};
