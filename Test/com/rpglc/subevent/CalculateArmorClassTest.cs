using com.rpglc.core;
using com.rpglc.testutils;
using com.rpglc.testutils.mocks;

namespace com.rpglc.subevent;

[AssignDatabase]
[Collection("Serial")]
public class CalculateArmorClassTest {

    [DefaultMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "prepares")]
    public void Prepares() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        CalculateArmorClass calculateArmorClass = new CalculateArmorClass()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal(10L, calculateArmorClass.Get());
    }

};
