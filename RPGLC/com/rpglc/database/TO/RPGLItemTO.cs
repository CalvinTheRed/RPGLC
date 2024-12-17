using com.rpglc.core;

namespace com.rpglc.database.TO;

public class RPGLItemTO : TaggableContentTO {
    public Dictionary<string, object> Effects { get; set; }
    public Dictionary<string, object> Events { get; set; }
    public Dictionary<string, object> Resources { get; set; }
    public long Cost { get; set; }
    public long Weight { get; set; }

    public RPGLItem ToRPGLItem() {
        return (RPGLItem) new RPGLItem()
            .SetEffects(new(Effects))
            .SetEvents(new(Events))
            .SetResources(new(Resources))
            .SetCost(Cost)
            .SetWeight(Weight)
            .SetTags(new(Tags))
            .SetUuid(Uuid)
            .SetMetadata(new(Metadata))
            .SetDatapackId(DatapackId)
            .SetDescription(Description)
            .SetName(Name);
            //.SetId(_id);
    }

};
