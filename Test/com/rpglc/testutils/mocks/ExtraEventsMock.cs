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
                "name": "Event With Cost",
                "description": "This event has a cost that can be referenced during testing.",
                "datapack_id": "test:event_with_cost",
                "area_of_effect": { },
                "cost": [
                    {
                        "resource_tags": [ "tag" ],
                        "count": 3,
                        "minimum_potency": 2,
                        "scale": [ ]
                    }
                ],
                "subevents": [ ]
            }
            """));
    }

};
