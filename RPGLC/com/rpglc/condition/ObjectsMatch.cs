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
///     "subevent": "source" | "target",
///     "effect": "source" | "target"
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"subevent" indicates which object drawn from the subevent to reference in the comparison.</item>
///     <item>"effect" indicates which object drawn from the effect to reference in the comparison.</item>
///   </list>
///   
/// </summary>
public class ObjectsMatch : Condition {

    public ObjectsMatch() : base("objects_match") { }

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

        return Equals(effectObject.GetUuid(), subeventObject.GetUuid());
    }

};
