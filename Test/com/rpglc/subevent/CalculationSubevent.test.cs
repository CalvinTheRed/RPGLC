using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.subevent;

[Collection("Serial")]
[DieTestingMode]
public class CalculationSubeventTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "simplifies number formula")]
    public void SimplifiesNumberFormula() {
        RPGLContext context = new DummyContext();

        RPGLEffect rpglEffect = RPGLFactory.NewEffect("test:dummy");
        Subevent subevent = new DummySubevent();

        JsonObject formulaJson = new JsonObject().LoadFromString("""
            {
                "formula": "number",
                "number": 1
            }
            """);

        JsonObject simplifiedFormula = CalculationSubevent.SimplifyCalculationFormulaJson(rpglEffect, subevent, formulaJson, context);

        Assert.Equal(
            """
            {
              "bonus": 1,
              "dice": [ ],
              "scale": {
                "denominator": 1,
                "numerator": 1,
                "round_up": false
              }
            }
            """,
            simplifiedFormula.PrettyPrint()
        );
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "simplifies dice formula")]
    public void SimplifiesDiceFormula() {
        RPGLContext context = new DummyContext();

        RPGLEffect rpglEffect = RPGLFactory.NewEffect("test:dummy");
        Subevent subevent = new DummySubevent();

        JsonObject formulaJson = new JsonObject().LoadFromString("""
            {
                "formula": "dice",
                "dice": [
                    { "count": 2, "size": 6, "determined": [ 3 ] }
                ]
            }
            """);

        JsonObject simplifiedFormula = CalculationSubevent.SimplifyCalculationFormulaJson(rpglEffect, subevent, formulaJson, context);

        Assert.Equal(
            """
            {
              "bonus": 0,
              "dice": [
                {
                  "determined": [
                    3
                  ],
                  "size": 6
                },
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
            """,
            simplifiedFormula.PrettyPrint()
        );
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [ExtraClassesMock]
    [ExtraEffectsMock]
    [ExtraObjectsMock]
    [Fact(DisplayName = "simplifies modifier formula")]
    public void SimplifiesModifierFormula() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:complex_object", TestUtils.USER_ID);
        RPGLContext context = new DummyContext().Add(rpglObject);

        RPGLEffect rpglEffect = RPGLFactory.NewEffect("test:dummy");
        Subevent subevent = new DummySubevent();
        subevent.SetSource(rpglObject);

        JsonObject formulaJson = new JsonObject().LoadFromString("""
            {
                "formula": "modifier",
                "ability": "cha",
                "object": {
                    "from": "subevent",
                    "object": "source"
                }
            }
            """);

        JsonObject processedBonus = CalculationSubevent.SimplifyCalculationFormulaJson(rpglEffect, subevent, formulaJson, context);

        Assert.Equal(
            """
            {
              "bonus": -2,
              "dice": [ ],
              "scale": {
                "denominator": 1,
                "numerator": 1,
                "round_up": false
              }
            }
            """,
            processedBonus.PrettyPrint()
        );
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [ExtraClassesMock]
    [ExtraEffectsMock]
    [ExtraObjectsMock]
    [Fact(DisplayName = "simplifies ability formula")]
    public void SimplifiesAbilityFormula() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:complex_object", TestUtils.USER_ID);
        RPGLContext context = new DummyContext().Add(rpglObject);

        RPGLEffect rpglEffect = RPGLFactory.NewEffect("test:dummy");
        Subevent subevent = new DummySubevent();
        subevent.SetSource(rpglObject);

        JsonObject formulaJson = new JsonObject().LoadFromString("""
            {
                "formula": "ability",
                "ability": "str",
                "object": {
                    "from": "subevent",
                    "object": "source"
                }
            }
            """);

        JsonObject processedBonus = CalculationSubevent.SimplifyCalculationFormulaJson(rpglEffect, subevent, formulaJson, context);

        Assert.Equal(
            """
            {
              "bonus": 13,
              "dice": [ ],
              "scale": {
                "denominator": 1,
                "numerator": 1,
                "round_up": false
              }
            }
            """,
            processedBonus.PrettyPrint()
        );
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "simplifies proficiency formula")]
    public void SimplifiesProficiencyFormula() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext().Add(rpglObject);

        RPGLEffect rpglEffect = RPGLFactory.NewEffect("test:dummy");
        Subevent subevent = new DummySubevent();
        subevent.SetSource(rpglObject);

        JsonObject formulaJson = new JsonObject().LoadFromString("""
            {
                "formula": "proficiency",
                "object": {
                    "from": "subevent",
                    "object": "source"
                }
            }
            """);

        JsonObject processedBonus = CalculationSubevent.SimplifyCalculationFormulaJson(rpglEffect, subevent, formulaJson, context);

        Assert.Equal(
            """
            {
              "bonus": 2,
              "dice": [ ],
              "scale": {
                "denominator": 1,
                "numerator": 1,
                "round_up": false
              }
            }
            """,
            processedBonus.PrettyPrint()
        );
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [ExtraClassesMock]
    [ExtraEffectsMock]
    [ExtraObjectsMock]
    [Fact(DisplayName = "simplifies level formula")]
    public void SimplifiesLevelFormula() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:complex_object", TestUtils.USER_ID);
        RPGLContext context = new DummyContext().Add(rpglObject);

        RPGLEffect rpglEffect = RPGLFactory.NewEffect("test:dummy");
        Subevent subevent = new DummySubevent();
        subevent.SetSource(rpglObject);

        JsonObject formulaJson = new JsonObject().LoadFromString("""
            {
                "formula": "level",
                "class": "test:class_with_nested_class",
                "object": {
                    "from": "subevent",
                    "object": "source"
                }
            }
            """);

        JsonObject processedBonus = CalculationSubevent.SimplifyCalculationFormulaJson(rpglEffect, subevent, formulaJson, context);

        Assert.Equal(
            """
            {
              "bonus": 1,
              "dice": [ ],
              "scale": {
                "denominator": 1,
                "numerator": 1,
                "round_up": false
              }
            }
            """,
            processedBonus.PrettyPrint()
        );
    }

    [Fact(DisplayName = "scales with defaults")]
    public void ScalesWithDefaults() {
        Assert.Equal(10, CalculationSubevent.Scale(10, new()));

        Assert.Equal(8, CalculationSubevent.Scale(5, new JsonObject().LoadFromString("""
            {
                "numerator": 3,
                "denominator": 2,
                "round_up": true
            }
            """)));
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares base")]
    public void PreparesBase() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        DummyCalculationSubevent dummyCalculationSubevent = (DummyCalculationSubevent) new DummyCalculationSubevent()
            .SetSource(rpglObject)
            .SetTarget(rpglObject);
        dummyCalculationSubevent.JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "base": {
                    "formula": "number",
                    "number": 2
                }
            }
            """));
        dummyCalculationSubevent.PrepareBase(new DummyContext());

        Assert.Equal(2, dummyCalculationSubevent.GetBase());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares minimum")]
    public void PreparesMinimum() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        DummyCalculationSubevent dummyCalculation = (DummyCalculationSubevent) new DummyCalculationSubevent()
            .SetSource(rpglObject)
            .SetTarget(rpglObject);
        dummyCalculation.JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "subevent": "dummy_calculation",
                "minimum": {
                    "formula": "number",
                    "number": 2
                }
            }
            """));
        dummyCalculation.PrepareMinimum(new DummyContext());

        Assert.Equal(2, dummyCalculation.GetMinimum());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares bonuses")]
    public void PreparesBonuses() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        DummyCalculationSubevent dummyCalculation = (DummyCalculationSubevent) new DummyCalculationSubevent()
            .SetSource(rpglObject)
            .SetTarget(rpglObject);
        dummyCalculation.JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "subevent": "dummy_calculation",
                "bonuses": [
                    {
                        "formula": "dice",
                        "dice": [
                            { "count": 2, "size": 6, "determined": [ 3 ] }
                        ]
                    },
                    {
                        "formula": "number",
                        "number": 2
                    }
                ]
            }
            """));
        dummyCalculation.PrepareBonuses(new DummyContext());

        Assert.Equal(
            """
            [
              {
                "bonus": 0,
                "dice": [
                  {
                    "determined": [
                      3
                    ],
                    "size": 6
                  },
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
              },
              {
                "bonus": 2,
                "dice": [ ],
                "scale": {
                  "denominator": 1,
                  "numerator": 1,
                  "round_up": false
                }
              }
            ]
            """, dummyCalculation.GetBonuses().PrettyPrint());

        Assert.Equal(3 + 3 + 2, dummyCalculation.GetBonus());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "gets")]
    public void Gets() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        DummyCalculationSubevent dummyCalculation = (DummyCalculationSubevent) new DummyCalculationSubevent()
            .SetSource(rpglObject)
            .SetTarget(rpglObject);
        dummyCalculation.JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "subevent": "dummy_calculation",
                "base": {
                    "formula": "number",
                    "number": 2
                },
                "bonuses": [
                    {
                        "formula": "dice",
                        "dice": [
                            { "count": 2, "size": 6, "determined": [ 3 ] }
                        ]
                    },
                    {
                        "formula": "number",
                        "number": 2
                    }
                ]
            }
            """));
        dummyCalculation.Prepare(new DummyContext(), new());

        Assert.Equal(10, dummyCalculation.Get());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "gets minimum")]
    public void GetsMinimum() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        DummyCalculationSubevent dummyCalculation = (DummyCalculationSubevent) new DummyCalculationSubevent()
            .SetSource(rpglObject)
            .SetTarget(rpglObject);
        dummyCalculation.JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "subevent": "dummy_calculation",
                "base": {
                    "formula": "number",
                    "number": 2
                },
                "bonuses": [
                    {
                        "formula": "dice",
                        "dice": [
                            { "count": 2, "size": 6, "determined": [ 3 ] }
                        ]
                    },
                    {
                        "formula": "number",
                        "number": 2
                    }
                ],
                "minimum": {
                    "formula": "number",
                    "number": 25
                }
            }
            """));
        dummyCalculation.Prepare(new DummyContext(), new());

        Assert.Equal(25, dummyCalculation.Get());
    }

};
