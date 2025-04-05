using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.condition;

/// <summary>
///   Returns true if the specified objects have a common user id.
///   <code>
///   {
///     "condition": "user_ids_match",
///     "subevent": "source" | "target",
///     "effect": "source" | "target"
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"subevent" indicates which object's user id to use in the comparison, drawn from the subevent.</item>
///     <item>"effect" indicates which object's user id to use in the comparison, drawn from the effect.</item>
///   </list>
///   
/// </summary>
public class UserIdsMatch : Condition {

    public UserIdsMatch() : base("user_ids_match") { }

    public override bool Run(RPGLEffect rpglEffect, Subevent subevent, JsonObject conditionJson, RPGLContext context, JsonArray originPoint) {
        RPGLObject effectObject = RPGLEffect.GetObject(rpglEffect, subevent, new JsonObject().LoadFromString($$"""
            {
                "from": "effect",
                "object": "{{conditionJson.GetString("effect")}}"
            }
            """));
        RPGLObject subeventObject = RPGLEffect.GetObject(rpglEffect, subevent, new JsonObject().LoadFromString($$"""
            {
                "from": "subevent",
                "object": "{{conditionJson.GetString("subevent")}}"
            }
            """));
        return Equals(effectObject.GetUserId(), subeventObject.GetUserId());
    }

};
