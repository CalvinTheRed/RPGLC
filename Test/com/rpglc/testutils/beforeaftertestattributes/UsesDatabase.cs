using com.rpglc.data;
using System.Reflection;
using Xunit.Sdk;

namespace com.rpglc.testutils.beforeaftertestattributes;

public class UsesDatabase : BeforeAfterTestAttribute {

    public override void Before(MethodInfo methodUnderTest) {
        base.Before(methodUnderTest);

        DBManager.SetDatabase(Path.Combine("C:", "Temp"), "DELETEME.db");
    }

    public override void After(MethodInfo methodUnderTest) {
        base.After(methodUnderTest);

        DBManager.DumpDatabase();
    }

}
