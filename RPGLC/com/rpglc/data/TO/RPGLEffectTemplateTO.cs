using com.rpglc.json;

namespace com.rpglc.data.TO;

public class RPGLEffectTemplateTO : TaggableContentTO {
    public Dictionary<string, object> SubeventFilters { get; set; }
    public List<object>? InheritedEffects { get; set; }
    public bool? AllowDuplicates { get; set; }
    public bool? Optional { get; set; }
    public bool? Intrinsic { get; set; }

    public RPGLEffectTemplate ToTemplate() {
        RPGLEffectTemplate template = new();
        template.PutJsonObject("subevent_filters", new JsonObject(SubeventFilters));
        template.PutJsonArray("inherited_effects", new JsonArray(InheritedEffects ?? []));
        template.PutBool("allow_duplicates", AllowDuplicates);
        template.PutBool("optional", Optional);
        template.PutBool("intrinsic", Intrinsic);

        ApplyToTemplate(template);
        return template;
    }

};
