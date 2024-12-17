using com.rpglc.core;
using System.Reflection;
using Xunit.Sdk;

namespace com.rpglc.testutils.beforeaftertestattributes;

public class RPGLInitTesting : BeforeAfterTestAttribute {

    public override void Before(MethodInfo methodUnderTest) {
        base.Before(methodUnderTest);
        RPGL.InitTesting();
    }

};
