using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.condition;

public class IncludesDamageTypeTest {

    [Fact(DisplayName = "condition mismatch")]
    public void ConditionMismatch() {
        bool result = new IncludesDamageType().Evaluate(new(), new DummySubevent(), new JsonObject().LoadFromString("""
            {
                "condition": "not-a-condition"
            }
            """), new DummyContext(), new());

        Assert.False(result);
    }

    [Fact(DisplayName = "inapplicable subevent")]
    public void InapplicableSubevent() {
        bool result = new IncludesDamageType().Evaluate(new(), new CalculateArmorClass(), new JsonObject().LoadFromString("""
            {
                "condition": "includes_damage_ttype",
                "damage_type": "fire"
            }
            """), new DummyContext(), new());

        Assert.False(result);
    }

    [Fact(DisplayName = "does include damage type")]
    public void DoesIncludeDamageType() {
        bool result = new IncludesDamageType().Evaluate(new(), new DummySubevent(), new JsonObject().LoadFromString("""
            {
                "condition": "includes_damage_type",
                "damage_type": "fire"
            }
            """), new DummyContext(), new());

        Assert.True(result);
    }

    [Fact(DisplayName = "does not include damage type")]
    public void DoesNotIncludeDamageType() {
        bool result = new IncludesDamageType().Evaluate(new(), new DummySubevent(), new JsonObject().LoadFromString("""
            {
                "condition": "includes_damage_type",
                "damage_type": "cold"
            }
            """), new DummyContext(), new());

        Assert.False(result);
    }

};
