using com.rpglc.condition;
using System.Reflection;
using Xunit.Sdk;

namespace com.rpglc.testutils;

public class InitializeConditions : BeforeAfterTestAttribute {

    public override void Before(MethodInfo methodUnderTest) {
        base.Before(methodUnderTest);
        Condition.Initialize(true);
    }

};
