using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.condition;

[Collection("Serial")]
public class CheckAbilityTest {

    [Fact(DisplayName = "condition mismatch")]
    public void ConditionMismatch() {
        bool result = new CheckAbility().Evaluate(new(), new DummySubevent(), new JsonObject().LoadFromString("""
            {
                "condition": "not-a-condition"
            }
            """), new DummyContext(), new());

        Assert.False(result);
    }

    [Fact(DisplayName = "inapplicable subevent")]
    public void InapplicableSubevent() {
        bool result = new CheckAbility().Evaluate(new(), new DamageAffinity(), new JsonObject().LoadFromString("""
            {
                "condition": "check_ability",
                "ability": "str"
            }
            """), new DummyContext(), new());

        Assert.False(result);
    }

    [Fact(DisplayName = "ability match")]
    public void AbilityMatch() {
        bool result = new CheckAbility().Evaluate(new(), new DummySubevent(), new JsonObject().LoadFromString("""
            {
                "condition": "check_ability",
                "ability": "str"
            }
            """), new DummyContext(), new());

        Assert.True(result);
    }

    [Fact(DisplayName = "ability mismatch")]
    public void AbilityMismatch() {
        bool result = new CheckAbility().Evaluate(new(), new DummySubevent(), new JsonObject().LoadFromString("""
            {
                "condition": "check_ability",
                "ability": "dex"
            }
            """), new DummyContext(), new());

        Assert.False(result);
    }

};
