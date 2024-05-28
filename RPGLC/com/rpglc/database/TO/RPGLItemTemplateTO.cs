using com.rpglc.json;

namespace com.rpglc.database.TO;

public class RPGLItemTemplateTO : TaggableContentTO {
    public List<object> effects { get; set; }
    public List<object> events { get; set; }
    public List<object> resources { get; set; }
    public long cost { get; set; }
    public long weight { get; set; }

    public RPGLItemTemplate ToTemplate() {
        RPGLItemTemplate template = new();
        template.PutJsonArray("effects", new JsonArray(this.effects)); 
        template.PutJsonArray("events", new JsonArray(this.events));
        template.PutJsonArray("resources", new JsonArray(this.resources));
        template.PutInt("cost", this.cost);
        template.PutInt("weight", this.weight);

        base.ApplyToTemplate(template);
        return template;
    }
};
