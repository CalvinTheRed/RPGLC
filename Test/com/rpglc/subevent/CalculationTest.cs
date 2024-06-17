using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.testutils;
using com.rpglc.testutils.mocks;

namespace com.rpglc.subevent;

public class DummyCalculation : CalculationSubevent {
    public DummyCalculation() : base("dummy_calculation") { }

    public override Subevent Clone() {
        return this;
    }

    public override Subevent Clone(JsonObject jsonData) {
        return this;
    }

    public override Subevent Run(RPGLContext context, JsonArray originPoint) {
        return this;
    }
};

[DieTestingMode]
[AssignDatabase]
[Collection("Serial")]
public class CalculationTest {

    [DefaultMock]
    [ClearDatabaseAfterTest]
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

    [DefaultMock]
    [ExtraObjectsMock]
    [ExtraClassesMock]
    [ClearDatabaseAfterTest]
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

    [DefaultMock]
    [ExtraObjectsMock]
    [ExtraClassesMock]
    [ClearDatabaseAfterTest]
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

    [DefaultMock]
    [ClearDatabaseAfterTest]
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

    [DefaultMock]
    [ExtraObjectsMock]
    [ExtraClassesMock]
    [ClearDatabaseAfterTest]
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

    [DefaultMock]
    [ClearDatabaseAfterTest]
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

    [DefaultMock]
    [ExtraObjectsMock]
    [ExtraClassesMock]
    [ClearDatabaseAfterTest]
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

    [DefaultMock]
    [ExtraObjectsMock]
    [ExtraClassesMock]
    [ClearDatabaseAfterTest]
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

    [DefaultMock]
    [ClearDatabaseAfterTest]
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

    [DefaultMock]
    [ExtraObjectsMock]
    [ExtraClassesMock]
    [ClearDatabaseAfterTest]
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

    [DefaultMock]
    [ClearDatabaseAfterTest]
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

    [DefaultMock]
    [ClearDatabaseAfterTest]
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

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "prepares base")]
    public void PreparesBase() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DummyCalculation dummyCalculation = (DummyCalculation) new DummyCalculation()
            .SetSource(rpglObject)
            .SetTarget(rpglObject);
        dummyCalculation.JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "subevent": "dummy_calculation",
                "base": {
                    "formula": "number",
                    "number": 2
                }
            }
            """));
        dummyCalculation.PrepareBase(new DummyContext());

        Assert.Equal(2, dummyCalculation.GetBase());
    }

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "prepares minimum")]
    public void PreparesMinimum() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DummyCalculation dummyCalculation = (DummyCalculation) new DummyCalculation()
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

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "prepares bonuses")]
    public void PreparesBonuses() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DummyCalculation dummyCalculation = (DummyCalculation) new DummyCalculation()
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

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "gets")]
    public void Gets() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DummyCalculation dummyCalculation = (DummyCalculation) new DummyCalculation()
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

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "gets minimum")]
    public void GetsMinimum() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        DummyCalculation dummyCalculation = (DummyCalculation) new DummyCalculation()
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
