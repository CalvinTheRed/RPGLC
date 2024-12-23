﻿using com.rpglc.core;
using com.rpglc.data;
using com.rpglc.json;
using System.Reflection;
using Xunit.Sdk;

namespace com.rpglc.testutils.beforeaftertestattributes.mocks;

public class ExtraItemsMock : BeforeAfterTestAttribute {

    public override void Before(MethodInfo methodUnderTest) {
        base.Before(methodUnderTest);

        RPGL.AddRPGLItemTemplate(new RPGLItemTemplate(new JsonObject().LoadFromString("""
            {
                "metadata": {
                    "author": "Calvin Withun"
                },
                "name": "Complex Item",
                "description": "This item has some testing features.",
                "datapack_id": "test:complex_item",
                "tags": [ ],
                "weight": 1,
                "cost": 1,
                "effects": {
                    "test:dummy": [
                        [ "mainhand", "offhand" ],
                        [ "mainhand" ],
                        [ "offhand" ]
                    ]
                },
                "events": {
                    "test:dummy": [
                        [ "mainhand", "offhand" ],
                        [ "mainhand" ],
                        [ "offhand" ]
                    ]
                },
                "resources": {
                    "test:dummy": {
                        "count": 1,
                        "slots": [
                            [ "mainhand", "offhand" ],
                            [ "mainhand" ],
                            [ "offhand" ]
                        ]
                    }
                }
            }
            """)));
    }

};
