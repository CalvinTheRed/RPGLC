using com.rpglc.json;

namespace com.rpglc.database.TO;

public class RPGLResourceTemplateTO : TaggableContentTO {
    public List<object> RefreshCriterion { get; set; }
    public string? OriginItem { get; set; }
    public long? MaximumUses { get; set; }
    public long Potency { get; set; }
    

    public RPGLResourceTemplate ToTemplate() {
        RPGLResourceTemplate template = new();
        template.PutJsonArray("refresh_criterion", new JsonArray(RefreshCriterion));
        template.PutString("origin_item", OriginItem);
        template.PutInt("maximum_uses", MaximumUses);
        template.PutInt("potency", Potency);
        
        ApplyToTemplate(template);
        return template;
    }

};
