namespace com.rpglc.data.TO;

public class RPGLItemTemplateTO : TaggableContentTO {
    public Dictionary<string, object> Effects { get; set; }
    public Dictionary<string, object> Events { get; set; }
    public Dictionary<string, object> Resources { get; set; }
    public long Cost { get; set; }
    public long Weight { get; set; }

    public RPGLItemTemplate ToTemplate() {
        RPGLItemTemplate template = new();
        template.PutJsonObject("effects", new(Effects));
        template.PutJsonObject("events", new(Events));
        template.PutJsonObject("resources", new(Resources));
        template.PutLong("cost", Cost);
        template.PutLong("weight", Weight);

        ApplyToTemplate(template);
        return template;
    }

};
