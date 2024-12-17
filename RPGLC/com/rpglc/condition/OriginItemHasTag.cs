using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.condition;

public class OriginItemHasTag : Condition {

    public OriginItemHasTag() : base("origin_item_has_tag") { }

    public override bool Run(RPGLEffect rpglEffect, Subevent subevent, JsonObject conditionJson, RPGLContext context, JsonArray originPoint) {
        string originItemAlias = conditionJson.GetString("origin_item");
        RPGLItem? originItem = null;
        if (Equals(originItemAlias, "subevent") && subevent.GetOriginItem() is not null) {
            originItem = RPGL.GetRPGLItems().Find(x => x.GetUuid() == subevent.GetOriginItem());
        } else if (Equals(originItemAlias, "effect") && rpglEffect.GetOriginItem() is not null) {
            originItem = RPGL.GetRPGLItems().Find(x => x.GetUuid() == rpglEffect.GetOriginItem());
        }
        return originItem is not null && originItem.HasTag(conditionJson.GetString("tag"));
    }

};
