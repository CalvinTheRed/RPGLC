using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.function;

[Collection("Serial")]
public class AddVampirismTest {

    [Fact(DisplayName = "adds vampirism (default)")]
    public void AddsVampirismDefault() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummyVampiricSubevent().JoinSubeventData(new JsonObject().LoadFromString(
            """{ "vampirism": [ ] }"""
        ));

        AddVampirism function = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "add_vampirism"
            }
            """);

        FunctionState.StateData result;

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal("""
            [
              {
                "damage_type": "*",
                "scale": {
                  "denominator": 2,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, IVampiricSubevent.GetVampirism(subevent).PrettyPrint());
    }

    [Fact(DisplayName = "adds vampirism (customized)")]
    public void AddsVampirismCustomized() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new DummyVampiricSubevent().JoinSubeventData(new JsonObject().LoadFromString(
            """{ "vampirism": [ ] }"""
        ));

        AddVampirism function = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "add_vampirism",
                "vampirism": [
                    { },
                    {
                        "damage_type": "necrotic",
                        "scale": {
                            "numerator": 1,
                            "denominator": 1,
                            "round_up": false
                        }
                    }
                ]
            }
            """);

        FunctionState.StateData result;

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.Equal("""
            [
              {
                "damage_type": "*",
                "scale": {
                  "denominator": 2,
                  "numerator": 1,
                  "round_up": false
                }
              },
              {
                "damage_type": "necrotic",
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, IVampiricSubevent.GetVampirism(subevent).PrettyPrint());
    }

};
