using com.rpglc.database;
using System.Reflection;
using Xunit.Sdk;

namespace com.rpglc.testutils.beforeaftertestattributes.mocks;

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
                "maximum_uses": 10,
                "refresh_criterion": [
                    {
                        "subevent": "dummy_subevent",
                        "tags": [ "refresh_resource" ],
                        "frequency": {
                            "bonus": 2,
                            "dice": [ ]
                        },
                        "tries": {
                            "bonus": 4,
                            "dice": [
                                { "count": 1, "size": 6, "determined": [ 1 ] }
                            ]
                        },
                        "chance": 1
                    }
                ]
            }
            """));
    }

};
