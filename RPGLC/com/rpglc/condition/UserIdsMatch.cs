using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.condition;

/// <summary>
///   Returns true if the specified objects have a common user id.
///   <code>
///   {
///     "condition": "user_ids_match",
///     "objects": [
///       {
///         "from": "subevent" | "effect",
///         "object": "source" | "target",
///         "as_origin": &lt;bool = false&gt;
///       }
///     ]
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"objects" indicates a list of objects from which to extract and compare user ids.</item>
///   </list>
///   
/// </summary>
public class UserIdsMatch : Condition {

    public UserIdsMatch() : base("user_ids_match") { }

    public override bool Run(RPGLEffect rpglEffect, Subevent subevent, JsonObject conditionJson, RPGLContext context, JsonArray originPoint) {
        JsonArray objects = conditionJson.GetJsonArray("objects");
        return objects
            .AsList()
            .Select(json => RPGLEffect.GetObject(rpglEffect, subevent, new JsonObject(json as Dictionary<string, object>)).GetUserId())
            .Distinct()
            .Count() == 1;
    }

};
