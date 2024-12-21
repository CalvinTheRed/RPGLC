using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;

namespace com.rpglc.core;

[Collection("Serial")]
public class RPGLItemTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [ExtraItemsMock]
    [Fact(DisplayName = "gives effects for slots")]
    public void GivesEffectsForSlots() {
        List<RPGLEffect> effects;
        RPGLItem rpglItem = RPGLFactory.NewItem("test:complex_item");

        effects = rpglItem.GetEffectsForSlots(["mainhand"]);
        Assert.Single(effects);
        Assert.Equal("test:dummy", effects[0].GetDatapackId());

        effects = rpglItem.GetEffectsForSlots(["offhand"]);
        Assert.Single(effects);
        Assert.Equal("test:dummy", effects[0].GetDatapackId());

        effects = rpglItem.GetEffectsForSlots(["mainhand", "offhand"]);
        Assert.Single(effects);
        Assert.Equal("test:dummy", effects[0].GetDatapackId());

        effects = rpglItem.GetEffectsForSlots(["offhand", "mainhand"]);
        Assert.Single(effects);
        Assert.Equal("test:dummy", effects[0].GetDatapackId());

        effects = rpglItem.GetEffectsForSlots(["not-a-slot"]);
        Assert.Empty(effects);

        effects = rpglItem.GetEffectsForSlots(["mainhand", "not-a-slot"]);
        Assert.Empty(effects);
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [ExtraItemsMock]
    [Fact(DisplayName = "gives events for slots")]
    public void GivesEventsForSlots() {
        List<RPGLEvent> events;
        RPGLItem rpglItem = RPGLFactory.NewItem("test:complex_item");

        events = rpglItem.GetEventsForSlots(["mainhand"]);
        Assert.Single(events);
        Assert.Equal("test:dummy", events[0].GetDatapackId());

        events = rpglItem.GetEventsForSlots(["offhand"]);
        Assert.Single(events);
        Assert.Equal("test:dummy", events[0].GetDatapackId());

        events = rpglItem.GetEventsForSlots(["mainhand", "offhand"]);
        Assert.Single(events);
        Assert.Equal("test:dummy", events[0].GetDatapackId());

        events = rpglItem.GetEventsForSlots(["offhand", "mainhand"]);
        Assert.Single(events);
        Assert.Equal("test:dummy", events[0].GetDatapackId());

        events = rpglItem.GetEventsForSlots(["not-a-slot"]);
        Assert.Empty(events);

        events = rpglItem.GetEventsForSlots(["mainhand", "not-a-slot"]);
        Assert.Empty(events);
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [ExtraItemsMock]
    [Fact(DisplayName = "gives resources for slots")]
    public void GivesResourcesForSlots() {
        List<RPGLResource> resources;
        RPGLItem rpglItem = RPGLFactory.NewItem("test:complex_item");

        resources = rpglItem.GetResourcesForSlots(["mainhand"]);
        Assert.Single(resources);
        Assert.Equal("test:dummy", resources[0].GetDatapackId());

        resources = rpglItem.GetResourcesForSlots(["offhand"]);
        Assert.Single(resources);
        Assert.Equal("test:dummy", resources[0].GetDatapackId());

        resources = rpglItem.GetResourcesForSlots(["mainhand", "offhand"]);
        Assert.Single(resources);
        Assert.Equal("test:dummy", resources[0].GetDatapackId());

        resources = rpglItem.GetResourcesForSlots(["offhand", "mainhand"]);
        Assert.Single(resources);
        Assert.Equal("test:dummy", resources[0].GetDatapackId());

        resources = rpglItem.GetResourcesForSlots(["not-a-slot"]);
        Assert.Empty(resources);

        resources = rpglItem.GetResourcesForSlots(["mainhand", "not-a-slot"]);
        Assert.Empty(resources);
    }

};
