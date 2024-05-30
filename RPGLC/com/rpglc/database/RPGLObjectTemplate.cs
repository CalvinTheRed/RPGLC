using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.database;

public class RPGLObjectTemplate : RPGLTemplate {

    public RPGLObjectTemplate() : base() {

    }

    public RPGLObjectTemplate(JsonObject other) : this() {
        Join(other);
    }

    public override RPGLObjectTemplate ApplyBonuses(JsonArray bonuses) {
        return new(base.ApplyBonuses(bonuses));
    }

    public RPGLObject NewInstance(long uuid) {
        RPGLObject rpglObject = (RPGLObject) new RPGLObject().SetUuid(uuid);
        Setup(rpglObject);
        ProcessEffects(rpglObject);
        ProcessInventory(rpglObject);
        ProcessEquippedItems(rpglObject);
        ProcessResources(rpglObject);
        // TODO process classes into data stored in the database

        return rpglObject;
    }

    internal static void ProcessEffects(RPGLObject rpglObject) {
        JsonArray effectList = rpglObject.RemoveJsonArray("effects");
        for (int i = 0; i < effectList.Count(); i++) {
            RPGLFactory.NewEffect(
                effectList.GetString(i),
                rpglObject.GetUuid(),
                rpglObject.GetUuid()
            );
        }
    }

    internal static void ProcessInventory(RPGLObject rpglObject) {
        JsonArray itemDatapackIdList = rpglObject.GetInventory();
        JsonArray itemUuidList = new();
        for (int i = 0; i < itemDatapackIdList.Count(); i++) {
            itemUuidList.AddInt(RPGLFactory.NewItem(itemDatapackIdList.GetString(i)).GetUuid());
        }
        rpglObject.SetInventory(itemUuidList);
    }

    internal static void ProcessEquippedItems(RPGLObject rpglObject) {
        JsonObject itemDatapackIdDict = rpglObject.GetEquippedItems();
        JsonObject itemUuidDict = new();
        foreach (string key in itemDatapackIdDict.AsDict().Keys) {
            RPGLItem rpglItem = RPGLFactory.NewItem(itemDatapackIdDict.GetString(key));
            itemUuidDict.PutInt(key, rpglItem.GetUuid());
            rpglObject.GiveItem(rpglItem.GetUuid());
        }
        rpglObject.SetEquippedItems(itemUuidDict);
    }

    internal static void ProcessResources(RPGLObject rpglObject) {
        JsonArray resourceList = rpglObject.GetResources();
        JsonArray resourceUuidList = new();
        for (int i = 0; i < resourceList.Count(); i++) {
            JsonObject resourceInstructions = resourceList.GetJsonObject(i);
            long count = resourceInstructions.GetInt("count") ?? 1L;
            for (int j = 0; j < count; j++) {
                resourceUuidList.AddInt(
                    RPGLFactory.NewResource(resourceInstructions.GetString("resource")).GetUuid()
                );
            }
        }
        rpglObject.SetResources(resourceUuidList);
    }

};
