using com.rpglc.core;

namespace com.rpglc.database.TO;

public class RPGLEffectTO : PersistentContentTO {
    public Dictionary<string, object> SubeventFilters { get; set; }
    public string? OriginItem { get; set; }
    public string? Source { get; set; }
    public string? Target { get; set; }
    
    public RPGLEffect ToRPGLEffect() {
        return (RPGLEffect) new RPGLEffect()
            .SetSubeventFilters(new(SubeventFilters))
            .SetOriginItem(OriginItem)
            .SetSource(Source)
            .SetTarget(Target)
            .SetUuid(Uuid)
            .SetMetadata(new(Metadata))
            .SetDatapackId(DatapackId)
            .SetDescription(Description)
            .SetName(Name)
            .SetId(_id);
    }

};
