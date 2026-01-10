using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.condition;

/// <summary>
///   Returns true if the specified object has the specified tag.
///   
///   <code>
///   {
///     "condition": "object_has_tag",
///     "object": {
///       "from": "subevent" | "effect",
///       "object": "source" | "target",
///       "as_origin": &lt;bool = false&gt;
///     },
///     "tag": &lt;string&gt;
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"object" indicates which object's tag list is being checked.</item>
///     <item>"tag" is the tag the object is expected to have.</item>
///   </list>
///   
/// </summary>
public class ObjectHasTag : Condition {

    public ObjectHasTag() : base("object_has_tag") {
        conditionSteps.AddRange([
            (rpglEffect, subevent, conditionJson, context) => {
                RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, subevent, conditionJson.GetJsonObject("object"));
                if (this.subeventDependency == null) {
                    subeventDependency = new GetObjectTags()
                        .JoinSubeventData(new JsonObject().LoadFromString("""
                            {
                                "object_tags": [ ]
                            }
                            """))
                        .SetSource(rpglObject)
                        .SetTarget(rpglObject);
                } else {
                    List<string> objectTags = (subeventDependency as GetObjectTags).ObjectTags();
                    this.evaluation = objectTags.Contains(conditionJson.GetString("tag"));
                    this.subeventDependency = null;
                }

                return new() {
                    conditionDependency = null,
                    subeventDependency = this.subeventDependency,
                    stepCompleted = this.subeventDependency is null,
                };
            },
        ]);
    }

    public override ObjectHasTag Clone() {
        return new();
    }

    public override bool Run(RPGLEffect rpglEffect, Subevent subevent, JsonObject conditionJson, RPGLContext context, JsonArray originPoint) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, subevent, conditionJson.GetJsonObject("object"));
        return rpglObject.GetObjectTags(context).Contains(conditionJson.GetString("tag"));
    }

};
