using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;

namespace com.rpglc.database;

[AssignDatabase]
[Collection("Serial")]
public class RPGLObjectTemplateTest {

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "creates new instance")]
    public void CreatesNewInstance() {
        string objectUuid = "uuid";
        RPGLObject rpglObject = DBManager.QueryRPGLObjectTemplateByDatapackId("test:dummy")
            .NewInstance(objectUuid);

        Assert.Equal(
            """{"author":"Calvin Withun"}""",
            rpglObject.GetMetadata().ToString()
        );
        Assert.Equal("Dummy Object", rpglObject.GetName());
        Assert.Equal("This object has no features.", rpglObject.GetDescription());
        Assert.Equal("test:dummy", rpglObject.GetDatapackId());
        Assert.Equal(objectUuid, rpglObject.GetUuid());
        Assert.Equal("""[]""", rpglObject.GetTags().ToString());
        Assert.Equal(
            """{"cha":10,"con":10,"dex":10,"int":10,"str":10,"wis":10}""",
            rpglObject.GetAbilityScores().ToString()
        );
        Assert.Equal("""{}""", rpglObject.GetEquippedItems().ToString());
        Assert.Equal("""[]""", rpglObject.GetClasses().ToString());
        Assert.Equal("""[]""", rpglObject.GetEvents().ToString());
        Assert.Equal("""[]""", rpglObject.GetInventory().ToString());
        Assert.Equal("""[]""", rpglObject.GetPosition().ToString());
        Assert.Equal("""[]""", rpglObject.GetRaces().ToString());
        Assert.Equal("""[]""", rpglObject.GetResources().ToString());
        Assert.Equal("""[]""", rpglObject.GetRotation().ToString());
        Assert.Null(rpglObject.GetOriginObject());
        Assert.Null(rpglObject.GetProxy());
        Assert.Null(rpglObject.GetUserId());
        Assert.Equal(1000L, rpglObject.GetHealthBase());
        Assert.Equal(1000L, rpglObject.GetHealthCurrent());
        Assert.Equal(0L, rpglObject.GetHealthTemporary());
        Assert.Equal(2L, rpglObject.GetProficiencyBonus());
    }

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [ExtraClassesMock]
    [ExtraObjectsMock]
    [Fact(DisplayName = "assigns effects")]
    public void AssignsEffects() {
        string objectUuid = "uuid";
        RPGLObject rpglObject = DBManager.QueryRPGLObjectTemplateByDatapackId(
            "test:complex_object"
        ).NewInstance(objectUuid);

        List<RPGLEffect> effects = DBManager.QueryRPGLEffects(x => x.Target == objectUuid);
        Assert.Equal(1, effects.Count());
        Assert.Equal("test:dummy", effects[0].GetDatapackId());
    }

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [ExtraClassesMock]
    [ExtraObjectsMock]
    [Fact(DisplayName = "populates inventory and equips items")]
    public void PopulatesInventoryAndEquipsItems() {
        string objectUuid = "uuid";
        RPGLObject rpglObject = DBManager.QueryRPGLObjectTemplateByDatapackId(
            "test:complex_object"
        ).NewInstance(objectUuid);

        JsonArray inventory = rpglObject.GetInventory();
        Assert.Equal(3, inventory.Count());
        Assert.Equal("test:dummy", DBManager.QueryRPGLItem(x => x.Uuid == inventory.GetString(0)).GetDatapackId());
        Assert.Equal("test:dummy", DBManager.QueryRPGLItem(x => x.Uuid == inventory.GetString(1)).GetDatapackId());
        Assert.Equal("test:dummy", DBManager.QueryRPGLItem(x => x.Uuid == inventory.GetString(2)).GetDatapackId());

        JsonObject equippedItems = rpglObject.GetEquippedItems();
        Assert.Equal(2, equippedItems.Count());

        Assert.True(inventory.Contains(equippedItems.GetString("mainhand")));
        Assert.True(inventory.Contains(equippedItems.GetString("offhand")));
    }

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [ExtraClassesMock]
    [ExtraObjectsMock]
    [Fact(DisplayName = "assigns resources")]
    public void AssignsResources() {
        string objectUuid = "uuid";
        RPGLObjectTemplate template = DBManager.QueryRPGLObjectTemplateByDatapackId(
            "test:complex_object"
        );
        RPGLObject rpglObject = template.NewInstance(objectUuid);

        JsonArray resources = rpglObject.GetResources();
        Assert.Equal(1, resources.Count());
        Assert.Equal("test:dummy", DBManager.QueryRPGLResource(x => x.Uuid == resources.GetString(0)).GetDatapackId());
    }

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [ExtraClassesMock]
    [ExtraObjectsMock]
    [Fact(DisplayName = "assigns nested classes")]
    public void AssignsNestedClasses() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:complex_object", "Player 1");

        Assert.Equal(
            """
            [
              {
                "additional_nested_classes": {
                  "test:additional_nested_class": {
                    "round_up": false,
                    "scale": 2
                  }
                },
                "id": "test:class_with_nested_class",
                "level": 1,
                "name": "Class With Nested Class"
              },
              {
                "additional_nested_classes": { },
                "id": "test:nested_class",
                "level": 1,
                "name": "Nested Class"
              }
            ]
            """,
            rpglObject.GetClasses().PrettyPrint()
        );
    }

};
