using com.rpglc.json;

namespace com.rpglc.database.TO;

public class RPGLObjectTemplateTO : TaggableContentTO {
    public Dictionary<string, object> abilityScores { get; set; }
    public Dictionary<string, object> equippedItems { get; set; }
    public List<object> classes { get; set; }
    public List<object> effects { get; set; }
    public List<object> events { get; set; }
    public List<object> inventory { get; set; }
    public List<object> position { get; set; }
    public List<object> races { get; set; }
    public List<object> resources { get; set; }
    public List<object> rotation { get; set; }
    public string originObject { get; set; }
    public string proxyObject { get; set; }
    public string userId { get; set; }
    public long healthBase { get; set; }
    public long healthCurrent { get; set; }
    public long healthTemporary { get; set; }
    public long proficiencyBonus { get; set; }

    public RPGLObjectTemplate ToTemplate() {
        RPGLObjectTemplate template = new();
        template.PutJsonObject("ability_scores", new JsonObject(this.abilityScores));
        template.PutJsonObject("equipped_items", new JsonObject(this.equippedItems));
        template.PutJsonArray("classes", new JsonArray(this.classes));
        template.PutJsonArray("effects", new JsonArray(this.effects));
        template.PutJsonArray("events", new JsonArray(this.events));
        template.PutJsonArray("inventory", new JsonArray(this.inventory));
        template.PutJsonArray("position", new JsonArray(this.position));
        template.PutJsonArray("races", new JsonArray(this.races));
        template.PutJsonArray("resources", new JsonArray(this.resources));
        template.PutJsonArray("rotation", new JsonArray(this.rotation));
        template.PutString("origin_object", this.originObject);
        template.PutString("proxy_object", this.proxyObject);
        template.PutString("user_id", this.userId);
        template.PutInt("health_base", this.healthBase);
        template.PutInt("health_current", this.healthCurrent);
        template.PutInt("health_temporary", this.healthTemporary);
        template.PutInt("proficiency_bonus", this.proficiencyBonus);

        base.ApplyToTemplate(template);
        return template;
    }

};
