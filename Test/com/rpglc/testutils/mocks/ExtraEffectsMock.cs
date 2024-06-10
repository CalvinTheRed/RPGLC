using com.rpglc.database;
using com.rpglc.json;
using System.Reflection;
using Xunit.Sdk;

namespace com.rpglc.testutils.mocks;

public class ExtraEffectsMock : BeforeAfterTestAttribute {

    public override void Before(MethodInfo methodUnderTest) {
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

    }

};
