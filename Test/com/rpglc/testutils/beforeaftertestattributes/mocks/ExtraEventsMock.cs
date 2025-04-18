﻿using com.rpglc.core;
using com.rpglc.data;
using com.rpglc.json;
using System.Reflection;
using Xunit.Sdk;

namespace com.rpglc.testutils.beforeaftertestattributes.mocks;

public class ExtraEventsMock : BeforeAfterTestAttribute {

    public override void Before(MethodInfo methodUnderTest) {
        base.Before(methodUnderTest);

        RPGL.AddRPGLEventTemplate(new RPGLEventTemplate(new JsonObject().LoadFromString("""
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
                        "scale": [
                            {
                                "field": "subevents[0].scalable_field",
                                "magnitude": 2
                            }
                        ]
                    }
                ],
                "subevents": [
                    {
                        "subevent": "dummy_subevent",
                        "scalable_field": 0
                    }
                ]
            }
            """)));
    }

};
