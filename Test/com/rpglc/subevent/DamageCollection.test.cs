using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class DamageCollectionTest {

    [Fact(DisplayName = "defaults")]
    public void Defaults() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new DamageCollection());

        var result = subevent.Advance(context);
        Assert.Equal((null, false), result);
        Assert.Empty((subevent.subevent as DamageCollection).GetDamageCollection().AsList());

        result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Empty((subevent.subevent as DamageCollection).GetDamageCollection().AsList());
    }

    [Fact(DisplayName = "uses damage (number)")]
    public void UsesDamageNumber() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new DamageCollection()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "formula": "number",
                            "number": 10
                        }
                    ]
                }
                """)));

        // skip over formula segregation
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Equal("""
            [
              {
                "bonus": 10,
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent.subevent as DamageCollection).GetDamageCollection().PrettyPrint());
    }

    [Fact(DisplayName = "uses damage (dice)")]
    public void UsesDamageDice() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new DamageCollection()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "formula": "dice",
                            "dice": [
                                { "count": 1, "size": 6, "determined": [ 3 ] }
                            ]
                        }
                    ]
                }
                """)));

        // skip over formula segregation
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Equal("""
            [
              {
                "bonus": 0,
                "damage_type": "fire",
                "dice": [
                  {
                    "determined": [
                      3
                    ],
                    "size": 6
                  }
                ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent.subevent as DamageCollection).GetDamageCollection().PrettyPrint());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "uses damage (modifier)")]
    public void UsesDamageModifier() {
        long strScore = 12L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.GetAbilityScores().PutLong("str", strScore);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);
        SubeventState subevent = new(new DamageCollection()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "formula": "modifier",
                            "object": {
                                "from": "subevent",
                                "object": "source"
                            },
                            "ability": "str"
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .SetTarget(rpglObject));

        // skip over formula segregation
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateAbilityScore);
        Assert.False(result.completed);

        (result.subevent.subevent as CalculateAbilityScore)
            .SetBase(strScore);

        result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Equal("""
            [
              {
                "bonus": 1,
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent.subevent as DamageCollection).GetDamageCollection().PrettyPrint());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "uses damage (ability)")]
    public void UsesDamageAbility() {
        long strScore = 12L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.GetAbilityScores().PutLong("str", strScore);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);
        SubeventState subevent = new(new DamageCollection()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "formula": "ability",
                            "object": {
                                "from": "subevent",
                                "object": "source"
                            },
                            "ability": "str"
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .SetTarget(rpglObject));

        // skip over formula segregation
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateAbilityScore);
        Assert.False(result.completed);

        (result.subevent.subevent as CalculateAbilityScore)
            .SetBase(strScore);

        result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Equal($$"""
            [
              {
                "bonus": {{strScore}},
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent.subevent as DamageCollection).GetDamageCollection().PrettyPrint());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "uses damage (proficiency)")]
    public void UsesDamageProficiency() {
        long proficiencyBonus = 6L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID)
            .SetProficiencyBonus(proficiencyBonus);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);
        SubeventState subevent = new(new DamageCollection()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "formula": "proficiency",
                            "object": {
                                "from": "subevent",
                                "object": "source"
                            }
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .SetTarget(rpglObject));

        // skip over formula segregation
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateProficiencyBonus);
        Assert.False(result.completed);

        (result.subevent.subevent as CalculateProficiencyBonus)
            .SetBase(proficiencyBonus);

        result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Equal($$"""
            [
              {
                "bonus": {{proficiencyBonus}},
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent.subevent as DamageCollection).GetDamageCollection().PrettyPrint());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [ExtraClassesMock]
    [Fact(DisplayName = "uses damage (level)")]
    public void UsesDamageLevel() {
        long firstClassLevel = 1;
        long secondClassLevel = 2;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID)
            .SetClasses(new JsonArray().LoadFromString($$"""
                [
                    {
                      "additional_nested_classes": { },
                      "id": "test:dummy",
                      "level": {{firstClassLevel}},
                      "name": "Dummy"
                    },
                    {
                      "additional_nested_classes": { },
                      "id": "test:nested_class",
                      "level": {{secondClassLevel}},
                      "name": "Nested Class"
                    }
                ]
                """));
        RPGLContext context = new DummyContext()
            .Add(rpglObject);
        SubeventState subevent = new(new DamageCollection()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "formula": "level",
                            "object": {
                                "from": "subevent",
                                "object": "source"
                            },
                            "class": "test:dummy"
                        },
                        {
                            "damage_type": "fire",
                            "formula": "level",
                            "object": {
                                "from": "subevent",
                                "object": "source"
                            }
                        }
                    ]
                }
                """))
            .SetSource(rpglObject)
            .SetTarget(rpglObject));

        // skip over formula segregation
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal((null, false), result);
        Assert.Equal($$"""
            [
              {
                "bonus": {{firstClassLevel}},
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent.subevent as DamageCollection).GetDamageCollection().PrettyPrint());

        result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Equal($$"""
            [
              {
                "bonus": {{firstClassLevel}},
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              },
              {
                "bonus": {{firstClassLevel + secondClassLevel}},
                "damage_type": "fire",
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, (subevent.subevent as DamageCollection).GetDamageCollection().PrettyPrint());
    }

};
