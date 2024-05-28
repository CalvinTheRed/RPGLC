namespace com.rpglc.database.TO;

public class RPGLEventTemplateTO : DatabaseContentTO {
    public Dictionary<string, object> areaOfEffect { get; set; }
    public List<object> cost { get; set; }
    public List<object> subevents { get; set; }
    public string originItem { get; set; }

    public RPGLEventTemplate ToTemplate() {
        RPGLEventTemplate template = new();
        template.PutJsonObject("area_of_effect", new json.JsonObject(this.areaOfEffect));
        template.PutJsonArray("cost", new json.JsonArray(this.cost));
        template.PutJsonArray("subevents", new json.JsonArray(this.subevents));
        template.PutString("origin_item", this.originItem);
        
        base.ApplyToTemplate(template);
        return template;
    }

};
