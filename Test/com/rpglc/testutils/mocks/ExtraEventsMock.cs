using com.rpglc.database;
using com.rpglc.json;
using System.Reflection;
using Xunit.Sdk;

namespace com.rpglc.testutils.mocks;

public class ExtraEventsMock : BeforeAfterTestAttribute {

    public override void Before(MethodInfo methodUnderTest) {
        base.Before(methodUnderTest);

        DBManager.InsertRPGLEventTemplate(new JsonObject().LoadFromString("""
            {
                "metadata": {
                    "author": "Calvin Withun"
                },
                "name": "Complex Event",
                "description": "This event has features that can be referenced during testing.",
                "datapack_id": "test:complex_event",
                "area_of_effect": { },
                "cost": [
                    {
                        "resource_tags": [ "dummy" ],
                        "count": 1,
                        "minimum_potency": 1,
                        "scale": [ ]
                    }
                ],
                "subevents": [
                    {
                        "subevent": "dummy_subevent"
                    }
                ]
            }
            """));
    }

};
