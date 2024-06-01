using com.rpglc.database;
using System.Reflection;
using Xunit.Sdk;

namespace com.rpglc.testutils;

public class OpenDatabaseConnection : BeforeAfterTestAttribute {

    // ensure database connection is open before running tests
    public override void Before(MethodInfo methodUnderTest) {
        base.Before(methodUnderTest);

        if (DBManager.IsDatabaseConnected()) {
            DBManager.ConnectToDatabase(Path.Join("C:", "Temp"), "DELETEME.db");
        }
    }

};
