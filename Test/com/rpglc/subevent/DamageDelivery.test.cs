using com.rpglc.core;
using com.rpglc.database;
using com.rpglc.json;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[AssignDatabase]
[Collection("Serial")]
public class DamageDeliveryTest {

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares")]
    public void Prepares() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DamageDelivery damageDelivery = new DamageDelivery()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal("""[]""", damageDelivery.json.GetJsonArray("damage").ToString());
        Assert.Equal("all", damageDelivery.json.GetString("damage_proportion"));
    }

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "delivers all damage")]
    public void DeliversAllDamage() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DamageDelivery damageDelivery = new DamageDelivery()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": 10,
                            "dice": [ ],
                            "scale": {
                                "numerator": 1,
                                "denominator": 1,
                                "round_up": false
                            }
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .SetTarget(rpglObject)
            .Invoke(new DummyContext(), new());

        rpglObject = DBManager.QueryRPGLObject(x => x._id == rpglObject.GetId());

        Assert.Equal(1000 - 10, rpglObject.GetHealthCurrent());
    }

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "delivers half damage")]
    public void DeliversHalfDamage() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DamageDelivery damageDelivery = new DamageDelivery()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage_proportion": "half",
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": 10,
                            "dice": [ ],
                            "scale": {
                                "numerator": 1,
                                "denominator": 1,
                                "round_up": false
                            }
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .SetTarget(rpglObject)
            .Invoke(new DummyContext(), new());

        rpglObject = DBManager.QueryRPGLObject(x => x._id == rpglObject.GetId());

        Assert.Equal(1000 - 5, rpglObject.GetHealthCurrent());
    }

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "delivers no damage")]
    public void DeliversNoDamage() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DamageDelivery damageDelivery = new DamageDelivery()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage_proportion": "none",
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": 10,
                            "dice": [ ],
                            "scale": {
                                "numerator": 1,
                                "denominator": 1,
                                "round_up": false
                            }
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new())
            .SetTarget(rpglObject)
            .Invoke(new DummyContext(), new());

        rpglObject = DBManager.QueryRPGLObject(x => x._id == rpglObject.GetId());

        Assert.Equal(1000 - 0, rpglObject.GetHealthCurrent());
    }

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [ExtraEffectsMock]
    [Fact(DisplayName = "delivers immune damage")]
    [RPGLCInit]
    public void DeliversImmuneDamage() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");

        RPGLEffect damageImmunity = RPGLFactory.NewEffect("test:damage_immunity");
        rpglObject.AddEffect(damageImmunity);

        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        DamageDelivery damageDelivery = new DamageDelivery()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": 10,
                            "dice": [ ],
                            "scale": {
                                "numerator": 1,
                                "denominator": 1,
                                "round_up": false
                            }
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(context, new())
            .SetTarget(rpglObject)
            .Invoke(context, new());

        rpglObject = DBManager.QueryRPGLObject(x => x._id == rpglObject.GetId());

        Assert.Equal(1000 - 0, rpglObject.GetHealthCurrent());
    }

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [ExtraEffectsMock]
    [Fact(DisplayName = "delivers resistance damage")]
    [RPGLCInit]
    public void DeliversResistanceDamage() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");

        RPGLEffect damageResistance = RPGLFactory.NewEffect("test:damage_resistance");
        rpglObject.AddEffect(damageResistance);

        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        DamageDelivery damageDelivery = new DamageDelivery()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": 11,
                            "dice": [ ],
                            "scale": {
                                "numerator": 1,
                                "denominator": 1,
                                "round_up": false
                            }
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(context, new())
            .SetTarget(rpglObject)
            .Invoke(context, new());

        rpglObject = DBManager.QueryRPGLObject(x => x._id == rpglObject.GetId());

        Assert.Equal(1000 - 5, rpglObject.GetHealthCurrent());
    }

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [ExtraEffectsMock]
    [Fact(DisplayName = "delivers vulnerability damage")]
    [RPGLCInit]
    public void DeliversVulnerabilityDamage() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");

        RPGLEffect damageResistance = RPGLFactory.NewEffect("test:damage_vulnerability");
        rpglObject.AddEffect(damageResistance);

        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        DamageDelivery damageDelivery = new DamageDelivery()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "bonus": 10,
                            "dice": [ ],
                            "scale": {
                                "numerator": 1,
                                "denominator": 1,
                                "round_up": false
                            }
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .Prepare(context, new())
            .SetTarget(rpglObject)
            .Invoke(context, new());

        rpglObject = DBManager.QueryRPGLObject(x => x._id == rpglObject.GetId());

        Assert.Equal(1000 - 20, rpglObject.GetHealthCurrent());
    }

};
