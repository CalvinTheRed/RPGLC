using com.rpglc.function;
using com.rpglc.subevent;
using System.Reflection;
using Xunit.Sdk;

namespace com.rpglc.testutils.beforeaftertestattributes;

public class ResetCountersAfterTest : BeforeAfterTestAttribute {

    public override void Before(MethodInfo methodUnderTest) {
        base.Before(methodUnderTest);

        DummySubevent.ResetCounter();
        DummyFunction.ResetCounter();
    }

};
