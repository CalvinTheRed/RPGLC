using com.rpglc.function;
using System.Reflection;
using Xunit.Sdk;

namespace com.rpglc.testutils;

public class InitializeFunctions : BeforeAfterTestAttribute {

    public override void Before(MethodInfo methodUnderTest) {
        base.Before(methodUnderTest);
        Function.Initialize(true);
    }

};
