using com.rpglc.core;
using com.rpglc.json;
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
    [Fact(DisplayName = "processes bonus json (range)")]
    public void ProcessesBonusJsonRange() {
        RPGLContext context = new DummyContext();

        RPGLEffect rpglEffect = RPGLFactory.NewEffect("test:dummy");
        Subevent subevent = new DummySubevent();

        JsonObject formulaJson = new JsonObject().LoadFromString("""
            {
                "formula": "range",
                "dice": [
                    { "count": 2, "size": 6, "determined": [ 3 ] }
                ],
                "bonus": 1
            }
            """);

        JsonObject processedBonus = CalculationSubevent.ProcessBonusJson(rpglEffect, subevent, formulaJson, context);

        Assert.Equal(
            """
            {
              "bonus": 1,
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
            processedBonus.PrettyPrint()
        );
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [ExtraClassesMock]
    [ExtraObjectsMock]
    [Fact(DisplayName = "processes bonus json (modifier)")]
    public void ProcessesBonusJsonModifier() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:complex_object", "Player 1");
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

        JsonObject processedBonus = CalculationSubevent.ProcessBonusJson(rpglEffect, subevent, formulaJson, context);

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
    [ExtraObjectsMock]
    [Fact(DisplayName = "processes bonus json (ability)")]
    public void ProcessesBonusJsonAbility() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:complex_object", "Player 1");
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

        JsonObject processedBonus = CalculationSubevent.ProcessBonusJson(rpglEffect, subevent, formulaJson, context);

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
    [Fact(DisplayName = "processes bonus json (proficiency)")]
    public void ProcessesBonusJsonProficiency() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
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

        JsonObject processedBonus = CalculationSubevent.ProcessBonusJson(rpglEffect, subevent, formulaJson, context);

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
    [ExtraObjectsMock]
    [Fact(DisplayName = "processes bonus json (level)")]
    public void ProcessesBonusJsonLevel() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:complex_object", "Player 1");
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

        JsonObject processedBonus = CalculationSubevent.ProcessBonusJson(rpglEffect, subevent, formulaJson, context);

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

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "processes set json (number)")]
    public void ProcessesSetJsonNumber() {
        RPGLContext context = new DummyContext();

        RPGLEffect rpglEffect = RPGLFactory.NewEffect("test:dummy");
        Subevent subevent = new DummySubevent();

        JsonObject formulaJson = new JsonObject().LoadFromString("""
            {
                "formula": "number",
                "number": 2
            }
            """);

        Assert.Equal(2, CalculationSubevent.ProcessSetJson(rpglEffect, subevent, formulaJson, context));
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [ExtraClassesMock]
    [ExtraObjectsMock]
    [Fact(DisplayName = "processes set json (modifier)")]
    public void ProcessesSetJsonModifier() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:complex_object", "Player 1");
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

        Assert.Equal(-2, CalculationSubevent.ProcessSetJson(rpglEffect, subevent, formulaJson, context));
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [ExtraClassesMock]
    [ExtraObjectsMock]
    [Fact(DisplayName = "processes set json (ability)")]
    public void ProcessesSetJsonAbility() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:complex_object", "Player 1");
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

        Assert.Equal(13, CalculationSubevent.ProcessSetJson(rpglEffect, subevent, formulaJson, context));
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "processes set json (proficiency)")]
    public void ProcessesSetJsonProficiency() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
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

        Assert.Equal(2, CalculationSubevent.ProcessSetJson(rpglEffect, subevent, formulaJson, context));
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [ExtraClassesMock]
    [ExtraObjectsMock]
    [Fact(DisplayName = "processes set json (level)")]
    public void ProcessesSetJsonLevel() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:complex_object", "Player 1");
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

        Assert.Equal(1, CalculationSubevent.ProcessSetJson(rpglEffect, subevent, formulaJson, context));
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "processes set json (scale)")]
    public void ProcessesSetJsonScale() {
        RPGLContext context = new DummyContext();

        RPGLEffect rpglEffect = RPGLFactory.NewEffect("test:dummy");
        Subevent subevent = new DummySubevent();

        JsonObject formulaJson = new JsonObject().LoadFromString("""
            {
                "formula": "number",
                "number": 2,
                "scale": {
                    "numerator": 2,
                    "denominator": 1,
                    "round_up": false
                }
            }
            """);

        Assert.Equal(4, CalculationSubevent.ProcessSetJson(rpglEffect, subevent, formulaJson, context));
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "processes set json (round up)")]
    public void ProcessesSetJsonRoundUp() {
        RPGLContext context = new DummyContext();

        RPGLEffect rpglEffect = RPGLFactory.NewEffect("test:dummy");
        Subevent subevent = new DummySubevent();

        JsonObject formulaJson = new JsonObject().LoadFromString("""
            {
                "formula": "number",
                "number": 1,
                "scale": {
                    "numerator": 4,
                    "denominator": 3,
                    "round_up": true
                }
            }
            """);

        Assert.Equal(2, CalculationSubevent.ProcessSetJson(rpglEffect, subevent, formulaJson, context));
    }

    [Fact(DisplayName = "scales")]
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
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
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
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
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
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DummyCalculationSubevent dummyCalculation = (DummyCalculationSubevent) new DummyCalculationSubevent()
            .SetSource(rpglObject)
            .SetTarget(rpglObject);
        dummyCalculation.JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "subevent": "dummy_calculation",
                "bonuses": [
                    {
                        "formula": "range",
                        "bonus": 0,
                        "dice": [
                            { "count": 2, "size": 6, "determined": [ 3 ] }
                        ]
                    },
                    {
                        "formula": "range",
                        "bonus": 2,
                        "dice": [ ]
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
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
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
                        "formula": "range",
                        "bonus": 0,
                        "dice": [
                            { "count": 2, "size": 6, "determined": [ 3 ] }
                        ]
                    },
                    {
                        "formula": "range",
                        "bonus": 2,
                        "dice": [ ]
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
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
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
                        "formula": "range",
                        "bonus": 0,
                        "dice": [
                            { "count": 2, "size": 6, "determined": [ 3 ] }
                        ]
                    },
                    {
                        "formula": "range",
                        "bonus": 2,
                        "dice": [ ]
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
