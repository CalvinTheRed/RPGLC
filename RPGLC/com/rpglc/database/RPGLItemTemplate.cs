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

    public RPGLItem NewInstance(long uuid) {
        RPGLItem rpglItem = (RPGLItem) new RPGLItem().SetUuid(uuid);
        Setup(rpglItem);
        ProcessEffects(rpglItem);
        ProcessResources(rpglItem);

        return rpglItem;
    }

    internal static void ProcessEffects(RPGLItem rpglItem) {
        JsonArray effectsBySlot = rpglItem.GetEffects();
        for (int i = 0; i < effectsBySlot.Count(); i++) {
            JsonObject effectsForSlot = effectsBySlot.GetJsonObject(i);
            JsonArray effectDatapackIdList = effectsForSlot.GetJsonArray("effects");
            JsonArray effectUuidList = new();
            for (int j = 0; j < effectDatapackIdList.Count(); j++) {
                effectUuidList.AddInt(RPGLFactory.NewEffect(effectDatapackIdList.GetString(j)).GetUuid());
            }
            effectsForSlot.PutJsonArray("effects", effectUuidList);
        }
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
                resourceUuidList.AddInt(rpglResource.GetUuid());
            }
            resourcesForSlot.PutJsonArray("resources", resourceUuidList);
        }
    }

};
