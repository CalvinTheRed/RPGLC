using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.database;

public class RPGLItemTemplate : RPGLTemplate {

    public RPGLItemTemplate() : base() {

    }

    public RPGLItemTemplate(JsonObject other) : this() {
        Join(other);
    }

    public RPGLItem NewInstance(long uuid) {
        RPGLItem rpglItem = (RPGLItem) new RPGLItem().SetUuid(uuid);
        Setup(rpglItem);
        ProcessEffects(rpglItem);
        ProcessResources(rpglItem);

        return rpglItem;
    }

    public override RPGLItemTemplate ApplyBonuses(JsonArray bonuses) {
        return new(base.ApplyBonuses(bonuses));
    }

    internal static void ProcessEffects(RPGLItem rpglItem) {
        JsonArray effectList = rpglItem.GetEffects();
        for (int i = 0; i < effectList.Count(); i++) {
            JsonObject effectListItem = effectList.GetJsonObject(i);
            JsonArray effectDatapackIdList = effectListItem.GetJsonArray("effects");
            JsonArray effectUuidList = new();
            for (int j = 0; j < effectDatapackIdList.Count(); j++) {
                effectUuidList.AddInt(RPGLFactory.NewEffect(effectDatapackIdList.GetString(j)).GetUuid());
            }
            effectListItem.PutJsonArray("effects", effectUuidList);
        }
    }

    internal static void ProcessResources(RPGLItem rpglItem) {
        JsonArray resourceList = rpglItem.GetEffects();
        for (int i = 0; i < resourceList.Count(); i++) {
            JsonObject resourceListItem = resourceList.GetJsonObject(i);
            JsonArray resourceDatapackIdList = resourceListItem.GetJsonArray("resources");
            JsonArray resourceUuidList = new();
            for (int j = 0; j < resourceDatapackIdList.Count(); j++) {
                string resourceDatapackId = resourceDatapackIdList.GetString(j);
                RPGLResource rpglResource = RPGLFactory.NewResource(resourceDatapackId);
                resourceUuidList.AddInt(rpglResource.GetUuid());
            }
            resourceListItem.PutJsonArray("resources", resourceUuidList);
        }
    }

};
