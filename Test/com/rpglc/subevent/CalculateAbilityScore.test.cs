using com.rpglc.json;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

public class CalculateAbilityScoreTest {

    [Fact(DisplayName = "gets ability")]
    public void GetsABility() {
        CalculateAbilityScore calculateAbilityScore = new CalculateAbilityScore()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "ability": "str"
                }
                """));

        Assert.Equal("str", calculateAbilityScore.GetAbility(new DummyContext()));
    }

};
