using com.rpglc.database;
using com.rpglc.json;
using System.Reflection;
using Xunit.Sdk;

namespace com.rpglc.testutils.beforeaftertestattributes.mocks;

public class ExtraObjectsMock : BeforeAfterTestAttribute {

    public override void Before(MethodInfo methodUnderTest) {
        base.Before(methodUnderTest);

        DBManager.InsertRPGLObjectTemplate(new JsonObject().LoadFromString("""
            {
                "metadata": {
                    "author": "Calvin Withun"
                },
                "name": "Complex Object",
                "description": "This object has numerous dummy features.",
                "datapack_id": "test:complex_object",
                "tags": [ ],
                "ability_scores": {
                    "str": 13,
                    "dex": 12,
                    "con": 11,
                    "int": 9,
                    "wis": 8,
                    "cha": 7
                },
                "health_base": 1000,
                "health_current": 1000,
                "health_temporary": 0,
                "equipped_items": {
                    "mainhand": "test:dummy",
                    "offhand": "test:dummy"
                },
                "inventory": [
                    "test:dummy"
                ],
                "classes": [
                    {
                        "additional_nested_classes": {
                            "test:additional_nested_class": {
                                "scale": 2,
                                "round_up": false
                            }
                        },
                        "id": "test:class_with_nested_class",
                        "level": 1,
                        "choices": { }
                    }
                ],
                "races": [ ],
                "effects": [
                    "test:dummy"
                ],
                "events": [ ],
                "resources": [
                    "test:dummy"
                ]
            }
            """));
    }

};
