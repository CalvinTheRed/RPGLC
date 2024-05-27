using com.rpglc.json;

namespace com.rpglc.database.TO;

public class RPGLItemTemplateTO : TaggableContentTO {
    public Int64 weight { get; set; }
    public Int64 cost { get; set; }
    public List<object> effects { get; set; }
    public List<object> events { get; set; }
    public List<object> resources { get; set; }

    public RPGLItemTemplate ToTemplate() {
        RPGLItemTemplate template = new();
        template.PutInt("weight", this.weight);
        template.PutInt("cost", this.cost);
        template.PutJsonArray("effects", new JsonArray(this.effects)); 
        template.PutJsonArray("events", new JsonArray(this.events));
        template.PutJsonArray("resources", new JsonArray(this.resources));

        base.ApplyToTemplate(template);
        return template;
    }
};
