using com.rpglc.core;

namespace com.rpglc.database.TO;

public class RPGLRaceTO : DatabaseContentTO {
    public Dictionary<string, object> AbilityScoreBonuses { get; set; }
    public Dictionary<string, object> Features { get; set; }

    public RPGLRace ToRPGLRace() {
        return (RPGLRace) new RPGLRace()
            .SetAbilityScoreBonuses(new(AbilityScoreBonuses))
            .SetFeatures(new(Features))
            .SetMetadata(new(Metadata))
            .SetDatapackId(DatapackId)
            .SetDescription(Description)
            .SetName(Name);
    }

};
