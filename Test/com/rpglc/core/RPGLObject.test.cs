﻿using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;

namespace com.rpglc.core;

[Collection("Serial")]
[RPGLInitTesting]
public class RPGLObjectTest {

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "manipulates item in inventory")]
    public void ManipulatesItemInInventory() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLItem rpglItem = RPGLFactory.NewItem("test:dummy");

        // give item
        rpglObject.GiveItem(rpglItem.GetUuid());
        Assert.Equal(1, rpglObject.GetInventory().Count());
        Assert.Equal(rpglItem.GetUuid(), rpglObject.GetInventory().GetString(0));

        // give item again
        rpglObject.GiveItem(rpglItem.GetUuid());
        Assert.Equal(1, rpglObject.GetInventory().Count());
        Assert.Equal(rpglItem.GetUuid(), rpglObject.GetInventory().GetString(0));

        // equip item
        rpglObject.EquipItem(rpglItem.GetUuid(), "mainhand");
        Assert.Equal(1, rpglObject.GetEquippedItems().AsDict().Keys.Count());
        Assert.Equal(
            rpglItem.GetUuid(),
            rpglObject.GetEquippedItems().GetString("mainhand")
        );

        // unequip item
        rpglObject.UnequipItem("mainhand");
        Assert.Equal(0, rpglObject.GetEquippedItems().AsDict().Keys.Count());

        // take item
        rpglObject.TakeItem(rpglItem.GetUuid());
        Assert.Equal(0, rpglObject.GetInventory().Count());

        // give, equip, and take item
        rpglObject.GiveItem(rpglItem.GetUuid());
        rpglObject.EquipItem(rpglItem.GetUuid(), "mainhand");
        rpglObject.TakeItem(rpglItem.GetUuid());
        Assert.Equal(0, rpglObject.GetInventory().Count());
        Assert.Equal(0, rpglObject.GetEquippedItems().AsDict().Keys.Count());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [ExtraClassesMock]
    [ExtraRacesMock]
    [Fact(DisplayName = "levels up")]
    public void LevelsUp() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        rpglObject.GetRaces().AddString("test:race_with_resource_per_level");
        
        rpglObject.LevelUp("test:class_with_nested_class", new());
        Assert.Equal(1, rpglObject.GetLevel());
        Assert.Equal(1, rpglObject.GetLevel("test:class_with_nested_class"));
        Assert.Equal(1, rpglObject.GetLevel("test:nested_class"));

        rpglObject.LevelUp("test:class_with_nested_class", new(), new JsonObject().LoadFromString("""
            {
                "test:additional_nested_class": {
                    "scale": 2,
                    "round_up": false
                }
            }
            """));
        Assert.Equal(2, rpglObject.GetLevel());
        Assert.Equal(2, rpglObject.GetLevel("test:class_with_nested_class"));
        Assert.Equal(2, rpglObject.GetLevel("test:nested_class"));
        Assert.Equal(1, rpglObject.GetLevel("test:additional_nested_class"));

        Assert.Equal(2, rpglObject.GetResources().Count());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [ExtraItemsMock]
    [Fact(DisplayName = "manipulates resources")]
    public void ManipulatesResources() {
        List<RPGLResource> resources;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        resources = rpglObject.GetResourceObjects();
        Assert.Equal(0, resources.Count());

        RPGLResource rpglResource = RPGLFactory.NewResource("test:dummy");
        rpglObject.GiveResource(rpglResource);
        resources = rpglObject.GetResourceObjects();
        Assert.Equal(1, resources.Count());
        Assert.Equal("test:dummy", resources[0].GetDatapackId());

        rpglObject.TakeResource(rpglResource.GetUuid());
        rpglResource = RPGL.GetRPGLResources().Find(x => x.GetUuid() == rpglResource.GetUuid());
        Assert.Null(rpglResource);
        resources = rpglObject.GetResourceObjects();
        Assert.Equal(0, resources.Count());

        RPGLItem rpglItem = RPGLFactory.NewItem("test:complex_item");
        rpglObject.GiveItem(rpglItem.GetUuid());
        rpglObject.EquipItem(rpglItem.GetUuid(), "mainhand");
        resources = rpglObject.GetResourceObjects();
        Assert.Equal(1, resources.Count());
        rpglResource = resources[0];
        Assert.Equal("test:dummy", rpglResource.GetDatapackId());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [ExtraItemsMock]
    [Fact(DisplayName = "lists effects")]
    public void ListsEffects() {
        List<RPGLEffect> effects;
        RPGLEffect rpglEffect;

        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        effects = rpglObject.GetEffectObjects();
        Assert.Equal(0, effects.Count());

        RPGLItem rpglItem = RPGLFactory.NewItem("test:complex_item");
        rpglObject.GiveItem(rpglItem.GetUuid());
        rpglObject.EquipItem(rpglItem.GetUuid(), "mainhand");
        rpglObject.EquipItem(rpglItem.GetUuid(), "offhand");
        effects = rpglObject.GetEffectObjects();
        Assert.Equal(1, effects.Count());
        rpglEffect = effects[0];
        Assert.Equal("test:dummy", rpglEffect.GetDatapackId());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [DummyCounterManager]
    [ExtraEventsMock]
    [ExtraResourcesMock]
    [Fact(DisplayName = "invokes event")]
    public void InvokesEvent() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext();
        context.Add(rpglObject);

        RPGLResource rpglResource = RPGLFactory.NewResource("test:complex_resource");
        rpglObject.GiveResource(rpglResource);

        rpglObject.InvokeEvent(
            RPGLFactory.NewEvent("test:complex_event"),
            new(),
            [ rpglResource ],
            [ rpglObject ],
            context
        );

        Assert.Equal(1, DummySubevent.Counter);

        Assert.Equal(9, rpglResource.GetAvailableUses());
    }

};
