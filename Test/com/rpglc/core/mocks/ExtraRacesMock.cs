using com.rpglc.database;
using com.rpglc.testutils;
using System.Reflection;

namespace com.rpglc.core.mocks;

public class ExtraRacesMock : DefaultMock {

    public override void Before(MethodInfo methodUnderTest) {
        base.Before(methodUnderTest);

        DBManager.InsertRPGLRace(new json.JsonObject().LoadFromString("""
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
    }

};
