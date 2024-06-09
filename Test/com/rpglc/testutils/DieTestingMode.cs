using com.rpglc.math;
using System.Reflection;
using Xunit.Sdk;

namespace com.rpglc.testutils;

public class DieTestingMode : BeforeAfterTestAttribute {

    public override void Before(MethodInfo methodUnderTest) {
        Die.SetTesting(true);
    }

};
