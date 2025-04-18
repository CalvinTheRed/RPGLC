﻿using com.rpglc.json;

namespace com.rpglc.core;

public static class RPGLFactory {

    public static RPGLEffect NewEffect(
            string datapackId,
            JsonArray bonuses,
            string? source = null,
            string? target = null
    ) {
        RPGLEffect rpglEffect = RPGL.GetRPGLEffectTemplate(datapackId)
            .ApplyBonuses(bonuses)
            .NewInstance(Guid.NewGuid().ToString())
            .SetSource(source)
            .SetTarget(target);
        RPGL.AddRPGLEffect(rpglEffect);
        return rpglEffect;
    }

    public static RPGLEffect NewEffect(string datapackId, string? source = null, string? target = null) {
        return NewEffect(datapackId, new(), source, target);
    }

    public static RPGLEvent NewEvent(string datapackId, JsonArray bonuses) {
        return RPGL.GetRPGLEventTemplate(datapackId)
            .ApplyBonuses(bonuses)
            .NewInstance();
    }

    public static RPGLEvent NewEvent(string datapackId) {
        return NewEvent(datapackId, new());
    }

    public static RPGLItem NewItem(string datapackId, JsonArray bonuses) {
        RPGLItem rpglItem = RPGL.GetRPGLItemTemplate(datapackId)
            .ApplyBonuses(bonuses)
            .NewInstance(Guid.NewGuid().ToString());
        RPGL.AddRPGLItem(rpglItem);
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
        RPGLObject rpglObject = RPGL.GetRPGLObjectTemplate(datapackId)
            .ApplyBonuses(bonuses)
            .NewInstance(Guid.NewGuid().ToString())
            .SetUserId(userId)
            .SetPosition(position)
            .SetRotation(rotation);
        RPGL.AddRPGLObject(rpglObject);
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
        RPGLResource rpglResource = RPGL.GetRPGLResourceTemplate(datapackId)
            .ApplyBonuses(bonuses)
            .NewInstance(Guid.NewGuid().ToString());
        RPGL.AddRPGLResource(rpglResource);
        return rpglResource;
    }

    public static RPGLResource NewResource(string datapackId) {
        return NewResource(datapackId, new());
    }

};
