using com.rpglc.core;

namespace com.rpglc.database.TO;

public class RPGLObjectTO : TaggableContentTO {
    public Dictionary<string, object> AbilityScores {  get; set; }
    public Dictionary<string, object> EquippedItems { get; set; }
    public List<object> Classes { get; set; }
    public List<object> Effects { get; set; }
    public List<object> Events { get; set; }
    public List<object> Inventory { get; set; }
    public List<object> Position { get; set; }
    public List<object> Races { get; set; }
    public List<object> Resources { get; set; }
    public List<object> Rotation { get; set; }
    public string UserId { get; set; }
    public long? OriginObject { get; set; }
    public long? ProxyObject { get; set; }
    public long HealthBase { get; set; }
    public long HealthCurrent { get; set; }
    public long HealthTemporary { get; set; }
    public long ProficiencyBonus { get; set; }

    public RPGLObject ToRPGLObject() {
        return (RPGLObject) new RPGLObject()
            .SetAbilityScores(new(AbilityScores))
            .SetEquippedItems(new(EquippedItems))
            .SetClasses(new(Classes))
            .SetClasses(new(Effects))
            .SetEvents(new(Events))
            .SetInventory(new(Inventory))
            .SetPosition(new(Position))
            .SetRaces(new(Races))
            .SetResources(new(Resources))
            .SetRotation(new(Rotation))
            .SetUserId(UserId)
            .SetOriginObject(OriginObject)
            .SetProxyObject(ProxyObject)
            .SetHealthBase(HealthBase)
            .SetHealthCurrent(HealthCurrent)
            .SetHealthTemporary(HealthTemporary)
            .SetProficiencyBonus(ProficiencyBonus)
            .SetUuid(Uuid)
            .SetMetadata(new(Metadata))
            .SetDatapackId(DatapackId)
            .SetDescription(Description)
            .SetName(Name);
    }
};
