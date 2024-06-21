using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.condition;

public class OriginItemsMatch : Condition {

    public OriginItemsMatch() : base("origin_items_match") { }

    public override bool Run(RPGLEffect rpglEffect, Subevent subevent, JsonObject conditionJson, RPGLContext context, JsonArray originPoint) {
        string effectOriginItem = rpglEffect.GetOriginItem();
        string subeventOriginItem = subevent.GetOriginItem();
        return effectOriginItem is not null && Equals(effectOriginItem, subeventOriginItem);
    }

};
