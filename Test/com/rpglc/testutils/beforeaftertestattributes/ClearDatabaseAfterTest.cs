using com.rpglc.database;
using com.rpglc.database.TO;
using System.Reflection;
using Xunit.Sdk;

namespace com.rpglc.testutils.beforeaftertestattributes;

public class ClearDatabaseAfterTest : BeforeAfterTestAttribute
{

    // clear database after each individual test
    public override void After(MethodInfo methodUnderTest)
    {
        base.After(methodUnderTest);

        DBManager.DeleteCollection<RPGLClassTO>("classes");
        DBManager.DeleteCollection<RPGLClassTO>("races");

        DBManager.DeleteCollection<RPGLEffectTemplateTO>("effect_templates");
        DBManager.DeleteCollection<RPGLEventTemplateTO>("event_templates");
        DBManager.DeleteCollection<RPGLItemTemplateTO>("item_templates");
        DBManager.DeleteCollection<RPGLObjectTemplateTO>("object_templates");
        DBManager.DeleteCollection<RPGLResourceTemplateTO>("resource_templates");

        DBManager.DeleteCollection<RPGLEffectTO>("effects");
        DBManager.DeleteCollection<RPGLEffectTO>("items");
        DBManager.DeleteCollection<RPGLEffectTO>("objects");
        DBManager.DeleteCollection<RPGLEffectTO>("resources");
    }

};
