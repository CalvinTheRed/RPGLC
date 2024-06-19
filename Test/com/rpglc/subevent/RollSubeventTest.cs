using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.testutils.mocks;
using com.rpglc.testutils;

namespace com.rpglc.subevent;

[AssignDatabase]
[DieTestingMode]
[Collection("Serial")]
public class RollSubeventTest {

    private class DummyRollSubevent : RollSubevent {
        public DummyRollSubevent() : base("dummy_roll_subevent") { }

        public override Subevent Clone() {
            DummyRollSubevent clone = new DummyRollSubevent();
            clone.JoinSubeventData(json);
            clone.appliedEffects.AddRange(appliedEffects);
            return clone;
        }

        public override Subevent Clone(JsonObject jsonData) {
            DummyRollSubevent clone = new DummyRollSubevent();
            clone.JoinSubeventData(jsonData);
            clone.appliedEffects.AddRange(appliedEffects);
            return clone;
        }

        public override DummyRollSubevent? Invoke(RPGLContext context, JsonArray originPoint) {
            return (DummyRollSubevent?) base.Invoke(context, originPoint);
        }

        public override DummyRollSubevent JoinSubeventData(JsonObject other) {
            return (DummyRollSubevent) base.JoinSubeventData(other);
        }

        public override DummyRollSubevent Prepare(RPGLContext context, JsonArray originPoint) {
            return (DummyRollSubevent) base.Prepare(context, originPoint);
        }

        public override Subevent Run(RPGLContext context, JsonArray originPoint) {
            return this;
        }

        public override DummyRollSubevent SetOriginItem(string? originItem) {
            return (DummyRollSubevent) base.SetOriginItem(originItem);
        }

        public override DummyRollSubevent SetSource(RPGLObject source) {
            return (DummyRollSubevent) base.SetSource(source);
        }

        public override DummyRollSubevent SetTarget(RPGLObject target) {
            return (DummyRollSubevent) base.SetTarget(target);
        }
    };

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "prepares")]
    public void Prepares() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DummyRollSubevent dummyRollSubevent = new DummyRollSubevent()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.False(dummyRollSubevent.json.GetBool("has_advantage"));
        Assert.False(dummyRollSubevent.json.GetBool("has_disadvantage"));
    }

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "rolls with advantage")]
    public void RollsWithAdvantage() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DummyRollSubevent dummyRollSubevent = new DummyRollSubevent()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "determined": [ 5, 10, -1 ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        dummyRollSubevent.GrantAdvantage();

        Assert.True(dummyRollSubevent.json.GetBool("has_advantage"));
        Assert.False(dummyRollSubevent.json.GetBool("has_disadvantage"));
        Assert.True(dummyRollSubevent.IsAdvantageRoll());
        Assert.False(dummyRollSubevent.IsDisadvantageRoll());
        Assert.False(dummyRollSubevent.IsNormalRoll());

        dummyRollSubevent.Roll();

        Assert.Equal(10, dummyRollSubevent.Get());
    }

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "rolls with disadvantage")]
    public void RollsWithDisadvantage() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DummyRollSubevent dummyRollSubevent = new DummyRollSubevent()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "determined": [ 10, 5, -1 ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        dummyRollSubevent.GrantDisadvantage();

        Assert.False(dummyRollSubevent.json.GetBool("has_advantage"));
        Assert.True(dummyRollSubevent.json.GetBool("has_disadvantage"));
        Assert.False(dummyRollSubevent.IsAdvantageRoll());
        Assert.True(dummyRollSubevent.IsDisadvantageRoll());
        Assert.False(dummyRollSubevent.IsNormalRoll());

        dummyRollSubevent.Roll();

        Assert.Equal(5, dummyRollSubevent.Get());
    }

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "rolls normally")]
    public void RollsNormally() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DummyRollSubevent dummyRollSubevent = new DummyRollSubevent()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "determined": [ 5, -1 ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.True(dummyRollSubevent.IsNormalRoll());

        dummyRollSubevent.GrantAdvantage();
        dummyRollSubevent.GrantDisadvantage();

        Assert.False(dummyRollSubevent.IsAdvantageRoll());
        Assert.False(dummyRollSubevent.IsDisadvantageRoll());
        Assert.True(dummyRollSubevent.IsNormalRoll());

        dummyRollSubevent.Roll();

        Assert.Equal(5, dummyRollSubevent.Get());
    }

};
