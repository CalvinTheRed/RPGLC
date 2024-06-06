using com.rpglc.json;

namespace com.rpglc.database.TO;

public class RPGLItemTemplateTO : TaggableContentTO {
    public Dictionary<string, object> Effects { get; set; }
    public List<object> Events { get; set; }
    public List<object> Resources { get; set; }
    public long Cost { get; set; }
    public long Weight { get; set; }

    public RPGLItemTemplate ToTemplate() {
        RPGLItemTemplate template = new();
        template.PutJsonObject("effects", new(Effects)); 
        template.PutJsonArray("events", new(Events));
        template.PutJsonArray("resources", new(Resources));
        template.PutInt("cost", Cost);
        template.PutInt("weight", Weight);

        base.ApplyToTemplate(template);
        return template;
    }

};
