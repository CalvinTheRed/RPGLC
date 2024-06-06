using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.database;

public class RPGLItemTemplate : RPGLTemplate {

    public RPGLItemTemplate() : base() {

    }

    public RPGLItemTemplate(JsonObject other) : this() {
        Join(other);
    }

    public override RPGLItemTemplate ApplyBonuses(JsonArray bonuses) {
        return new(base.ApplyBonuses(bonuses));
    }

    public RPGLItem NewInstance(string uuid) {
        RPGLItem rpglItem = (RPGLItem) new RPGLItem().SetUuid(uuid);
        Setup(rpglItem);
        ProcessEffects(rpglItem);
        ProcessResources(rpglItem);

        return rpglItem;
    }

    private static void ProcessEffects(RPGLItem rpglItem) {
        JsonObject effects = rpglItem.GetEffects();
        JsonObject processedEffects = new();
        foreach (string effectDatapackId in effects.AsDict().Keys) {
            RPGLEffect rpglEffect = RPGLFactory.NewEffect(effectDatapackId);
            processedEffects.PutJsonArray(rpglEffect.GetUuid(), effects.GetJsonArray(effectDatapackId));
        }
        rpglItem.SetEffects(processedEffects);
    }

    internal static void ProcessResources(RPGLItem rpglItem) {
        JsonArray resourcesBySlot = rpglItem.GetResources();
        for (int i = 0; i < resourcesBySlot.Count(); i++) {
            JsonObject resourcesForSlot = resourcesBySlot.GetJsonObject(i);
            JsonArray resourceDatapackIdList = resourcesForSlot.GetJsonArray("resources");
            JsonArray resourceUuidList = new();
            for (int j = 0; j < resourceDatapackIdList.Count(); j++) {
                string resourceDatapackId = resourceDatapackIdList.GetString(j);
                RPGLResource rpglResource = RPGLFactory.NewResource(resourceDatapackId);
                resourceUuidList.AddString(rpglResource.GetUuid());
            }
            resourcesForSlot.PutJsonArray("resources", resourceUuidList);
        }
    }

};
