using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Invokes subevents.
///   
///   <code>
///   {
///     "function": "invoke_subevents",
///     "subevents": [
///       &lt;subevent instructions&gt;
///     ],
///     "source": {
///       "from": "subevent" | "effect",
///       "object": "source" | "target",
///       "as_origin": &lt;bool = false&gt;
///     },
///     "targets": [
///       {
///         "from": "subevent" | "effect",
///         "object": "source" | "target",
///         "as_origin": &lt;bool = false&gt;
///       }
///     ]
///   }
///   </code>
/// </summary>
public class InvokeSubevents : Function {

    public InvokeSubevents() : base("invoke_subevents") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        JsonArray nestedSubeventJsonArray = functionJson.GetJsonArray("subevents") ?? new();
        RPGLObject source = RPGLEffect.GetObject(rpglEffect, subevent, functionJson.GetJsonObject("source"));
        List<RPGLObject> targets = functionJson.GetJsonArray("targets").AsList()
            .Select(targetJson => RPGLEffect.GetObject(rpglEffect, subevent, new JsonObject((Dictionary<string, object>) targetJson)))
            .ToList();

        for (int i = 0; i < nestedSubeventJsonArray.Count(); i++) {
            JsonObject nestedSubeventJson = nestedSubeventJsonArray.GetJsonObject(i);
            Subevent nestedSubevent = Subevent.Subevents[nestedSubeventJson.GetString("subevent")]
                .Clone(nestedSubeventJson)
                .SetSource(source)
                .SetOriginItem(rpglEffect.GetOriginItem())
                .Prepare(context, originPoint, rpglEffect);
            foreach (RPGLObject target in targets) {
                nestedSubevent.Clone().SetTarget(target).Invoke(context, originPoint, rpglEffect);
            }
        }
    }

};
