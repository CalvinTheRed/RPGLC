using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

/// <summary>
///   Collects tags assigned to an object.
///   
///   <br /><br />
///   <i>This subevent is unavailable to be used directly inside an RPGLEvent.</i>
///   
///   <br /><br />
///   <b>Special Functions</b>
///   <list type="bullet">
///     <item>AddObjectTag</item>
///   </list>
///   
/// </summary>
public class GetObjectTags : Subevent {
    
    public GetObjectTags() : base("get_object_tags") { }

    public override Subevent Clone() {
        Subevent clone = new GetObjectTags();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new GetObjectTags();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override GetObjectTags? Invoke(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return (GetObjectTags?) base.Invoke(context, originPoint, invokingEffect);
    }

    public override GetObjectTags JoinSubeventData(JsonObject other) {
        return (GetObjectTags) base.JoinSubeventData(other);
    }

    public override GetObjectTags Prepare(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        json.PutIfAbsent("object_tags", new JsonArray());
        return this;
    }

    public override GetObjectTags Run(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        JsonArray objectTags = json.GetJsonArray("object_tags");
        JsonArray tags = GetTarget().GetTags();
        for (int i = 0; i < tags.Count(); i++) {
            objectTags.AddString(tags.GetString(i));
        }
        return this;
    }

    public override GetObjectTags SetOriginItem(string? originItem) {
        return (GetObjectTags) base.SetOriginItem(originItem);
    }

    public override GetObjectTags SetSource(RPGLObject source) {
        return (GetObjectTags) base.SetSource(source);
    }

    public override GetObjectTags SetTarget(RPGLObject target) {
        return (GetObjectTags) base.SetTarget(target);
    }

    public GetObjectTags AddObjectTag(string tag) {
        json.GetJsonArray("object_tags").AddString(tag);
        return this;
    }

    public List<string> ObjectTags() {
        List<string> tags = [];
        JsonArray tagsArray = json.GetJsonArray("object_tags");
        for (int i = 0; i < tagsArray.Count(); i++) {
            tags.Add(tagsArray.GetString(i));
        }
        return tags;
    }
}
