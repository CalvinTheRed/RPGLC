using com.rpglc.data;
using System.Reflection;
using Xunit.Sdk;

namespace com.rpglc.testutils.beforeaftertestattributes;

public class UsesDatabase : BeforeAfterTestAttribute {

    public override void Before(MethodInfo methodUnderTest) {
        base.Before(methodUnderTest);

        DBManager.SetDatabase(TestUtils.DB_DIR, TestUtils.DB_NAME);
    }

    public override void After(MethodInfo methodUnderTest) {
        base.After(methodUnderTest);

        DBManager.DumpDatabase();
    }

}
