using com.rpglc.database;
using com.rpglc.json;

namespace com.rpglc.core;

public static class RPGLFactory {

    public static RPGLClass? GetClass(string classDatapackId) {
        return DBManager.QueryRPGLClassByDatapackId(classDatapackId);
    }

    public static RPGLRace? GetRace(string raceDatapackId) {
        return DBManager.QueryRPGLRaceByDatapackId(raceDatapackId);
    }

    public static RPGLEffect NewEffect(
            string datapackId,
            JsonArray bonuses,
            string? source = null,
            string? target = null
    ) {
        RPGLEffect rpglEffect = DBManager.QueryRPGLEffectTemplateByDatapackId(datapackId)
            .ApplyBonuses(bonuses)
            .NewInstance(Guid.NewGuid().ToString())
            .SetSource(source)
            .SetTarget(target);

        DBManager.InsertRPGLEffect(rpglEffect);
        rpglEffect.SetId(DBManager.QueryRPGLEffect(x => x.Uuid == rpglEffect.GetUuid()).GetId());
        return rpglEffect;
    }

    public static RPGLEffect NewEffect(string datapackId, string? source = null, string? target = null) {
        return NewEffect(datapackId, new(), source, target);
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
            .NewInstance(Guid.NewGuid().ToString());

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
            .NewInstance(Guid.NewGuid().ToString())
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
            .NewInstance(Guid.NewGuid().ToString());

        DBManager.InsertRPGLResource(rpglResource);
        rpglResource.SetId(DBManager.QueryRPGLResource(x => x.Uuid == rpglResource.GetUuid()).GetId());
        return rpglResource;
    }

    public static RPGLResource NewResource(string datapackId) {
        return NewResource(datapackId, new());
    }

};
