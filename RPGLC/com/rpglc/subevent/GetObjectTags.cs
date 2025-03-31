using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

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

    public override GetObjectTags? Invoke(RPGLContext context, JsonArray originPoint) {
        return (GetObjectTags?) base.Invoke(context, originPoint);
    }

    public override GetObjectTags JoinSubeventData(JsonObject other) {
        return (GetObjectTags) base.JoinSubeventData(other);
    }

    public override GetObjectTags Prepare(RPGLContext context, JsonArray originPoint) {
        json.PutIfAbsent("tags", new JsonArray());
        return this;
    }

    public override GetObjectTags Run(RPGLContext context, JsonArray originPoint) {
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
        json.GetJsonArray("tags").AddString(tag);
        return this;
    }

    public List<string> ObjectTags() {
        List<string> tags = [];
        JsonArray tagsArray = json.GetJsonArray("tags");
        for (int i = 0; i < tagsArray.Count(); i++) {
            tags.Add(tagsArray.GetString(i));
        }
        return tags;
    }
}
