﻿using com.rpglc.core;
using com.rpglc.database;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.condition;

public class OriginItemHasTag : Condition {

    public OriginItemHasTag() : base("origin_item_has_tag") { }

    public override bool Run(RPGLEffect rpglEffect, Subevent subevent, JsonObject conditionJson, RPGLContext context, JsonArray originPoint) {
        string originItemAlias = conditionJson.GetString("origin_item");
        RPGLItem? originItem = null;
        if (Equals(originItemAlias, "subevent")) {
            originItem = DBManager.QueryRPGLItem(
                x => x.Uuid == subevent.GetOriginItem()
            );
        } else if (Equals(originItemAlias, "effect")) {
            originItem = DBManager.QueryRPGLItem(
                x => x.Uuid == rpglEffect.GetOriginItem()
            );
        }
        return originItem is not null && originItem.HasTag(conditionJson.GetString("tag"));
    }

};
