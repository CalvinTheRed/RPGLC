using com.rpglc.core;

namespace com.rpglc.database.TO;

public class RPGLClassTO : DatabaseContentTO {
    public Dictionary<string, object>? StartingFeatures { get; set; }
    public Dictionary<string, object> Features { get; set; }
    public Dictionary<string, object> NestedClasses { get; set; }
    public List<object>? AbilityScoreIncreaseLevels { get; set; }
    public List<object>? MulticlassRequirements { get; set; }
    public long? SubclassLevel { get; set; }

    public RPGLClass ToRPGLClass() {
        return (RPGLClass) new RPGLClass()
            .SetStartingFeatures(StartingFeatures is not null ? new(StartingFeatures) : null)
            .SetFeatures(new(Features))
            .SetNestedClasses(new(NestedClasses))
            .SetAbilityScoreIncreaseLevels(AbilityScoreIncreaseLevels is not null ? new(AbilityScoreIncreaseLevels) : null)
            .SetMulticlassRequirements(MulticlassRequirements is not null ? new(MulticlassRequirements) : null)
            .SetSubclassLevel(SubclassLevel)
            .SetMetadata(new(Metadata))
            .SetDatapackId(DatapackId)
            .SetDescription(Description)
            .SetName(Name);
    }

};
