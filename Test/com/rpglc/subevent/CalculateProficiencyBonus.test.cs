﻿using com.rpglc.core;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[Collection("Serial")]
public class CalculateProficiencyBonusTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares assigned proficiency bonus")]
    public void PreparesAssignedProficiencyBonus() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        CalculateProficiencyBonus calculateProficiencyBonus = new CalculateProficiencyBonus()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal(2L, calculateProficiencyBonus.Get());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [ExtraClassesMock]
    [ExtraEffectsMock]
    [ExtraObjectsMock]
    [Fact(DisplayName = "prepares inferred proficiency bonus")]
    public void PreparesInferredProficiencyBonus() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:complex_object", TestUtils.USER_ID);
        CalculateProficiencyBonus calculateProficiencyBonus = new CalculateProficiencyBonus()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal(2L, calculateProficiencyBonus.Get());
    }

};
