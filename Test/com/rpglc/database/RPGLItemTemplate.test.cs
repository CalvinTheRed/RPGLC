﻿using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.testutils.beforeaftertestattributes;
using com.rpglc.testutils.beforeaftertestattributes.mocks;

namespace com.rpglc.database;

[AssignDatabase]
[Collection("Serial")]
public class RPGLItemTemplateTest {

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [Fact(DisplayName = "creates new instance")]
    public void CreatesNewInstance() {
        string itemUuid = "uuid";
        RPGLItem rpglItem = DBManager.QueryRPGLItemTemplateByDatapackId("test:dummy")
            .NewInstance(itemUuid);

        Assert.Equal(
            """{"author":"Calvin Withun"}""",
            rpglItem.GetMetadata().ToString()
        );
        Assert.Equal("Dummy Item", rpglItem.GetName());
        Assert.Equal("This item has no features.", rpglItem.GetDescription());
        Assert.Equal("test:dummy", rpglItem.GetDatapackId());
        Assert.Equal(itemUuid, rpglItem.GetUuid());
        Assert.Equal("""[]""", rpglItem.GetTags().ToString());
        Assert.Equal(1L, rpglItem.GetWeight());
        Assert.Equal(1L, rpglItem.GetCost());
        Assert.Equal("""{}""", rpglItem.GetEffects().ToString());
        Assert.Equal("""{}""", rpglItem.GetEvents().ToString());
        Assert.Equal("""{}""", rpglItem.GetResources().ToString());
    }

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [ExtraItemsMock]
    [Fact(DisplayName = "assigns effects")]
    public void AssignsEffects() {
        string itemUuid = "uuid";
        RPGLItem rpglItem = DBManager.QueryRPGLItemTemplateByDatapackId("test:complex_item")
            .NewInstance(itemUuid);

        JsonObject effects = rpglItem.GetEffects();
        Assert.Equal(1, effects.Count());
        string effectUuid = effects.AsDict().Keys.ElementAt(0);
        RPGLEffect rpglEffect = DBManager.QueryRPGLEffect(x => x.Uuid == effectUuid);
        Assert.NotNull(rpglEffect);
        Assert.Equal($"""
            {'{'}
              "{rpglEffect.GetUuid()}": [
                [
                  "mainhand",
                  "offhand"
                ],
                [
                  "mainhand"
                ],
                [
                  "offhand"
                ]
              ]
            {'}'}
            """, effects.PrettyPrint());
    }

    [ClearDatabaseAfterTest]
    [DefaultMock]
    [ExtraItemsMock]
    [Fact(DisplayName = "assigns resources")]
    public void AssignsResources() {
        string itemUuid = "uuid";
        RPGLItem rpglItem = DBManager.QueryRPGLItemTemplateByDatapackId("test:complex_item")
            .NewInstance(itemUuid);

        JsonObject resources = rpglItem.GetResources();
        Assert.Equal(1, resources.Count());
        string resourceUuid = resources.AsDict().Keys.ElementAt(0);
        RPGLResource rpglResource = DBManager.QueryRPGLResource(x => x.Uuid == resourceUuid);
        Assert.NotNull(rpglResource);
        Assert.Equal($"""
            {'{'}
              "{rpglResource.GetUuid()}": [
                [
                  "mainhand",
                  "offhand"
                ],
                [
                  "mainhand"
                ],
                [
                  "offhand"
                ]
              ]
            {'}'}
            """, resources.PrettyPrint());
    }

};
