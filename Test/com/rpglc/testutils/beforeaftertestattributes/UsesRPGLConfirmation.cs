using com.rpglc.core;
using System.Reflection;
using Xunit.Sdk;

namespace com.rpglc.testutils.beforeaftertestattributes;

public class UsesRPGLConfirmation : BeforeAfterTestAttribute {

    public override void Before(MethodInfo methodUnderTest) {
        base.Before(methodUnderTest);

        RPGLConfirmation.SetInstance(new DummyConfirmation());
    }

    public override void After(MethodInfo methodUnderTest) {
        base.After(methodUnderTest);

        DummyConfirmation.ClearResponses();
    }

}
