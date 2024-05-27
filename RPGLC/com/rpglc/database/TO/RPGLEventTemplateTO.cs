namespace com.rpglc.database.TO;

public class RPGLEventTemplateTO : DatabaseContentTO {
    public Dictionary<string, object> areaOfEffect { get; set; }
    public List<object> subevents { get; set; }
    public List<object> cost { get; set; }
    public string originItem { get; set; }

    public RPGLEventTemplate ToTemplate() {
        RPGLEventTemplate template = new();
        template.PutJsonObject("area_of_effect", new json.JsonObject(this.areaOfEffect));
        template.PutJsonArray("subevents", new json.JsonArray(this.subevents));
        template.PutJsonArray("cost", new json.JsonArray(this.cost));
        template.PutString("origin_item", this.originItem);
        
        base.ApplyToTemplate(template);
        return template;
    }
};
