namespace com.rpglc.database.TO;

public class TaggableContentTO : DatabaseContentTO {
    public List<object> tags { get; set; }

    override public void ApplyToTemplate(RPGLTemplate template) {
        base.ApplyToTemplate(template);
        template.PutJsonArray("tags", new json.JsonArray(this.tags));
    }
};
