using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class CalculateProficiencyBonusTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "calculates assigned proficiency bonus")]
    public void CalculatesAssignedProficiencyBonus() {
        long proficiencyBonus = 6L;
        
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID)
            .SetProficiencyBonus(proficiencyBonus)
            .SetClasses(new JsonArray().LoadFromString("""
                [
                    {
                      "additional_nested_classes": { },
                      "id": "test:dummy",
                      "level": 1,
                      "name": "Dummy"
                    }
                ]
                """));
        RPGLContext context = new DummyContext()
            .Add(rpglObject);
        SubeventState subevent = new(new CalculateProficiencyBonus()
            .SetSource(rpglObject)
            .SetTarget(rpglObject));

        // skip over inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Equal(proficiencyBonus, (subevent.subevent as CalculateProficiencyBonus).Get());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "calculates inferred proficiency bonus")]
    public void CalculatesInferredProficiencyBonus() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID)
            .SetClasses(new JsonArray().LoadFromString("""
                [
                    {
                      "additional_nested_classes": { },
                      "id": "test:dummy",
                      "level": 1,
                      "name": "Dummy"
                    }
                ]
                """));
        RPGLContext context = new DummyContext()
            .Add(rpglObject);
        SubeventState subevent = new(new CalculateProficiencyBonus()
            .SetSource(rpglObject)
            .SetTarget(rpglObject));

        // skip over inherited steps
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);
        _ = subevent.Advance(context);

        var result = subevent.Advance(context);
        Assert.Equal((null, true), result);
        Assert.Equal(2L, (subevent.subevent as CalculateProficiencyBonus).Get());
    }

};
