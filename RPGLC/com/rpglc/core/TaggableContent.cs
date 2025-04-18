using com.rpglc.json;

namespace com.rpglc.core;

public class TaggableContent : PersistentContent {

    public JsonArray GetTags() {
        return GetJsonArray("tags");
    }

    public TaggableContent SetTags(JsonArray tags) {
        PutJsonArray("tags", tags);
        return this;
    }

    public TaggableContent AddTag(string tag) {
        JsonArray tags = GetTags();
        if (!tags.Contains(tag)) {
            tags.AddString(tag);
        }
        return this;
    }

    public TaggableContent RemoveTag(string tag) {
        GetTags().AsList().Remove(tag);
        return this;
    }

    public bool HasTag(string tag) {
        return GetTags().Contains(tag);
    }

    public bool HasAnyTag(List<object> tags) {
        return GetTags().ContainsAny(tags);
    }

};
