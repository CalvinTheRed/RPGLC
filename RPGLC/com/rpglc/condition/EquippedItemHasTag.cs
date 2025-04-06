using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.condition;

/// <summary>
///   Returns true if the equipped item in the specified slot has the specified tag.
///   
///   <code>
///   {
///     "condition": "equipped_item_has_tag",
///     "object": {
///       "from": "subevent" | "effect",
///       "object": "source" | "target"
///     },
///     "slot": &lt;string = "*"&gt;,
///     "tag": &lt;string&gt;
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"object" indicates which object's equipment is being checked for a tag.</item>
///     <item>"slot" is an optional field that defaults to a value of "*" if left unspecified. This value causes the condition to check all equipped items for the specified tag, rather than an item in only one slot. This field indicates which equipment slot is being checked for an item with the specified tag.</item>
///     <item>"tag" is the tag the equipped item is expected to have.</item>
///   </list>
///   
/// </summary>
public class EquippedItemHasTag : Condition {

    public EquippedItemHasTag() : base("equipped_item_has_tag") { }

    public override bool Run(RPGLEffect rpglEffect, Subevent subevent, JsonObject conditionJson, RPGLContext context, JsonArray originPoint) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, subevent, conditionJson.GetJsonObject("object"));
        string slot = conditionJson.GetString("slot") ?? "*";
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
