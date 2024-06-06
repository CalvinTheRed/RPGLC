using com.rpglc.database;
using System.Reflection;
using Xunit.Sdk;

namespace com.rpglc.testutils;

public class AssignDatabase : BeforeAfterTestAttribute {

    public override void Before(MethodInfo methodUnderTest) {
        base.Before(methodUnderTest);

        DBManager.SetDatabase(Path.Combine("C:", "Temp"), "DELETEME.db");
    }

}
