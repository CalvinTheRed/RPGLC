using com.rpglc.core;
using com.rpglc.testutils;
using com.rpglc.testutils.mocks;

namespace com.rpglc.subevent;

[AssignDatabase]
[Collection("Serial")]
public class CalculateProficiencyBonusTest {

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "prepares assigned proficiency bonus")]
    public void PreparesAssignedProficiencyBonus() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        CalculateProficiencyBonus calculateProficiencyBonus = new CalculateProficiencyBonus()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal(2L, calculateProficiencyBonus.Get());
    }

    [DefaultMock]
    [ExtraObjectsMock]
    [ExtraClassesMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "prepares inferred proficiency bonus")]
    public void PreparesInferredProficiencyBonus() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:complex_object", "Player 1");
        CalculateProficiencyBonus calculateProficiencyBonus = new CalculateProficiencyBonus()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal(2L, calculateProficiencyBonus.Get());
    }

};
