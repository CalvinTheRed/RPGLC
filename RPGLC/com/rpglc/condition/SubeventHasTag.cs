using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.condition;

/// <summary>
///   Returns true if the subevent has the specified tag.
///   
///   <code>
///   {
///     "condition": "subevent_has_tag",
///     "tag": &lt;string&gt;
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"tag" is the tag the subevent is expected to have.</item>
///   </list>
///   
/// </summary>
public class SubeventHasTag : Condition {

    public SubeventHasTag() : base("subevent_has_tag") { }

    public override bool Run(RPGLEffect rpglEffect, Subevent subevent, JsonObject conditionJson, RPGLContext context, JsonArray originPoint) {
        return subevent.HasTag(conditionJson.GetString("tag"));
    }

};
