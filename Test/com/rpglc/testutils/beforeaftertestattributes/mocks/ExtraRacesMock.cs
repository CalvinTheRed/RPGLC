using com.rpglc.database;
using com.rpglc.json;
using System.Reflection;
using Xunit.Sdk;

namespace com.rpglc.testutils.beforeaftertestattributes.mocks;

public class ExtraRacesMock : BeforeAfterTestAttribute
{

    public override void Before(MethodInfo methodUnderTest)
    {
        base.Before(methodUnderTest);

        DBManager.InsertRPGLRace(new JsonObject().LoadFromString("""
            {
                "metadata": {
                    "author": "Calvin Withun"
                },
                "name": "Race With Leveled Features",
                "description": "A race designed for testing racial level-up features.",
                "datapack_id": "test:race_with_leveled_features",
                "ability_score_bonuses": { },
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
                                    "resource": "test:dummy",
                                    "count": 1
                                }
                            ]
                        }
                    }
                }
            }
            """));
        DBManager.InsertRPGLRace(new JsonObject().LoadFromString("""
            {
                "metadata": {
                    "author": "Calvin Withun"
                },
                "name": "Race With Resource Per Level",
                "description": "A race which grants a dummy resource for its first 5 levels.",
                "datapack_id": "test:race_with_resource_per_level",
                "ability_score_bonuses": { },
                "features": {
                    "1": {
                        "gain": {
                            "effects": [ ],
                            "events": [ ],
                            "resources": [
                                "test:dummy"
                            ]
                        }
                    },
                    "2": {
                        "gain": {
                            "effects": [ ],
                            "events": [ ],
                            "resources": [
                                "test:dummy"
                            ]
                        }
                    },
                    "3": {
                        "gain": {
                            "effects": [ ],
                            "events": [ ],
                            "resources": [
                                "test:dummy"
                            ]
                        }
                    },
                    "4": {
                        "gain": {
                            "effects": [ ],
                            "events": [ ],
                            "resources": [
                                "test:dummy"
                            ]
                        }
                    },
                    "5": {
                        "gain": {
                            "effects": [ ],
                            "events": [ ],
                            "resources": [
                                "test:dummy"
                            ]
                        }
                    }
                }
            }
            """));
    }

};
