using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class GrantSkillProficiencyTest {

    [Fact(DisplayName = "grants proficiency (ability check)")]
    public void GrantsProficiencyAbilityCheck() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new AbilityCheck().JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "has_expertise": false,
                "has_half_proficiency": false,
                "has_proficiency": false
            }
            """
        ));

        GrantRollProficiency function = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "grant_skill_proficiency"
            }
            """);

        FunctionState.StateData result;

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.True(subevent.json.GetBool("has_proficiency"));
    }

    [Fact(DisplayName = "grants proficiency (ability save)")]
    public void GrantsProficiencyAbilitySave() {
        RPGLContext context = new DummyContext();
        RPGLEffect rpglEffect = new();
        Subevent subevent = new AbilitySave().JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "has_expertise": false,
                "has_half_proficiency": false,
                "has_proficiency": false
            }
            """
        ));

        GrantRollProficiency function = new();

        JsonObject functionJson = new JsonObject().LoadFromString("""
            {
                "function": "grant_skill_proficiency"
            }
            """);

        FunctionState.StateData result;

        result = function.functionSteps[0](rpglEffect, subevent, functionJson, context);
        Assert.Equal(new FunctionState.StateData() { dependency = null, stepCompleted = true }, result);
        Assert.True(subevent.json.GetBool("has_proficiency"));
    }

};
