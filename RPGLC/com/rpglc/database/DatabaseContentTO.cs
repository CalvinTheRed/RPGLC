using LiteDB;

namespace com.rpglc.database;

public class DatabaseContentTO {
    public ObjectId _id { get; set; }

    public Dictionary<string, object> metadata { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public string datapackId { get; set; }

    virtual public void ApplyToTemplate(RPGLTemplate template) {
        template.PutJsonObject("metadata", new json.JsonObject(this.metadata));
        template.PutString("name", this.name);
        template.PutString("description", this.description);
        template.PutString("datapack_id", this.datapackId);
    }
};
