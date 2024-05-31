using com.rpglc.database;
using com.rpglc.database.TO;
using com.rpglc.json;
using com.rpglc.math;

namespace com.rpglc.core;

public static class RPGLFactory {

    private static long GenerateUuid<T>(string collectionName) where T : PersistentContentTO {
        // TODO - can this be constrained by enforcing Uuid uniqueness in the database?
        long uuid;
        bool uuidIsAvailable = true;
        do {
            uuid = RPGLRandom.GetRandom().Next();
            if (!DBManager.IsUuidAvailable<T>(collectionName, uuid)) {
                uuidIsAvailable = false;
            }
        } while (!uuidIsAvailable);
        return uuid;
    }

    public static RPGLEffect NewEffect(
            string datapackId,
            long? source,
            long? target,
            JsonArray bonuses
    ) {
        RPGLEffect rpglEffect = DBManager.QueryRPGLEffectTemplateByDatapackId(datapackId)
            .ApplyBonuses(bonuses)
            .NewInstance(GenerateUuid<RPGLEffectTO>("effects"))
            .SetSource(source)
            .SetTarget(target);

        DBManager.InsertRPGLEffect(rpglEffect);
        rpglEffect.SetId(DBManager.QueryRPGLEffect(x => x.Uuid == rpglEffect.GetUuid()).GetId());
        return rpglEffect;
    }

    public static RPGLEffect NewEffect(string datapackId, long? source, long? target) {
        return NewEffect(datapackId, source, target, new());
    }

    public static RPGLEffect NewEffect(string datapackId) {
        return NewEffect(datapackId, null, null);
    }

    public static RPGLEvent NewEvent(string datapackId, JsonArray bonuses) {
        return DBManager.QueryRPGLEventTemplateByDatapackId(datapackId)
            .ApplyBonuses(bonuses)
            .NewInstance();
    }

    public static RPGLEvent NewEvent(string datapackId) {
        return NewEvent(datapackId, new());
    }

    public static RPGLItem NewItem(string datapackId, JsonArray bonuses) {
        RPGLItem rpglItem = DBManager.QueryRPGLItemTemplateByDatapackId(datapackId)
            .ApplyBonuses(bonuses)
            .NewInstance(GenerateUuid<RPGLItemTO>("items"));

        DBManager.InsertRPGLItem(rpglItem);
        rpglItem.SetId(DBManager.QueryRPGLItem(x => x.Uuid == rpglItem.GetUuid()).GetId());
        return rpglItem;
    }

    public static RPGLItem NewItem(string datapackId) {
        return NewItem(datapackId, new());
    }

    public static RPGLObject NewObject(
            string datapackId,
            string userId,
            JsonArray position,
            JsonArray rotation,
            JsonArray bonuses
    ) {
        RPGLObject rpglObject = DBManager.QueryRPGLObjectTemplateByDatapackId(datapackId)
            .ApplyBonuses(bonuses)
            .NewInstance(GenerateUuid<RPGLObjectTO>("objects"))
            .SetUserId(userId)
            .SetPosition(position)
            .SetRotation(rotation);

        DBManager.InsertRPGLObject(rpglObject);
        rpglObject.SetId(DBManager.QueryRPGLObject(x => x.Uuid == rpglObject.GetUuid()).GetId());
        return rpglObject;
    }

    public static RPGLObject NewObject(
            string datapackId,
            string userId,
            JsonArray position,
            JsonArray rotation
    ) {
        return NewObject(datapackId, userId, position, rotation, new());
    }

    public static RPGLObject NewObject(string datapackId, string userId) {
        JsonArray position = new JsonArray()
            .AddDouble(0d)
            .AddDouble(0d)
            .AddDouble(0d);
        JsonArray rotation = new JsonArray()
            .AddDouble(0d)
            .AddDouble(0d)
            .AddDouble(0d);

        return NewObject(datapackId, userId, position, rotation);
    }

    public static RPGLResource NewResource(string datapackId, JsonArray bonuses) {
        RPGLResource rpglResource = DBManager.QueryRPGLResourceTemplateByDatapackId(datapackId)
            .ApplyBonuses(bonuses)
            .NewInstance(GenerateUuid<RPGLResourceTO>("resources"));

        DBManager.InsertRPGLResource(rpglResource);
        rpglResource.SetId(DBManager.QueryRPGLResource(x => x.Uuid == rpglResource.GetUuid()).GetId());
        return rpglResource;
    }

    public static RPGLResource NewResource(string datapackId) {
        return NewResource(datapackId, new());
    }

};
