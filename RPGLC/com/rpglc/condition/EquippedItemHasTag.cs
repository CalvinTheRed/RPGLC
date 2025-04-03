﻿using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.condition;

public class EquippedItemHasTag : Condition {

    public EquippedItemHasTag() : base("equipped_item_has_tag") { }

    public override bool Run(RPGLEffect rpglEffect, Subevent subevent, JsonObject conditionJson, RPGLContext context, JsonArray originPoint) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, subevent, conditionJson.GetJsonObject("object"));
        string slot = conditionJson.GetString("slot");
        if (slot == "*") {
            JsonObject equippedItems = rpglObject.GetEquippedItems();
            foreach (string slotKey in equippedItems.AsDict().Keys) {
                if (RPGL.GetRPGLItem(equippedItems.GetString(slotKey)).HasTag(conditionJson.GetString("tag"))) {
                    return true;
                }
            }
            return false;
        } else {
            RPGLItem? rpglItem = RPGL.GetRPGLItem(rpglObject.GetEquippedItems().GetString(slot));
            return rpglItem is not null && rpglItem.HasTag(conditionJson.GetString("tag"));
        }
    }

};
