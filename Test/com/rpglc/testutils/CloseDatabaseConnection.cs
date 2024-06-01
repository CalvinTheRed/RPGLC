using com.rpglc.database;
using System.Reflection;
using Xunit.Sdk;

namespace com.rpglc.testutils;

public class CloseDatabaseConnection : BeforeAfterTestAttribute {

    // ensure database connection is closed after running tests
    public override void After(MethodInfo methodUnderTest) {
        base.After(methodUnderTest);

        DBManager.DisconnectFromDatabase();
    }

};
