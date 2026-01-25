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
            .SetBase(strScore)
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "bonuses": [ ]
                }
                """));

        result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Equal("""
            [
              {
                "bonus": 1,
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
            .SetBase(strScore)
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "bonuses": [ ]
                }
                """));

        result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Equal($$"""
            [
              {
                "bonus": {{strScore}},
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
            .SetBase(proficiencyBonus)
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "bonuses": [ ]
                }
                """));

        result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Equal($$"""
            [
              {
                "bonus": {{proficiencyBonus}},
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
                            "formula": "level",
                            "object": {
                                "from": "subevent",
                                "object": "source"
                            },
                            "class": "test:dummy"
                        },
                        {
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
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              },
              {
                "bonus": {{firstClassLevel + secondClassLevel}},
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
