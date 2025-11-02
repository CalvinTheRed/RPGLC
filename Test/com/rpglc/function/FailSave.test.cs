using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.function;

[Collection("Serial")]
[RPGLInitTesting]
public class FailSaveTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [DummyCounterManager]
    [Fact(DisplayName = "forcibly fails save")]
    public void ForciblyFailsSave() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SavingThrow savingThrow = new SavingThrow()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "save_ability": "dex",
                    "difficulty_class": 1,
                    "damage": [ ],
                    "damage_on_pass": "none",
                    "pass": [ ],
                    "fail": [
                        {
                            "subevent": "dummy_subevent"
                        }
                    ],
                    "determined": [ 20 ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(context, new());

        new FailSave().Execute(
            new RPGLEffect(),
            savingThrow,
            new JsonObject().LoadFromString("""
                {
                    "function": "fail_save"
                }
                """),
            context,
            new()
        );

        savingThrow
            .SetTarget(rpglObject)
            .Invoke(context, new());

        Assert.Equal(1, DummySubevent.Counter);
    }

};
