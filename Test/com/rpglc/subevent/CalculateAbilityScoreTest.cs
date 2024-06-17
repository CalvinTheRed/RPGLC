using com.rpglc.testutils;

namespace com.rpglc.subevent;

public class CalculateAbilityScoreTest {

    [Fact(DisplayName = "gets ability")]
    public void GetsABility() {
        CalculateAbilityScore calculateAbilityScore = new CalculateAbilityScore()
            .JoinSubeventData(new json.JsonObject()
                .PutString("ability", "str")
            );

        Assert.Equal("str", calculateAbilityScore.GetAbility(new DummyContext()));
    }

};
