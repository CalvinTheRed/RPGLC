using com.rpglc.json;

namespace com.rpglc.database.TO;

public class RPGLResourceTemplateTO : TaggableContentTO {
    public List<object> refreshCriterion { get; set; }
    public string originItem { get; set; }
    public long potency { get; set; }
    public bool exhausted { get; set; }

    public RPGLResourceTemplate ToTemplate() {
        RPGLResourceTemplate template = new();
        template.PutJsonArray("refresh_criterion", new JsonArray(this.refreshCriterion));
        template.PutString("origin_item", this.originItem);
        template.PutInt("potency", this.potency);
        template.PutBool("exhausted", this.exhausted);
        
        base.ApplyToTemplate(template);
        return template;
    }
};
