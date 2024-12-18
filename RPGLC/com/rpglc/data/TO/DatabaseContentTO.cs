using LiteDB;

namespace com.rpglc.data.TO;

public class DatabaseContentTO {
    //public ObjectId _id { get; set; }

    public Dictionary<string, object> Metadata { get; set; }
    public string DatapackId { get; set; }
    public string Description { get; set; }
    public string Name { get; set; }

    virtual public void ApplyToTemplate(RPGLTemplate template) {
        template.PutJsonObject("metadata", new json.JsonObject(Metadata));
        template.PutString("datapack_id", DatapackId);
        template.PutString("description", Description);
        template.PutString("name", Name);
    }

};
