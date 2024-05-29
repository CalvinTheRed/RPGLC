using com.rpglc.core;

namespace com.rpglc.database.TO;

public class RPGLItemTO : TaggableContentTO {
    public List<object> Effects { get; set; }
    public List<object> Events { get; set; }
    public List<object> Resources { get; set; }
    public long Cost { get; set; }
    public long Weight { get; set; }

    public RPGLItem ToRPGLItem() {
        return (RPGLItem) new RPGLItem()
            .SetEffects(new(Effects))
            .SetEvents(new(Events))
            .SetResources(new(Resources))
            .SetCost(Cost)
            .SetWeight(Weight)
            .SetUuid(Uuid)
            .SetMetadata(new(Metadata))
            .SetDatapackId(DatapackId)
            .SetDescription(Description)
            .SetName(Name);
    }

};
