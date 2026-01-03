using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.function;

[Collection("Serial")]
[RPGLInitTesting]
public class CritOnHitTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DieTestingMode]
    [DummyCounterManager]
    [Fact(DisplayName = "crits on hit")]
    public void CritsOnHit() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        AttackRoll attackRoll = new AttackRoll()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "ability": "str",
                    "attack_type": "melee",
                    "determined": [ 19 ],
                    "damage": [
                        {
                            "formula": "dice",
                            "damage_type": "fire",
                            "dice": [
                                { "count": 1, "size": 6, "determined": [ 3 ] }
                            ]
                        },
                        {
                            "formula": "number",
                            "damage_type": "fire",
                            "number": 1
                        }
                    ],
                    "hit": [ ],
                    "miss": [ ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(context, new());

        new CritOnHit().Execute(
            new RPGLEffect(),
            attackRoll,
            new JsonObject().LoadFromString("""
                {
                    "function": "crit_on_hit"
                }
                """),
            context,
            new()
        );

        attackRoll
            .SetTarget(rpglObject)
            .Invoke(context, new());

        Assert.Equal(1000 - 1 - 3 - 3, rpglObject.GetHealthCurrent());
    }

};
