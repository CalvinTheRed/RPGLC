﻿using com.rpglc.json;

namespace com.rpglc.core;

public class DatabaseContent : JsonObject {

    public DatabaseContent() { }

    public DatabaseContent(Dictionary<string, object> data) : base(data) { }

    public JsonObject GetMetadata() {
        return GetJsonObject("metadata");
    }

    public DatabaseContent SetMetadata(JsonObject metadata) {
        PutJsonObject("metadata", metadata);
        return this;
    }

    public string GetDatapackId() {
        return GetString("datapack_id");
    }

    public DatabaseContent SetDatapackId(string datapackId) {
        PutString("datapack_id", datapackId);
        return this;
    }

    public string GetDescription() {
        return GetString("description");
    }

    public DatabaseContent SetDescription(string description) {
        PutString("description", description);
        return this;
    }

    public string GetName() {
        return GetString("name");
    }

    public DatabaseContent SetName(string name) {
        PutString("name", name);
        return this;
    }

};
