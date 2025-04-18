using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

/// <summary>
///   Spawns a new object.
///   
///   <code>
///   {
///     "subevent": "spawn_object",
///     "object": &lt;string&gt;
///     "controlled_by": {
///       "from": "subevent" | "effect",
///       "object": "source" | "target",
///       "as_origin": &lt;bool = false&gt;
///     },
///     "object_bonuses": [
///       {
///         "field": &lt;string&gt;,
///         "bonus": &lt;long&gt;
///       }
///     ],
///     "extra_effects": [
///       &lt;string&gt;
///     ],
///     "extra_events": [
///       &lt;string&gt;
///     ],
///     "extra_tags": [
///       &lt;string&gt;
///     ],
///     "use_origin_proficiency": &lt;bool = false&gt;,
///     "proxy": &lt;bool = false&gt;
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"object" is an RPGLObject datapack id.</item>
///     <item>"controlled_by" indicates which object's user id will be inherited by the spawned object.</item>
///     <item>"object_bonuses" is an optional field and it will default to a value of { } if left unspecified. This field stores any bonuses that apply to the object once it spawns.</item>
///     <item>"extra_effects" is an optional field and it will default to a value of [ ] if left unspecified. This field stores any extra effects that will affect the object once it spawns.</item>
///     <item>"extra_events" is an optional field and it will default to a value of [ ] if left unspecified. This field stores any extra events made available to the object once it spawns.</item>
///     <item>"extra_tags" is an optional field and it will default to a value of [ ] if left unspecified. This field stores any extra tags that will be applied to the object once it spawns.</item>
///     <item>"use_origin_proficiency" is an optional field and it will default to a value of false if left unspecified. The utility of this field is under scrutiny.</item>
///     <item>"proxy" is an optional field and it will default to a value of false if left unspecified. This field indicates of the spawned object is a proxy for another object.</item>
///   </list>
///   
///   <b>Special Functions</b>
///   <list type="bullet">
///     <item>AddSpawnObjectBonus</item>
///     <item>AddSpawnObjectEffect</item>
///     <item>AddSpawnObjectEvent</item>
///     <item>AddSpawnObjectTag</item>
///   </list>
///   
/// </summary>
public class SpawnObject : Subevent {
    
    public SpawnObject() : base("spawn_object") { }

    public override Subevent Clone() {
        Subevent clone = new SpawnObject();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new SpawnObject();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override SpawnObject? Invoke(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return (SpawnObject?) base.Invoke(context, originPoint, invokingEffect);
    }

    public override SpawnObject JoinSubeventData(JsonObject other) {
        return (SpawnObject) base.JoinSubeventData(other);
    }

    public override SpawnObject Prepare(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        json.PutIfAbsent("controlled_by", new JsonObject());
        json.PutIfAbsent("object_bonuses", new JsonArray());
        json.PutIfAbsent("extra_effects", new JsonArray());
        json.PutIfAbsent("extra_events", new JsonArray());
        json.PutIfAbsent("extra_tags", new JsonArray());
        json.PutIfAbsent("use_origin_proficiency", false);
        json.PutIfAbsent("proxy", false);
        return this;
    }

    public override SpawnObject Run(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        RPGLObject rpglObject = RPGLFactory.NewObject(
            json.GetString("object"),
            RPGLEffect.GetObject(
                invokingEffect,
                this,
                json.GetJsonObject("controlled_by")
            )?.GetUserId() ?? "",
            originPoint.DeepClone(),
            GetSource().GetRotation().DeepClone(),
            json.GetJsonArray("object_bonuses")
        )
            .SetOriginObject(GetSource().GetUuid())
            .SetProxy(json.GetBool("proxy"));

        JsonArray extraEffects = json.GetJsonArray("extra_effects");
        for (int i = 0; i < extraEffects.Count(); i++) {
            rpglObject.AddEffect(RPGLFactory.NewEffect(extraEffects.GetString(i))
                .SetOriginItem(GetOriginItem())
                .SetSource(GetSource().GetUuid()));
            // TODO what happens to the Effect object if it fails to be assigned to an object? When does it get cleaned up?
        }

        rpglObject.GetEvents().AsList().AddRange(json.GetJsonArray("extra_events").AsList());

        JsonArray extraTags = json.GetJsonArray("extra_tags");
        for (int i = 0; i < extraTags.Count(); i++) {
            rpglObject.AddTag(extraTags.GetString(i));
        }

        // TODO is the use_origin_proficiency field actually needed?

        context.Add(rpglObject);
        return this;
    }

    public override SpawnObject SetOriginItem(string? originItem) {
        return (SpawnObject) base.SetOriginItem(originItem);
    }

    public override SpawnObject SetSource(RPGLObject source) {
        return (SpawnObject) base.SetSource(source);
    }

    public override SpawnObject SetTarget(RPGLObject target) {
        return (SpawnObject) base.SetTarget(target);
    }
}
