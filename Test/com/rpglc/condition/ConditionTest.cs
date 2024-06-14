namespace com.rpglc.condition;


public class ConditionTest {

    [Fact(DisplayName = "compares values (=)")]
    public void ComparesValuesEquals() {
        Assert.True(Condition.CompareValues(1, "=", 1));
        Assert.False(Condition.CompareValues(1, "=", 0));
        Assert.False(Condition.CompareValues(0, "=", 1));
    }

    [Fact(DisplayName = "compares values (!=)")]
    public void ComparesValuesNotEquals() {
        Assert.False(Condition.CompareValues(1, "!=", 1));
        Assert.True(Condition.CompareValues(1, "!=", 0));
        Assert.True(Condition.CompareValues(0, "!=", 1));
    }

    [Fact(DisplayName = "compares values (<)")]
    public void ComparesValuesLessThan() {
        Assert.False(Condition.CompareValues(1, "<", 1));
        Assert.False(Condition.CompareValues(1, "<", 0));
        Assert.True(Condition.CompareValues(0, "<", 1));
    }

    [Fact(DisplayName = "compares values (>)")]
    public void ComparesValuesGreaterThan() {
        Assert.False(Condition.CompareValues(1, ">", 1));
        Assert.True(Condition.CompareValues(1, ">", 0));
        Assert.False(Condition.CompareValues(0, ">", 1));
    }

    [Fact(DisplayName = "compares values (<=)")]
    public void ComparesValuesLessThanOrEqualTo() {
        Assert.True(Condition.CompareValues(1, "<=", 1));
        Assert.False(Condition.CompareValues(1, "<=", 0));
        Assert.True(Condition.CompareValues(0, "<=", 1));
    }

    [Fact(DisplayName = "compares values (>=)")]
    public void ComparesValuesGreaterThanOrEqualTo() {
        Assert.True(Condition.CompareValues(1, ">=", 1));
        Assert.True(Condition.CompareValues(1, ">=", 0));
        Assert.False(Condition.CompareValues(0, ">=", 1));
    }

    [Fact(DisplayName = "defaults false")]
    public void DefaultsFalse() {
        Assert.False(Condition.CompareValues(1, "not-a-comparison", 1));
    }

};
