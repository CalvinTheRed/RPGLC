namespace com.rpglc.database.TO;

public class RPGLEffectTemplateTO : TaggableContentTO {
    public Dictionary<string, object> SubeventFilters { get; set; }

    public RPGLEffectTemplate ToTemplate() {
        RPGLEffectTemplate template = new();
        template.PutJsonObject("subevent_filters", new json.JsonObject(SubeventFilters));

        ApplyToTemplate(template);
        return template;
    }

};
