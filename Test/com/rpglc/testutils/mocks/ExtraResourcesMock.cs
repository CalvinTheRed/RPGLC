using com.rpglc.database;
using System.Reflection;
using Xunit.Sdk;

namespace com.rpglc.testutils.mocks;

public class ExtraResourcesMock : BeforeAfterTestAttribute {

    public override void Before(MethodInfo methodUnderTest) {
        base.Before(methodUnderTest);

        DBManager.InsertRPGLResourceTemplate(new json.JsonObject().LoadFromString("""
            {
                "metadata": {
                    "author": "Calvin Withun"
                },
                "name": "Complex Resource",
                "description": "This resource has features that can be referenced during testing.",
                "datapack_id": "test:complex_resource",
                "tags": [ "dummy" ],
                "potency": 2,
                "refresh_criterion": [ ]
            }
            """));

    }

};
