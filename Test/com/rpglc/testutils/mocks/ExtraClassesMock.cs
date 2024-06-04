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
    }

}
