namespace com.rpglc.database.TO;

public class RPGLEventTemplateTO : DatabaseContentTO {
    public Dictionary<string, object> AreaOfEffect { get; set; }
    public List<object> Cost { get; set; }
    public List<object> Subevents { get; set; }
    public string? OriginItem { get; set; }

    public RPGLEventTemplate ToTemplate() {
        RPGLEventTemplate template = new();
        template.PutJsonObject("area_of_effect", new json.JsonObject(AreaOfEffect));
        template.PutJsonArray("cost", new json.JsonArray(Cost));
        template.PutJsonArray("subevents", new json.JsonArray(Subevents));
        template.PutString("origin_item", OriginItem);
        
        base.ApplyToTemplate(template);
        return template;
    }

};
