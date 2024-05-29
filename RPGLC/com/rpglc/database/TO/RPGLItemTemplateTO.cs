using com.rpglc.json;

namespace com.rpglc.database.TO;

public class RPGLItemTemplateTO : TaggableContentTO {
    public List<object> Effects { get; set; }
    public List<object> Events { get; set; }
    public List<object> Resources { get; set; }
    public long Cost { get; set; }
    public long Weight { get; set; }

    public RPGLItemTemplate ToTemplate() {
        RPGLItemTemplate template = new();
        template.PutJsonArray("effects", new JsonArray(Effects)); 
        template.PutJsonArray("events", new JsonArray(Events));
        template.PutJsonArray("resources", new JsonArray(Resources));
        template.PutInt("cost", Cost);
        template.PutInt("weight", Weight);

        base.ApplyToTemplate(template);
        return template;
    }

};
