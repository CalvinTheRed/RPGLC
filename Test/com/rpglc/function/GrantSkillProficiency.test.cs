using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
public class GrantSkillProficiencyTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "grants proficiency")]
    public void GrantsProficiency() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        Subevent subevent = new AbilityCheck()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        new GrantSkillProficiency().Execute(
            new RPGLEffect(),
            subevent,
            new JsonObject().LoadFromString("""
                {
                    "function": "grant_skill_proficiency"
                }
                """),
            new DummyContext(),
            new()
        );

        Assert.True(
            (subevent as AbilityCheck).HasProficiency()
        );
    }

};
