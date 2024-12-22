using com.rpglc.condition;
using com.rpglc.core;
using com.rpglc.function;
using com.rpglc.subevent;
using com.rpglc.testutils.condition;
using com.rpglc.testutils.function;
using com.rpglc.testutils.subevent;
using System.Reflection;
using Xunit.Sdk;

namespace com.rpglc.testutils.beforeaftertestattributes;

public class RPGLInitTesting : BeforeAfterTestAttribute {

    public override void Before(MethodInfo methodUnderTest) {
        base.Before(methodUnderTest);
        RPGL.Init();

        Condition.Conditions.Add("true", new True());
        Condition.Conditions.Add("false", new False());

        Function.Functions.Add("dummy_function", new DummyFunction());

        Subevent.Subevents.Add("dummy_subevent", new DummySubevent());
    }

};
