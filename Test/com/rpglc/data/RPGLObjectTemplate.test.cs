using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;

namespace com.rpglc.data;

[Collection("Serial")]
public class RPGLObjectTemplateTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "creates new instance")]
    public void CreatesNewInstance() {
        string objectUuid = "uuid";
        RPGLObject rpglObject = RPGL.GetRPGLObjectTemplate("test:dummy")
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
        Assert.Equal(
            """{"count":0,"rider_effects":[]}""",
            rpglObject.GetHealthTemporary().ToString()
        );
        Assert.Equal("""[]""", rpglObject.GetClasses().ToString());
        Assert.Equal("""[]""", rpglObject.GetEvents().ToString());
        Assert.Equal("""[]""", rpglObject.GetInventory().ToString());
        // position assigned by RPGLFactory
        Assert.Equal("""[]""", rpglObject.GetRaces().ToString());
        Assert.Equal("""[]""", rpglObject.GetResources().ToString());
        // rotation assigned by RPGLFactory
        Assert.Null(rpglObject.GetOriginObject());
        Assert.Null(rpglObject.GetProxy());
        Assert.Null(rpglObject.GetUserId());
        Assert.Equal(1000L, rpglObject.GetHealthBase());
        Assert.Equal(1000L, rpglObject.GetHealthCurrent());
        Assert.Equal(2L, rpglObject.GetProficiencyBonus());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [ExtraClassesMock]
    [ExtraEffectsMock]
    [ExtraObjectsMock]
    [Fact(DisplayName = "assigns effects and rider effects")]
    public void AssignsEffectsAndRiderEffects() {
        string objectUuid = "uuid";
        RPGLObject rpglObject = RPGL.GetRPGLObjectTemplate("test:complex_object")
            .NewInstance(objectUuid);

        List<RPGLEffect> effects = rpglObject.GetEffectObjects();
        Assert.Equal(2, effects.Count());
        Assert.NotNull(effects.Find(x => x.GetDatapackId() == "test:dummy"));
        Assert.NotNull(effects.Find(x => x.GetDatapackId() == "test:complex_effect"));
        Assert.Equal("test:dummy", effects[0].GetDatapackId());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [ExtraClassesMock]
    [ExtraEffectsMock]
    [ExtraObjectsMock]
    [Fact(DisplayName = "populates inventory and equips items")]
    public void PopulatesInventoryAndEquipsItems() {
        string objectUuid = "uuid";
        RPGLObject rpglObject = RPGL.GetRPGLObjectTemplate("test:complex_object")
            .NewInstance(objectUuid);

        JsonArray inventory = rpglObject.GetInventory();
        Assert.Equal(3, inventory.Count());
        Assert.Equal("test:dummy", RPGL.GetRPGLItem(inventory.GetString(0)).GetDatapackId());
        Assert.Equal("test:dummy", RPGL.GetRPGLItem(inventory.GetString(1)).GetDatapackId());
        Assert.Equal("test:dummy", RPGL.GetRPGLItem(inventory.GetString(2)).GetDatapackId());

        JsonObject equippedItems = rpglObject.GetEquippedItems();
        Assert.Equal(2, equippedItems.Count());

        Assert.True(inventory.Contains(equippedItems.GetString("mainhand")));
        Assert.True(inventory.Contains(equippedItems.GetString("offhand")));
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [ExtraClassesMock]
    [ExtraEffectsMock]
    [ExtraObjectsMock]
    [Fact(DisplayName = "assigns resources")]
    public void AssignsResources() {
        string objectUuid = "uuid";
        RPGLObject rpglObject = RPGL.GetRPGLObjectTemplates()
            .Find(x => x.GetDatapackId() == "test:complex_object")
            .NewInstance(objectUuid);

        JsonArray resources = rpglObject.GetResources();
        Assert.Equal(1, resources.Count());
        Assert.Equal("test:dummy", RPGL.GetRPGLResources().Find(x => x.GetUuid() == resources.GetString(0)).GetDatapackId());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [ExtraClassesMock]
    [ExtraEffectsMock]
    [ExtraObjectsMock]
    [Fact(DisplayName = "assigns nested classes")]
    public void AssignsNestedClasses() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:complex_object", TestUtils.USER_ID);

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
