using com.rpglc.core;

namespace com.rpglc.database.TO;

public class RPGLResourceTO : TaggableContentTO {
    public List<object> RefreshCriterion { get; set; }
    public long? OriginItem { get; set; }
    public long Potency { get; set; }
    public bool Exhausted { get; set; }

    public RPGLResource ToRPGLResource() {
        return (RPGLResource) new RPGLResource()
            .SetRefreshCriterion(new(RefreshCriterion))
            .SetOriginItem(OriginItem)
            .SetPotency(Potency)
            .SetExhausted(Exhausted)
            .SetUuid(Uuid)
            .SetMetadata(new(Metadata))
            .SetDatapackId(DatapackId)
            .SetDescription(Description)
            .SetName(Name)
            .SetId(_id);
    }
};
