using LiteDB;

namespace com.rpglc.database.TO;

public class DatabaseContentTO {
    public ObjectId _id { get; set; }

    public Dictionary<string, object> metadata { get; set; }
    public string datapackId { get; set; }
    public string description { get; set; }
    public string name { get; set; }

    virtual public void ApplyToTemplate(RPGLTemplate template) {
        template.PutJsonObject("metadata", new json.JsonObject(this.metadata));
        template.PutString("datapack_id", this.datapackId);
        template.PutString("description", this.description);
        template.PutString("name", this.name);
    }

};
