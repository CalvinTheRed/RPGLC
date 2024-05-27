namespace com.rpglc.database.TO;

public class RPGLEffectTemplateTO : DatabaseContentTO {
    public Dictionary<string, object> subeventFilters { get; set; }

    public RPGLEffectTemplate ToTemplate() {
        RPGLEffectTemplate template = new();
        template.PutJsonObject("subevent_filters", new json.JsonObject(this.subeventFilters));

        base.ApplyToTemplate(template);
        return template;
    }
};
