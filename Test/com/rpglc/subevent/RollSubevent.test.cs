using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.subevent;

[AssignDatabase]
[Collection("Serial")]
public class RollSubeventTest {

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares")]
    public void Prepares() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DummyRollSubevent dummyRollSubevent = new DummyRollSubevent()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.False(dummyRollSubevent.json.GetBool("has_advantage"));
        Assert.False(dummyRollSubevent.json.GetBool("has_disadvantage"));
    }

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
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

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
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

    [ClearDatabaseAfterTest]
    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
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
