using com.rpglc.json;

namespace com.rpglc.data.TO;

public class TaggableContentTO : PersistentContentTO {
    public List<object> Tags { get; set; }

    override public void ApplyToTemplate(RPGLTemplate template) {
        base.ApplyToTemplate(template);
        template.PutJsonArray("tags", new JsonArray(Tags));
    }

};
