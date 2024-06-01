using com.rpglc.database;
using System.Reflection;
using Xunit.Sdk;

namespace com.rpglc.testutils;

public class SetupDatabase : BeforeAfterTestAttribute {

    // ensure database is available before running tests
    public override void Before(MethodInfo methodUnderTest) {
        base.Before(methodUnderTest);

        if (DBManager.IsDatabaseSet()) {
            DBManager.SetDatabase(Path.Join("C:", "Temp"), "DELETEME.db");
        }
    }

};
