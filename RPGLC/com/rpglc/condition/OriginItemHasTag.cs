using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.condition;

/// <summary>
///   Returns true if the specified origin item has the specified tag.
///   
///   <code>
///   {
///     "condition": "origin_item_has_tag",
///     "origin_item": "subevent" | "effect",
///     "tag": &lt;string&gt;
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"origin_item" indicates whether the origin item is taken from the subevent or the effect.</item>
///     <item>"tag" is the tag the origin item is expected to have.</item>
///   </list>
///   
/// </summary>
public class OriginItemHasTag : Condition {

    public OriginItemHasTag() : base("origin_item_has_tag") { }

    public override bool Run(RPGLEffect rpglEffect, Subevent subevent, JsonObject conditionJson, RPGLContext context, JsonArray originPoint) {
        string originItemAlias = conditionJson.GetString("origin_item");
        RPGLItem? originItem = null;
        if (Equals(originItemAlias, "subevent") && subevent.GetOriginItem() is not null) {
            originItem = RPGL.GetRPGLItem(subevent.GetOriginItem());
        } else if (Equals(originItemAlias, "effect") && rpglEffect.GetOriginItem() is not null) {
            originItem = RPGL.GetRPGLItem(rpglEffect.GetOriginItem());
        }
        return originItem is not null && originItem.HasTag(conditionJson.GetString("tag"));
    }

};
