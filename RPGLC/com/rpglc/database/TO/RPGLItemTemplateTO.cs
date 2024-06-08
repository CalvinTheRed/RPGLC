namespace com.rpglc.database.TO;

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
        template.PutInt("cost", Cost);
        template.PutInt("weight", Weight);

        base.ApplyToTemplate(template);
        return template;
    }

};
