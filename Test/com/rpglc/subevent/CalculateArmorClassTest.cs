using com.rpglc.core;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.subevent;

[AssignDatabase]
[Collection("Serial")]
public class CalculateArmorClassTest {

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prepares")]
    public void Prepares() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", "Player 1");
        CalculateArmorClass calculateArmorClass = new CalculateArmorClass()
            .SetSource(rpglObject)
            .Prepare(new DummyContext(), new());

        Assert.Equal(10L, calculateArmorClass.Get());
    }

};
