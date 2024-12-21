using com.rpglc.core;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;

namespace com.rpglc.data;

[Collection("Serial")]
public class DBManagerTest {

    [DefaultMock]
    [Fact(DisplayName = "inserts and queries RPGLEffectTemplate")]
    [UsesDatabase]
    public void InsertsAndQueriesRPGLEffectTemplate() {
        RPGLEffectTemplate template = RPGL.GetRPGLEffectTemplate("test:dummy");
        DBManager.InsertRPGLEffectTemplate(template);
        List<RPGLEffectTemplate> templates = DBManager.QueryRPGLEffectTemplates();
        Assert.Single(templates);
        Assert.Equal("test:dummy", templates[0].GetDatapackId());
    }

    [DefaultMock]
    [Fact(DisplayName = "inserts and queries RPGLEventTemplate")]
    [UsesDatabase]
    public void InsertsAndQueriesRPGLEventTemplate() {
        RPGLEventTemplate template = RPGL.GetRPGLEventTemplate("test:dummy");
        DBManager.InsertRPGLEventTemplate(template);
        List<RPGLEventTemplate> templates = DBManager.QueryRPGLEventTemplates();
        Assert.Single(templates);
        Assert.Equal("test:dummy", templates[0].GetDatapackId());
    }

    [DefaultMock]
    [Fact(DisplayName = "inserts and queries RPGLItemTemplate")]
    [UsesDatabase]
    public void InsertsAndQueriesRPGLItemTemplate() {
        RPGLItemTemplate template = RPGL.GetRPGLItemTemplate("test:dummy");
        DBManager.InsertRPGLItemTemplate(template);
        List<RPGLItemTemplate> templates = DBManager.QueryRPGLItemTemplates();
        Assert.Single(templates);
        Assert.Equal("test:dummy", templates[0].GetDatapackId());
    }

    [DefaultMock]
    [Fact(DisplayName = "inserts and queries RPGLObjectTemplate")]
    [UsesDatabase]
    public void InsertsAndQueriesRPGLObjectTemplate() {
        RPGLObjectTemplate template = RPGL.GetRPGLObjectTemplate("test:dummy");
        DBManager.InsertRPGLObjectTemplate(template);
        List<RPGLObjectTemplate> templates = DBManager.QueryRPGLObjectTemplates();
        Assert.Single(templates);
        Assert.Equal("test:dummy", templates[0].GetDatapackId());
    }

    [DefaultMock]
    [Fact(DisplayName = "inserts and queries RPGLResourceTemplate")]
    [UsesDatabase]
    public void InsertsAndQueriesRPGLResourceTemplate() {
        RPGLResourceTemplate template = RPGL.GetRPGLResourceTemplate("test:dummy");
        DBManager.InsertRPGLResourceTemplate(template);
        List<RPGLResourceTemplate> templates = DBManager.QueryRPGLResourceTemplates();
        Assert.Single(templates);
        Assert.Equal("test:dummy", templates[0].GetDatapackId());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "inserts and queries RPGLEffect")]
    [UsesDatabase]
    public void InsertsAndQueriesRPGLEffect() {
        RPGLEffect rpglEffect = RPGLFactory.NewEffect("test:dummy");
        DBManager.InsertRPGLEffect(rpglEffect);
        List<RPGLEffect> rpglEffects = DBManager.QueryRPGLEffects();
        Assert.Single(rpglEffects);
        Assert.Equal("test:dummy", rpglEffects[0].GetDatapackId());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "inserts and queries RPGLItem")]
    [UsesDatabase]
    public void InsertsAndQueriesRPGLItem() {
        RPGLItem rpglItem = RPGLFactory.NewItem("test:dummy");
        DBManager.InsertRPGLItem(rpglItem);
        List<RPGLItem> rpglItems = DBManager.QueryRPGLItems();
        Assert.Single(rpglItems);
        Assert.Equal("test:dummy", rpglItems[0].GetDatapackId());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "inserts and queries RPGLObject")]
    [UsesDatabase]
    public void InsertsAndQueriesRPGLObject() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        DBManager.InsertRPGLObject(rpglObject);
        List<RPGLObject> rpglObjects = DBManager.QueryRPGLObjects();
        Assert.Single(rpglObjects);
        Assert.Equal("test:dummy", rpglObjects[0].GetDatapackId());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "inserts and queries RPGLResource")]
    [UsesDatabase]
    public void InsertsAndQueriesRPGLResource() {
        RPGLResource rpglResource = RPGLFactory.NewResource("test:dummy");
        DBManager.InsertRPGLResource(rpglResource);
        List<RPGLResource> rpglResources = DBManager.QueryRPGLResources();
        Assert.Single(rpglResources);
        Assert.Equal("test:dummy", rpglResources[0].GetDatapackId());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "inserts and queries RPGLClass")]
    [UsesDatabase]
    public void InsertsAndQueriesRPGLClass() {
        RPGLClass rpglClass = RPGLFactory.GetClass("test:dummy");
        DBManager.InsertRPGLClass(rpglClass);
        List<RPGLClass> rpglClasses = DBManager.QueryRPGLClasses();
        Assert.Single(rpglClasses);
        Assert.Equal("test:dummy", rpglClasses[0].GetDatapackId());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "inserts and queries RPGLRace")]
    [UsesDatabase]
    public void InsertsAndQueriesRPGLRace() {
        RPGLRace rpglRace = RPGLFactory.GetRace("test:dummy");
        DBManager.InsertRPGLRace(rpglRace);
        List<RPGLRace> rpglRaces = DBManager.QueryRPGLRaces();
        Assert.Single(rpglRaces);
        Assert.Equal("test:dummy", rpglRaces[0].GetDatapackId());
    }

};
