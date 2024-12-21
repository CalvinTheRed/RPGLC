using com.rpglc.json;

namespace com.rpglc.data.TO;

public class RPGLEffectTemplateTO : TaggableContentTO {
    public Dictionary<string, object> SubeventFilters { get; set; }
    public bool? AllowDuplicates { get; set; }

    public RPGLEffectTemplate ToTemplate() {
        RPGLEffectTemplate template = new();
        template.PutJsonObject("subevent_filters", new JsonObject(SubeventFilters));
        template.PutBool("allow_duplicates", AllowDuplicates);

        ApplyToTemplate(template);
        return template;
    }

};
