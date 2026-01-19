using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

namespace com.rpglc.subevent;

[Collection("Serial")]
[DieTestingMode]
public class CalculationSubeventTest {

    [Fact(DisplayName = "defaults values")]
    public void DefaultsValues() {
        SubeventState subevent = new(new DummyCalculationSubevent());
        RPGLContext context = new DummyContext();

        var result = subevent.Advance(context);
        Assert.Equal((null, true, false), result);
        Assert.Equal(0, (subevent.subevent as DummyCalculationSubevent).GetBase());
        Assert.Empty((subevent.subevent as DummyCalculationSubevent).GetBonuses().AsList());
        Assert.Equal(long.MinValue, (subevent.subevent as DummyCalculationSubevent).GetMinimum());
    }

    [Fact(DisplayName = "sets base number")]
    public void SetsBaseNumber() {
        SubeventState subevent = new(new DummyCalculationSubevent().JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "base": {
                    "formula": "number",
                    "number": 10
                }
            }
            """)));
        RPGLContext context = new DummyContext();

        // skip past default step
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal((null, true, false), result);
        Assert.Equal(10, (subevent.subevent as DummyCalculationSubevent).GetBase());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "sets base modifier")]
    public void SetsBaseModifier() {
        long strScore = 12;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.GetAbilityScores().PutLong("str", strScore);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SubeventState subevent = new(new DummyCalculationSubevent()
            .SetSource(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "base": {
                        "formula": "modifier",
                        "object": {
                            "from": "subevent",
                            "object": "source"
                        },
                        "ability": "str"
                    }
                }
                """)));
        

        // skip past default step
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateAbilityScore);
        Assert.False(result.canProceed);
        Assert.False(result.completed);
        (result.subevent.subevent as CalculateAbilityScore).JoinSubeventData(new JsonObject().LoadFromString($$"""
            {
                "base": {{strScore}},
                "bonuses": [ ],
                "minimum": 0
            }
            """));

        result = subevent.Advance(context);
        Assert.Equal((null, true, false), result);
        Assert.Equal(1, (subevent.subevent as DummyCalculationSubevent).GetBase());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "sets base ability")]
    public void SetsBaseAbility() {
        long strScore = 12;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.GetAbilityScores().PutLong("str", strScore);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SubeventState subevent = new(new DummyCalculationSubevent()
            .SetSource(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "base": {
                        "formula": "ability",
                        "object": {
                            "from": "subevent",
                            "object": "source"
                        },
                        "ability": "str"
                    }
                }
                """)));


        // skip past default step
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateAbilityScore);
        Assert.False(result.canProceed);
        Assert.False(result.completed);
        (result.subevent.subevent as CalculateAbilityScore).JoinSubeventData(new JsonObject().LoadFromString($$"""
            {
                "base": {{strScore}},
                "bonuses": [ ],
                "minimum": 0
            }
            """));

        result = subevent.Advance(context);
        Assert.Equal((null, true, false), result);
        Assert.Equal(strScore, (subevent.subevent as DummyCalculationSubevent).GetBase());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "sets base proficiency")]
    public void SetsBaseProficiency() {
        long proficiencyBonus = 2;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SubeventState subevent = new(new DummyCalculationSubevent()
            .SetSource(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "base": {
                        "formula": "proficiency",
                        "object": {
                            "from": "subevent",
                            "object": "source"
                        }
                    }
                }
                """)));


        // skip past default step
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateProficiencyBonus);
        Assert.False(result.canProceed);
        Assert.False(result.completed);

        (result.subevent.subevent as CalculateProficiencyBonus).JoinSubeventData(new JsonObject().LoadFromString($$"""
            {
                "base": {{proficiencyBonus}},
                "bonuses": [ ],
                "minimum": 0
            }
            """));

        result = subevent.Advance(context);
        Assert.Equal((null, true, false), result);
        Assert.Equal(proficiencyBonus, (subevent.subevent as DummyCalculationSubevent).GetBase());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "sets base level")]
    public void SetsBaseLevel() {
        long classLevel = 1;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.SetClasses(new JsonArray().LoadFromString($$"""
            [
                {
                  "additional_nested_classes": { },
                  "id": "test:dummy",
                  "level": {{classLevel}},
                  "name": "Nested Class"
                }
            ]
            """));
        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SubeventState subevent = new(new DummyCalculationSubevent()
            .SetSource(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "base": {
                        "formula": "level",
                        "object": {
                            "from": "subevent",
                            "object": "source"
                        },
                        "class": "test:dummy"
                    }
                }
                """)));


        // skip past default step
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal((null, true, false), result);
        Assert.Equal(classLevel, (subevent.subevent as DummyCalculationSubevent).GetBase());
    }

    [Fact(DisplayName = "adds bonus number")]
    public void AddsBonusNumber() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new DummyCalculationSubevent().JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "bonuses": [
                    {
                        "formula": "number",
                        "number": 10
                    }
                ]
            }
            """)));

        // skip past default step
        _ = subevent.Advance(context);
        // skip past base step
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal((null, true, false), result);
        Assert.Equal(10, (subevent.subevent as DummyCalculationSubevent).GetBonus());
    }

    [DieTestingMode]
    [Fact(DisplayName = "adds bonus dice")]
    public void AddsBonusDice() {
        long dieRoll = 3;

        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new DummyCalculationSubevent().JoinSubeventData(new JsonObject().LoadFromString($$"""
            {
                "bonuses": [
                    {
                        "formula": "dice",
                        "dice": [
                            { "count": 1, "size": 6, "determined": [ {{dieRoll}} ] }
                        ]
                    }
                ]
            }
            """)));

        // skip past default step
        _ = subevent.Advance(context);
        // skip past base step
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal((null, true, false), result);
        Assert.Equal(dieRoll, (subevent.subevent as DummyCalculationSubevent).GetBonus());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "adds bonus modifier")]
    public void AddsBonusModifier() {
        long strScore = 12;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.GetAbilityScores().PutLong("str", strScore);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SubeventState subevent = new(new DummyCalculationSubevent()
            .SetSource(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "bonuses": [
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
                """)));

        // skip past default step
        _ = subevent.Advance(context);
        // skip past base step
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateAbilityScore);
        Assert.False(result.canProceed);
        Assert.False(result.completed);
        (result.subevent.subevent as CalculateAbilityScore).JoinSubeventData(new JsonObject().LoadFromString($$"""
            {
                "base": {{strScore}},
                "bonuses": [ ],
                "minimum": 0
            }
            """));

        result = subevent.Advance(context);
        Assert.Equal((null, true, false), result);
        Assert.Equal(1, (subevent.subevent as DummyCalculationSubevent).GetBonus());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "adds bonus ability")]
    public void AddsBonusAbility() {
        long strScore = 12;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.GetAbilityScores().PutLong("str", strScore);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SubeventState subevent = new(new DummyCalculationSubevent()
            .SetSource(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "bonuses": [
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
                """)));

        // skip past default step
        _ = subevent.Advance(context);
        // skip past base step
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateAbilityScore);
        Assert.False(result.canProceed);
        Assert.False(result.completed);
        (result.subevent.subevent as CalculateAbilityScore).JoinSubeventData(new JsonObject().LoadFromString($$"""
            {
                "base": {{strScore}},
                "bonuses": [ ],
                "minimum": 0
            }
            """));

        result = subevent.Advance(context);
        Assert.Equal((null, true, false), result);
        Assert.Equal(strScore, (subevent.subevent as DummyCalculationSubevent).GetBonus());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "adds bonus proficiency")]
    public void AddsBonusProficiency() {
        long proficiencyBonus = 2;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SubeventState subevent = new(new DummyCalculationSubevent()
            .SetSource(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "bonuses": [
                        {
                            "formula": "proficiency",
                            "object": {
                                "from": "subevent",
                                "object": "source"
                            }
                        }
                    ]
                }
                """)));

        // skip past default step
        _ = subevent.Advance(context);
        // skip past base step
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateProficiencyBonus);
        Assert.False(result.canProceed);
        Assert.False(result.completed);

        (result.subevent.subevent as CalculateProficiencyBonus).JoinSubeventData(new JsonObject().LoadFromString($$"""
            {
                "base": {{proficiencyBonus}},
                "bonuses": [ ],
                "minimum": 0
            }
            """));

        result = subevent.Advance(context);
        Assert.Equal((null, true, false), result);
        Assert.Equal(proficiencyBonus, (subevent.subevent as DummyCalculationSubevent).GetBonus());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "adds bonus level")]
    public void AddsBonusLevel() {
        long classLevel = 1;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.SetClasses(new JsonArray().LoadFromString($$"""
            [
                {
                  "additional_nested_classes": { },
                  "id": "test:dummy",
                  "level": {{classLevel}},
                  "name": "Nested Class"
                }
            ]
            """));
        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SubeventState subevent = new(new DummyCalculationSubevent()
            .SetSource(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "bonuses": [
                        {
                            "formula": "level",
                            "object": {
                                "from": "subevent",
                                "object": "source"
                            },
                            "class": "test:dummy"
                        }
                    ]
                }
                """)));

        // skip past default step
        _ = subevent.Advance(context);
        // skip past base step
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal((null, true, false), result);
        Assert.Equal(classLevel, (subevent.subevent as DummyCalculationSubevent).GetBonus());
    }

    [Fact(DisplayName = "sets minimum number")]
    public void SetsMinimumNumber() {
        RPGLContext context = new DummyContext();
        SubeventState subevent = new(new DummyCalculationSubevent().JoinSubeventData(new JsonObject().LoadFromString("""
            {
                "minimum": {
                    "formula": "number",
                    "number": 10
                }
            }
            """)));

        // skip past default step
        _ = subevent.Advance(context);
        // skip past base step
        _ = subevent.Advance(context);
        // skip past bonuses step
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal((null, true, true), result);
        Assert.Equal(10, (subevent.subevent as DummyCalculationSubevent).GetMinimum());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "sets minimum modifier")]
    public void SetsMinimumModifier() {
        long strScore = 12;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.GetAbilityScores().PutLong("str", strScore);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SubeventState subevent = new(new DummyCalculationSubevent()
            .SetSource(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "minimum": {
                        "formula": "modifier",
                        "object": {
                            "from": "subevent",
                            "object": "source"
                        },
                        "ability": "str"
                    }
                }
                """)));

        // skip past default step
        _ = subevent.Advance(context);
        // skip past base step
        _ = subevent.Advance(context);
        // skip past bonuses step
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateAbilityScore);
        Assert.False(result.canProceed);
        Assert.False(result.completed);
        (result.subevent.subevent as CalculateAbilityScore).JoinSubeventData(new JsonObject().LoadFromString($$"""
            {
                "base": {{strScore}},
                "bonuses": [ ],
                "minimum": 0
            }
            """));

        result = subevent.Advance(context);
        Assert.Equal((null, true, true), result);
        Assert.Equal(1, (subevent.subevent as DummyCalculationSubevent).GetMinimum());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "sets minimum ability")]
    public void SetsMinimumAbility() {
        long strScore = 12;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.GetAbilityScores().PutLong("str", strScore);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SubeventState subevent = new(new DummyCalculationSubevent()
            .SetSource(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "minimum": {
                        "formula": "ability",
                        "object": {
                            "from": "subevent",
                            "object": "source"
                        },
                        "ability": "str"
                    }
                }
                """)));

        // skip past default step
        _ = subevent.Advance(context);
        // skip past base step
        _ = subevent.Advance(context);
        // skip past bonuses step
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateAbilityScore);
        Assert.False(result.canProceed);
        Assert.False(result.completed);
        (result.subevent.subevent as CalculateAbilityScore).JoinSubeventData(new JsonObject().LoadFromString($$"""
            {
                "base": {{strScore}},
                "bonuses": [ ],
                "minimum": 0
            }
            """));

        result = subevent.Advance(context);
        Assert.Equal((null, true, true), result);
        Assert.Equal(strScore, (subevent.subevent as DummyCalculationSubevent).GetMinimum());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "sets minimum proficiency")]
    public void SetsMinimumProficiency() {
        long proficiencyBonus = 2;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SubeventState subevent = new(new DummyCalculationSubevent()
            .SetSource(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "minimum": {
                        "formula": "proficiency",
                        "object": {
                            "from": "subevent",
                            "object": "source"
                        }
                    }
                }
                """)));

        // skip past default step
        _ = subevent.Advance(context);
        // skip past base step
        _ = subevent.Advance(context);
        // skip past bonuses step
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateProficiencyBonus);
        Assert.False(result.canProceed);
        Assert.False(result.completed);

        (result.subevent.subevent as CalculateProficiencyBonus).JoinSubeventData(new JsonObject().LoadFromString($$"""
            {
                "base": {{proficiencyBonus}},
                "bonuses": [ ],
                "minimum": 0
            }
            """));

        result = subevent.Advance(context);
        Assert.Equal((null, true, true), result);
        Assert.Equal(proficiencyBonus, (subevent.subevent as DummyCalculationSubevent).GetMinimum());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "sets minimum level")]
    public void SetsMinimumLevel() {
        long classLevel = 1;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.SetClasses(new JsonArray().LoadFromString($$"""
            [
                {
                  "additional_nested_classes": { },
                  "id": "test:dummy",
                  "level": {{classLevel}},
                  "name": "Nested Class"
                }
            ]
            """));
        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SubeventState subevent = new(new DummyCalculationSubevent()
            .SetSource(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "minimum": {
                        "formula": "level",
                        "object": {
                            "from": "subevent",
                            "object": "source"
                        },
                        "class": "test:dummy"
                    }
                }
                """)));

        // skip past default step
        _ = subevent.Advance(context);
        // skip past base step
        _ = subevent.Advance(context);
        // skip past bonuses step
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal((null, true, true), result);
        Assert.Equal(classLevel, (subevent.subevent as DummyCalculationSubevent).GetMinimum());
    }

};
