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
        JsonObject resources = rpglItem.GetResources();
        JsonObject processedResources = new();
        foreach (string resourceDatapackId in resources.AsDict().Keys) {
            JsonObject resourceData = resources.GetJsonObject(resourceDatapackId);
            long count = resourceData.RemoveInt("count") ?? 1L;
            for (int i = 0; i < count; i++) {
                RPGLResource rpglResource = RPGLFactory.NewResource(resourceDatapackId);
                processedResources.PutJsonArray(rpglResource.GetUuid(), resourceData.GetJsonArray("slots"));
            }
        }
        rpglItem.SetResources(processedResources);
    }

};
