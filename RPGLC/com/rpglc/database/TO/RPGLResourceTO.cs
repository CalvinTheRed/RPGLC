using com.rpglc.core;

namespace com.rpglc.database.TO;

public class RPGLResourceTO : TaggableContentTO {
    public List<object> RefreshCriterion { get; set; }
    public string? OriginItem { get; set; }
    public long AvailableUses { get; set; }
    public long MaximumUses { get; set; }
    public long Potency { get; set; }
    

    public RPGLResource ToRPGLResource() {
        return (RPGLResource) new RPGLResource()
            .SetRefreshCriterion(new(RefreshCriterion))
            .SetOriginItem(OriginItem)
            .SetAvailableUses(AvailableUses)
            .SetMaximumUses(MaximumUses)
            .SetPotency(Potency)
            .SetTags(new(Tags))
            .SetUuid(Uuid)
            .SetMetadata(new(Metadata))
            .SetDatapackId(DatapackId)
            .SetDescription(Description)
            .SetName(Name)
            .SetId(_id);
    }
};
