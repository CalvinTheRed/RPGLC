using com.rpglc.subevent;
using System.Reflection;
using Xunit.Sdk;

namespace com.rpglc.testutils;

public class InitializeSubevents : BeforeAfterTestAttribute {

    public override void Before(MethodInfo methodUnderTest) {
        Subevent.Initialize(true);
    }

};
