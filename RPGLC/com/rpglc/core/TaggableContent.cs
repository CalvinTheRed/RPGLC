using com.rpglc.json;

namespace com.rpglc.core;

public class TaggableContent : PersistentContent {

    public JsonArray GetTags() {
        return base.GetJsonArray("tags");
    }

    public void SetTags(JsonArray tags) {
        base.PutJsonArray("tags", tags);
    }

    public void AddTag(string tag) {
        this.GetTags().AddString(tag);
    }

    public void RemoveTag(string tag) {
        this.GetTags().AsList().Remove(tag);
    }

    public bool HasTag(string tag) {
        return this.GetTags().Contains(tag);
    }

    public bool HasAnyTag(List<object> tags) {
        return this.GetTags().ContainsAny(tags);
    }

};
