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

    public RPGLObject NewInstance(string uuid) {
        RPGLObject rpglObject = (RPGLObject) new RPGLObject().SetUuid(uuid);
        Setup(rpglObject);
        ProcessEffects(rpglObject);
        ProcessInventory(rpglObject);
        ProcessEquippedItems(rpglObject);
        ProcessResources(rpglObject);
        ProcessClasses(rpglObject);

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
            itemUuidList.AddString(RPGLFactory.NewItem(itemDatapackIdList.GetString(i)).GetUuid());
        }
        rpglObject.SetInventory(itemUuidList);
    }

    internal static void ProcessEquippedItems(RPGLObject rpglObject) {
        JsonObject itemDatapackIdDict = rpglObject.GetEquippedItems();
        JsonObject itemUuidDict = new();
        foreach (string key in itemDatapackIdDict.AsDict().Keys) {
            RPGLItem rpglItem = RPGLFactory.NewItem(itemDatapackIdDict.GetString(key));
            itemUuidDict.PutString(key, rpglItem.GetUuid());
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
                resourceUuidList.AddString(
                    RPGLFactory.NewResource(resourceInstructions.GetString("resource")).GetUuid()
                );
            }
        }
        rpglObject.SetResources(resourceUuidList);
    }

    internal static void ProcessClasses(RPGLObject rpglObject) {
        JsonArray classList = rpglObject.GetClasses();
        rpglObject.SetClasses(new());

        // set classes and nested classes
        for (int i = 0; i < classList.Count(); i++) {
            JsonObject classData = classList.GetJsonObject(i);
            string classDatapackId = classData.GetString("id");
            long level = (long) classData.GetInt("level");
            JsonObject choices = classData.GetJsonObject("choices");

            // re-assign additional nested classes
            JsonObject additionalNestedClassList = classData.GetJsonObject("additional_nested_classes") ?? new();
            foreach (string key in additionalNestedClassList.AsDict().Keys) {
                JsonObject additionalNestedClassData = additionalNestedClassList.GetJsonObject(key);
                rpglObject.AddAdditionalNestedClass(
                    classDatapackId,
                    key,
                    additionalNestedClassData.GetInt("scale") ?? 1L,
                    additionalNestedClassData.GetBool("round_up") ?? false
                );
            }

            // level up
            for (int j = 0; j < level; j++) {
                rpglObject.LevelUp(classDatapackId, choices);
            }
        }
    }

};
