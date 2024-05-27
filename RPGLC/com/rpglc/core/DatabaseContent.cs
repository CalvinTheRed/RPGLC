using com.rpglc.json;

namespace com.rpglc.core;

public class DatabaseContent : JsonObject {

    public JsonObject GetMetadata() {
        return base.GetJsonObject("metadata");
    }

    public void SetMetadata(JsonObject metadata) {
        base.PutJsonObject("metadata", metadata);
    }

    public string GetName(string name) {
        return base.GetString("name");
    }

    public void SetName(string name) {
        base.PutString("name", name);
    }

    public string GetDescription() {
        return base.GetString("description");
    }

    public void SetDescription(string description) {
        base.PutString("description", description);
    }

    public string GetDatapackId() {
        return base.GetString("datapack_id");
    }

    public void SetDatapackId(string datapackId) {
        base.PutString("datapack_id", datapackId);
    }

};
