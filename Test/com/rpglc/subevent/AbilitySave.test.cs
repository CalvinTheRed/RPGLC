using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
[DieTestingMode]
[RPGLInitTesting]
public class AbilitySaveTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "uses difficulty class (calculated)")]
    public void UsesDifficultyClassCalculated() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SubeventState subevent = new(new AbilitySave()
            .SetSource(rpglObject)
            .SetTarget(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "difficulty_class_ability": "int"
                }
                """)));

        // skip inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateDifficultyClass);
        Assert.Equal($$"""
            {
              "bonuses": [ ],
              "difficulty_class_ability": "int",
              "source": "{{rpglObject.GetUuid()}}",
              "subevent": "calculate_difficulty_class",
              "tags": [
                "calculate_difficulty_class",
                "ability_save"
              ],
              "target": "{{rpglObject.GetUuid()}}"
            }
            """, result.subevent.subevent.PrettyPrint());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "uses difficulty class (assigned)")]
    public void UsesDifficultyClassAssigned() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SubeventState subevent = new(new AbilitySave()
            .SetSource(rpglObject)
            .SetTarget(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "difficulty_class": 12
                }
                """)));

        // skip inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateDifficultyClass);
        Assert.Equal($$"""
            {
              "bonuses": [ ],
              "difficulty_class": 12,
              "source": "{{rpglObject.GetUuid()}}",
              "subevent": "calculate_difficulty_class",
              "tags": [
                "calculate_difficulty_class",
                "ability_save"
              ],
              "target": "{{rpglObject.GetUuid()}}"
            }
            """, result.subevent.subevent.PrettyPrint());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "uses difficulty class (as origin)")]
    public void UsesDifficultyClassAsOrigin() {
        RPGLObject origin = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID)
            .SetOriginObject(origin.GetUuid());
        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SubeventState subevent = new(new AbilitySave()
            .SetSource(rpglObject)
            .SetTarget(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "difficulty_class_ability": "int",
                    "use_origin_difficulty_class_ability": true
                }
                """)));

        // skip inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateDifficultyClass);
        Assert.Equal($$"""
            {
              "bonuses": [ ],
              "difficulty_class_ability": "int",
              "source": "{{origin.GetUuid()}}",
              "subevent": "calculate_difficulty_class",
              "tags": [
                "calculate_difficulty_class",
                "ability_save"
              ],
              "target": "{{origin.GetUuid()}}"
            }
            """, result.subevent.subevent.PrettyPrint());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "deals damage (all on fail)")]
    public void DealsDamageAllOnFail() {
        long difficultyClass = 10L;
        long savingThrowRoll = 1L;
        long abilityScore = 10L;
        long damage = 10L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SubeventState subevent = new(new AbilitySave()
            .SetSource(rpglObject)
            .SetTarget(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "difficulty_class": {{difficultyClass}},
                    "determined": [ {{savingThrowRoll}} ],
                    "ability": "dex",
                    "damage": [
                        {
                            "formula": "number",
                            "number": {{damage}}
                        }
                    ]
                }
                """)));

        // skip inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateDifficultyClass);
        Assert.False(result.completed);

        result.subevent.subevent.JoinSubeventData(new JsonObject().LoadFromString($$"""
            {
                "base": {{difficultyClass}},
                "bonuses": [ ],
                "minimum": 0
            }
            """));

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageCollection);
        Assert.False(result.completed);
        Assert.Equal(difficultyClass, subevent.subevent.json.GetLong("difficulty_class"));

        SubeventState dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageRoll);
        Assert.False(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);
        Assert.Equal(SubeventState.Phase.Targeting, subevent.phase);
        subevent.SetTargets([rpglObject]);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateAbilityScore);
        Assert.False(result.completed);

        result.subevent.subevent.JoinSubeventData(new JsonObject().LoadFromString($$"""
            {
                "base": {{abilityScore}},
                "bonuses": [ ],
                "minimum": 0
            }
            """));

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageCollection);
        Assert.False(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageRoll);
        Assert.False(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageDelivery);
        Assert.True(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        Assert.Equal(1000L - damage, rpglObject.GetHealthCurrent());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "deals damage (half on pass)")]
    public void DealsDamageHalfOnPass() {
        long difficultyClass = 10L;
        long savingThrowRoll = 20L;
        long abilityScore = 10L;
        long damage = 10L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SubeventState subevent = new(new AbilitySave()
            .SetSource(rpglObject)
            .SetTarget(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "difficulty_class": {{difficultyClass}},
                    "determined": [ {{savingThrowRoll}} ],
                    "ability": "dex",
                    "damage": [
                        {
                            "formula": "number",
                            "number": {{damage}}
                        }
                    ],
                    "damage_on_pass": "half"
                }
                """)));

        // skip inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateDifficultyClass);
        Assert.False(result.completed);

        result.subevent.subevent.JoinSubeventData(new JsonObject().LoadFromString($$"""
            {
                "base": {{difficultyClass}},
                "bonuses": [ ],
                "minimum": 0
            }
            """));

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageCollection);
        Assert.False(result.completed);
        Assert.Equal(difficultyClass, subevent.subevent.json.GetLong("difficulty_class"));

        SubeventState dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageRoll);
        Assert.False(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);
        Assert.Equal(SubeventState.Phase.Targeting, subevent.phase);
        subevent.SetTargets([rpglObject]);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateAbilityScore);
        Assert.False(result.completed);

        result.subevent.subevent.JoinSubeventData(new JsonObject().LoadFromString($$"""
            {
                "base": {{abilityScore}},
                "bonuses": [ ],
                "minimum": 0
            }
            """));

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageCollection);
        Assert.False(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageRoll);
        Assert.False(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageDelivery);
        Assert.True(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        Assert.Equal(1000L - (damage / 2), rpglObject.GetHealthCurrent());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "deals damage (none on pass)")]
    public void DealsDamageNoneOnPass() {
        long difficultyClass = 10L;
        long savingThrowRoll = 20L;
        long abilityScore = 10L;
        long damage = 10L;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext()
            .Add(rpglObject);

        SubeventState subevent = new(new AbilitySave()
            .SetSource(rpglObject)
            .SetTarget(rpglObject)
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "difficulty_class": {{difficultyClass}},
                    "determined": [ {{savingThrowRoll}} ],
                    "ability": "dex",
                    "damage": [
                        {
                            "formula": "number",
                            "number": {{damage}}
                        }
                    ],
                    "damage_on_pass": "none"
                }
                """)));

        // skip inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateDifficultyClass);
        Assert.False(result.completed);

        result.subevent.subevent.JoinSubeventData(new JsonObject().LoadFromString($$"""
            {
                "base": {{difficultyClass}},
                "bonuses": [ ],
                "minimum": 0
            }
            """));

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageCollection);
        Assert.False(result.completed);
        Assert.Equal(difficultyClass, subevent.subevent.json.GetLong("difficulty_class"));

        SubeventState dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageRoll);
        Assert.False(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);
        Assert.Equal(SubeventState.Phase.Targeting, subevent.phase);
        subevent.SetTargets([rpglObject]);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is CalculateAbilityScore);
        Assert.False(result.completed);

        result.subevent.subevent.JoinSubeventData(new JsonObject().LoadFromString($$"""
            {
                "base": {{abilityScore}},
                "bonuses": [ ],
                "minimum": 0
            }
            """));

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageCollection);
        Assert.False(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.True(result.subevent.subevent is DamageRoll);
        Assert.False(result.completed);

        dependency = result.subevent;
        do {
            result = dependency.Advance(context);
        } while (!result.completed);

        result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = false }, result);

        result = subevent.Advance(context);
        Assert.Equal(new() { subevent = null, completed = true }, result);

        Assert.Equal(1000L - 0L, rpglObject.GetHealthCurrent());
    }

}
