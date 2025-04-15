using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.condition;

/// <summary>
///   Returns true if the specified objects are the same object.
///   
///   <code>
///   {
///     "condition": "objects_match",
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
///     <item>"objects" indicates a list of objects to compare to each other.</item>
///   </list>
///   
/// </summary>
public class ObjectsMatch : Condition {

    public ObjectsMatch() : base("objects_match") { }

    public override bool Run(RPGLEffect rpglEffect, Subevent subevent, JsonObject conditionJson, RPGLContext context, JsonArray originPoint) {
        JsonArray objects = conditionJson.GetJsonArray("objects");
        return objects
            .AsList()
            .Select(json => RPGLEffect.GetObject(rpglEffect, subevent, new JsonObject(json as Dictionary<string, object>)).GetUuid())
            .Distinct()
            .Count() == 1;
    }

};
