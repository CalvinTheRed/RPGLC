using com.rpglc.testutils.function;
using com.rpglc.testutils.subevent;
using System.Reflection;
using Xunit.Sdk;

namespace com.rpglc.testutils.beforeaftertestattributes;

public class DummyCounterManager : BeforeAfterTestAttribute {

    public override void Before(MethodInfo methodUnderTest) {
        base.Before(methodUnderTest);

        DummySubevent.ResetCounter();
        DummyFunction.ResetCounter();
    }

};
