using com.rpglc.condition;
using com.rpglc.function;
using com.rpglc.subevent;
using System.Reflection;
using Xunit.Sdk;

namespace com.rpglc.testutils.beforeaftertestattributes;

public class RPGLCInit : BeforeAfterTestAttribute {

    public override void Before(MethodInfo methodUnderTest) {
        base.Before(methodUnderTest);
        Condition.Initialize(true);
        Function.Initialize(true);
        Subevent.Initialize(true);
    }

};
