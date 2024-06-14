using com.rpglc.json;

namespace com.rpglc.database.TO;

public class RPGLObjectTemplateTO : TaggableContentTO {
    public Dictionary<string, object> AbilityScores { get; set; }
    public Dictionary<string, object> EquippedItems { get; set; }
    public List<object> Classes { get; set; }
    public List<object> Effects { get; set; }
    public List<object> Events { get; set; }
    public List<object> Inventory { get; set; }
    public List<object> Position { get; set; }
    public List<object> Races { get; set; }
    public List<object> Resources { get; set; }
    public List<object> Rotation { get; set; }
    public string? OriginObject { get; set; }
    public string UserId { get; set; }
    public long HealthBase { get; set; }
    public long HealthCurrent { get; set; }
    public long HealthTemporary { get; set; }
    public long ProficiencyBonus { get; set; }
    public bool? Proxy { get; set; }

    public RPGLObjectTemplate ToTemplate() {
        RPGLObjectTemplate template = new();
        template.PutJsonObject("ability_scores", new JsonObject(AbilityScores));
        template.PutJsonObject("equipped_items", new JsonObject(EquippedItems));
        template.PutJsonArray("classes", new JsonArray(Classes));
        template.PutJsonArray("effects", new JsonArray(Effects));
        template.PutJsonArray("events", new JsonArray(Events));
        template.PutJsonArray("inventory", new JsonArray(Inventory));
        template.PutJsonArray("position", new JsonArray(Position));
        template.PutJsonArray("races", new JsonArray(Races));
        template.PutJsonArray("resources", new JsonArray(Resources));
        template.PutJsonArray("rotation", new JsonArray(Rotation));
        template.PutString("origin_object", OriginObject);
        template.PutString("user_id", UserId);
        template.PutInt("health_base", HealthBase);
        template.PutInt("health_current", HealthCurrent);
        template.PutInt("health_temporary", HealthTemporary);
        template.PutInt("proficiency_bonus", ProficiencyBonus);
        template.PutBool("proxy", Proxy);

        ApplyToTemplate(template);
        return template;
    }

};
