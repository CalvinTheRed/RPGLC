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
            long source,
            long target,
            JsonArray bonuses
    ) {
        RPGLEffect rpglEffect = (RPGLEffect) DBManager.QueryRPGLEffectTemplateByDatapackId(datapackId)
            .ApplyBonuses(bonuses)
            .NewInstance()
            .SetSource(source)
            .SetTarget(target)
            .SetUuid(GenerateUuid<RPGLEffectTO>("effects"));

        DBManager.InsertRPGLEffect(rpglEffect);
        return rpglEffect;
    }

    public static RPGLEffect NewEffect(string datapackId, long source, long target) {
        return NewEffect(datapackId, source, target, new());
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
        RPGLItem rpglItem = (RPGLItem) DBManager.QueryRPGLItemTemplateByDatapackId(datapackId)
            .ApplyBonuses(bonuses)
            .NewInstance()
            .SetUuid(GenerateUuid<RPGLItemTO>("items"));

        DBManager.InsertRPGLItem(rpglItem);
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
        RPGLObject rpglObject = (RPGLObject) DBManager.QueryRPGLObjectTemplateByDatapackId(datapackId)
            .ApplyBonuses(bonuses)
            .NewInstance()
            .SetUserId(userId)
            .SetPosition(position)
            .SetRotation(rotation)
            .SetUuid(GenerateUuid<RPGLObjectTO>("objects"));

        DBManager.InsertRPGLObject(rpglObject);
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
        RPGLResource rpglResource = (RPGLResource) DBManager.QueryRPGLResourceTemplateByDatapackId(datapackId)
            .ApplyBonuses(bonuses)
            .NewInstance()
            .SetUuid(GenerateUuid<RPGLResourceTO>("resources"));

        DBManager.InsertRPGLResource(rpglResource);
        return rpglResource;
    }

    public static RPGLResource NewResource(string datapackId) {
        return NewResource(datapackId, new());
    }

};
