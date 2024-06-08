using com.rpglc.testutils;
using com.rpglc.testutils.mocks;

namespace com.rpglc.core;

[AssignDatabase]
[Collection("Serial")]
public class RPGLItemTest {

    [DefaultMock]
    [ExtraItemsMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "gives effects for slots")]
    public void GivesEffectsForSlots() {
        List<RPGLEffect> effects;
        RPGLItem rpglItem = RPGLFactory.NewItem("test:complex_item");

        effects = rpglItem.GetEffectsForSlots(["mainhand"]);
        Assert.Equal(1, effects.Count);
        Assert.Equal("test:dummy", effects[0].GetDatapackId());

        effects = rpglItem.GetEffectsForSlots(["offhand"]);
        Assert.Equal(1, effects.Count);
        Assert.Equal("test:dummy", effects[0].GetDatapackId());

        effects = rpglItem.GetEffectsForSlots(["mainhand", "offhand"]);
        Assert.Equal(1, effects.Count);
        Assert.Equal("test:dummy", effects[0].GetDatapackId());

        effects = rpglItem.GetEffectsForSlots(["offhand", "mainhand"]);
        Assert.Equal(1, effects.Count);
        Assert.Equal("test:dummy", effects[0].GetDatapackId());

        effects = rpglItem.GetEffectsForSlots(["not-a-slot"]);
        Assert.Equal(0, effects.Count);

        effects = rpglItem.GetEffectsForSlots(["mainhand", "not-a-slot"]);
        Assert.Equal(0, effects.Count);
    }

    [DefaultMock]
    [ExtraItemsMock]
    [ClearDatabaseAfterTest]
    [Fact(DisplayName = "gives resources for slots")]
    public void GivesResourcesForSlots() {
        List<RPGLResource> resources;
        RPGLItem rpglItem = RPGLFactory.NewItem("test:complex_item");

        resources = rpglItem.GetResourcesForSlots(["mainhand"]);
        Assert.Equal(1, resources.Count);
        Assert.Equal("test:dummy", resources[0].GetDatapackId());

        resources = rpglItem.GetResourcesForSlots(["offhand"]);
        Assert.Equal(1, resources.Count);
        Assert.Equal("test:dummy", resources[0].GetDatapackId());

        resources = rpglItem.GetResourcesForSlots(["mainhand", "offhand"]);
        Assert.Equal(1, resources.Count);
        Assert.Equal("test:dummy", resources[0].GetDatapackId());

        resources = rpglItem.GetResourcesForSlots(["offhand", "mainhand"]);
        Assert.Equal(1, resources.Count);
        Assert.Equal("test:dummy", resources[0].GetDatapackId());

        resources = rpglItem.GetResourcesForSlots(["not-a-slot"]);
        Assert.Equal(0, resources.Count);

        resources = rpglItem.GetResourcesForSlots(["mainhand", "not-a-slot"]);
        Assert.Equal(0, resources.Count);
    }

};
