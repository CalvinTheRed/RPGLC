using com.rpglc.core;
using System.Reflection;
using Xunit.Sdk;

namespace com.rpglc.testutils.beforeaftertestattributes;

public class ClearRPGLAfterTest : BeforeAfterTestAttribute {

    // clear database after each individual test
    public override void After(MethodInfo methodUnderTest) {
        base.After(methodUnderTest);
        RPGL.ClearRPGL();
    }

};
