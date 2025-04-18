using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class GrantSkillHalfProficiencyTest {

    [Fact(DisplayName = "grants half proficiency")]
    public void GrantsProficiency() {
        Subevent subevent = new AbilityCheck()
            .Prepare(new DummyContext(), new());

        new GrantSkillHalfProficiency().Execute(
            new RPGLEffect(),
            subevent,
            new JsonObject().LoadFromString("""
                {
                    "function": "grant_skill_half_proficiency"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.True(
            (subevent as AbilityCheck).HasHalfProficiency()
        );
    }

};
