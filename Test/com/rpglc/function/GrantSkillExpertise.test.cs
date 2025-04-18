using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class GrantSkillExpertiseTest {

    [Fact(DisplayName = "grants expertise")]
    public void GrantsExpertise() {
        Subevent subevent = new AbilityCheck()
            .Prepare(new DummyContext(), new());

        new GrantSkillExpertise().Execute(
            new RPGLEffect(),
            subevent,
            new JsonObject().LoadFromString("""
                {
                    "function": "grant_skill_expertise"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.True(
            (subevent as AbilityCheck).HasExpertise()
        );
    }

};
