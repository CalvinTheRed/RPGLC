using com.rpglc.json;
using com.rpglc.subevent;
using com.rpglc.testutils;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;
using com.rpglc.testutils.core;
using com.rpglc.testutils.subevent;

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
        Assert.Single(rpglObject.GetEquippedItems().AsDict().Keys);
        Assert.Equal(
            rpglItem.GetUuid(),
            rpglObject.GetEquippedItems().GetString("mainhand")
        );

        // unequip item
        rpglObject.UnequipItem("mainhand");
        Assert.Empty(rpglObject.GetEquippedItems().AsDict().Keys);

        // take item
        rpglObject.TakeItem(rpglItem.GetUuid());
        Assert.Equal(0, rpglObject.GetInventory().Count());

        // give, equip, and take item
        rpglObject.GiveItem(rpglItem.GetUuid());
        rpglObject.EquipItem(rpglItem.GetUuid(), "mainhand");
        rpglObject.TakeItem(rpglItem.GetUuid());
        Assert.Equal(0, rpglObject.GetInventory().Count());
        Assert.Empty(rpglObject.GetEquippedItems().AsDict().Keys);
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
        Assert.Empty(resources);

        RPGLResource rpglResource = RPGLFactory.NewResource("test:dummy");
        rpglObject.GiveResource(rpglResource);
        resources = rpglObject.GetResourceObjects();
        Assert.Single(resources);
        Assert.Equal("test:dummy", resources[0].GetDatapackId());

        rpglObject.TakeResource(rpglResource.GetUuid());
        rpglResource = RPGL.GetRPGLResource(rpglResource.GetUuid());
        Assert.Null(rpglResource);
        resources = rpglObject.GetResourceObjects();
        Assert.Empty(resources);

        RPGLItem rpglItem = RPGLFactory.NewItem("test:complex_item");
        rpglObject.GiveItem(rpglItem.GetUuid());
        rpglObject.EquipItem(rpglItem.GetUuid(), "mainhand");
        resources = rpglObject.GetResourceObjects();
        Assert.Single(resources);
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
        Assert.Empty(effects);

        RPGLItem rpglItem = RPGLFactory.NewItem("test:complex_item");
        rpglObject.GiveItem(rpglItem.GetUuid());
        rpglObject.EquipItem(rpglItem.GetUuid(), "mainhand");
        rpglObject.EquipItem(rpglItem.GetUuid(), "offhand");
        effects = rpglObject.GetEffectObjects();
        Assert.Single(effects);
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
            [rpglResource],
            [rpglObject],
            context
        );

        Assert.Equal(1, DummySubevent.Counter);

        Assert.Equal(9, rpglResource.GetAvailableUses());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "gains temporary hit points and rider effects")]
    public void GainsTemporaryHitPointsAndRiderEffects() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext();
        context.Add(rpglObject);

        Assert.Equal(0, rpglObject.GetHealthTemporary().GetJsonArray("rider_effects").Count());

        GiveTemporaryHitPoints giveTemporaryHitPoints = new GiveTemporaryHitPoints()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "temporary_hit_points": [
                        {
                            "formula": "range",
                            "dice": [ ],
                            "bonus": 10
                        }
                    ],
                    "rider_effects": [
                        "test:dummy"
                    ]
                }
                """))
            .SetOriginItem(null)
            .SetSource(rpglObject)
            .Prepare(context, new())
            .SetTarget(rpglObject)
            .Invoke(context, new());

        Assert.Equal(10L, rpglObject.GetTemporaryHitPoints());

        Assert.Equal(1, rpglObject.GetHealthTemporary().GetJsonArray("rider_effects").Count());
        RPGLEffect riderEffect = RPGL.GetRPGLEffect(rpglObject.GetHealthTemporary().GetJsonArray("rider_effects").GetString(0));
        Assert.Equal("test:dummy", riderEffect.GetDatapackId());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [ExtraClassesMock]
    [ExtraEffectsMock]
    [ExtraObjectsMock]
    [Fact(DisplayName = "loses temporary hit point rider effects")]
    public void LosesTemporaryHitPointRiderEffects() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:complex_object", TestUtils.USER_ID);
        RPGLContext context = new DummyContext();
        context.Add(rpglObject);

        Assert.Equal(1, rpglObject.GetHealthTemporary().GetJsonArray("rider_effects").Count());
        RPGLEffect riderEffect = RPGL.GetRPGLEffect(rpglObject.GetHealthTemporary().GetJsonArray("rider_effects").GetString(0));
        Assert.Equal("test:complex_effect", riderEffect.GetDatapackId());

        DamageDelivery damageDelivery = new DamageDelivery()
            .JoinSubeventData(new JsonObject().LoadFromString("""
                {
                    "damage": [
                        {
                            "damage_type": "fire",
                            "dice": [],
                            "bonus": 10,
                            "scale": {
                                "numerator": 1,
                                "denominator": 1,
                                "round_up": false
                            }
                        }
                    ],
                    "tags": []
                }
                """))
            .SetOriginItem(null)
            .SetSource(rpglObject)
            .Prepare(context, new())
            .SetTarget(rpglObject)
            .Invoke(context, new());

        Assert.Null(RPGL.GetRPGLEffects().Find(x => x.GetDatapackId() == "test:complex_effect"));
        Assert.DoesNotContain(riderEffect, rpglObject.GetEffectObjects());
        Assert.Equal(0, rpglObject.GetHealthTemporary().GetJsonArray("rider_effects").Count());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "adds new effect")]
    public void AddsNewEffect() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext();
        context.Add(rpglObject);

        RPGLEffect rpglEffect = RPGLFactory.NewEffect("test:dummy");
        rpglObject.AddEffect(rpglEffect);

        Assert.Contains(rpglEffect, rpglObject.GetEffectObjects());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "allows effect duplicates")]
    public void AllowsEffectDuplicates() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext();
        context.Add(rpglObject);

        RPGLEffect rpglEffect1 = RPGLFactory.NewEffect("test:dummy").SetAllowDuplicates(true);
        RPGLEffect rpglEffect2 = RPGLFactory.NewEffect("test:dummy").SetAllowDuplicates(true);
        rpglObject.AddEffect(rpglEffect1);
        rpglObject.AddEffect(rpglEffect2);

        Assert.Contains(rpglEffect1, rpglObject.GetEffectObjects());
        Assert.Contains(rpglEffect2, rpglObject.GetEffectObjects());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "prevents effect duplicates")]
    public void PreventsEffectDuplicates() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLContext context = new DummyContext();
        context.Add(rpglObject);

        RPGLEffect rpglEffect1 = RPGLFactory.NewEffect("test:dummy").SetAllowDuplicates(false);
        RPGLEffect rpglEffect2 = RPGLFactory.NewEffect("test:dummy").SetAllowDuplicates(false);
        rpglObject.AddEffect(rpglEffect1);
        rpglObject.AddEffect(rpglEffect2);

        Assert.Contains(rpglEffect1, rpglObject.GetEffectObjects());
        Assert.DoesNotContain(rpglEffect2, rpglObject.GetEffectObjects());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "gets events from template data")]
    public void GetsEventsFromTemplateData() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID)
            .AddEvent("test:dummy");

        List<RPGLEvent> events = rpglObject.GetEventObjects(new DummyContext().Add(rpglObject));
        Assert.Equal(1, events.Count());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [ExtraItemsMock]
    [Fact(DisplayName = "gets events from equipped items")]
    public void GetsEventsFromEquippedItems() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLItem rpglItem = RPGLFactory.NewItem("test:complex_item");
        rpglObject.GiveItem(rpglItem.GetUuid());
        rpglObject.EquipItem(rpglItem.GetUuid(), "mainhand");

        List<RPGLEvent> events = rpglObject.GetEventObjects(new DummyContext().Add(rpglObject));
        Assert.Equal(1, events.Count());
    }

    [ClearRPGLAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "gets events from effects")]
    public void GetsEventsFromEffects() {
        RPGLObject rpglObject = RPGLFactory.NewObject("test:dummy", TestUtils.USER_ID);
        RPGLEffect rpglEffect = RPGLFactory.NewEffect("test:dummy", rpglObject.GetUuid(), rpglObject.GetUuid());
        rpglEffect.Join(new JsonObject().LoadFromString("""
            {
                "subevent_filters": {
                    "get_events": [
                        {
                            "conditions": [ ],
                            "functions": [
                                {
                                    "function": "add_event",
                                    "event": "test:dummy"
                                }
                            ]
                        }
                    ]
                }
            }
            """));

        List<RPGLEvent> events = rpglObject.GetEventObjects(new DummyContext().Add(rpglObject));
        Assert.Equal(1, events.Count());
    }

};
