using com.rpglc.json;

namespace com.rpglc.database.TO;

public class RPGLResourceTemplateTO : TaggableContentTO {
    public List<object> RefreshCriterion { get; set; }
    public long? OriginItem { get; set; }
    public long Potency { get; set; }
    public bool Exhausted { get; set; }

    public RPGLResourceTemplate ToTemplate() {
        RPGLResourceTemplate template = new();
        template.PutJsonArray("refresh_criterion", new JsonArray(RefreshCriterion));
        template.PutInt("origin_item", OriginItem);
        template.PutInt("potency", Potency);
        template.PutBool("exhausted", Exhausted);
        
        base.ApplyToTemplate(template);
        return template;
    }

};
