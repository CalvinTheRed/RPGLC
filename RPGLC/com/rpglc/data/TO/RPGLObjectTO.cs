using com.rpglc.core;

namespace com.rpglc.data.TO;

public class RPGLObjectTO : TaggableContentTO {
    public Dictionary<string, object> AbilityScores {  get; set; }
    public Dictionary<string, object> EquippedItems { get; set; }
    public Dictionary<string, object> HealthTemporary { get; set; }
    public List<object> Classes { get; set; }
    public List<object> Events { get; set; }
    public List<object> Inventory { get; set; }
    public List<object> Position { get; set; }
    public List<object> Races { get; set; }
    public List<object> Resources { get; set; }
    public List<object> Rotation { get; set; }
    public string? OriginObject { get; set; }
    public string UserId { get; set; }
    public long? ProficiencyBonus { get; set; }
    public long HealthBase { get; set; }
    public long HealthCurrent { get; set; }
    public bool? Proxy { get; set; }

    public RPGLObject ToRPGLObject() {
        return (RPGLObject) new RPGLObject()
            .SetAbilityScores(new(AbilityScores))
            .SetEquippedItems(new(EquippedItems))
            .SetHealthTemporary(new(HealthTemporary))
            .SetClasses(new(Classes))
            .SetEvents(new(Events))
            .SetInventory(new(Inventory))
            .SetPosition(new(Position))
            .SetRaces(new(Races))
            .SetResources(new(Resources))
            .SetRotation(new(Rotation))
            .SetOriginObject(OriginObject)
            .SetUserId(UserId)
            .SetProficiencyBonus(ProficiencyBonus)
            .SetHealthBase(HealthBase)
            .SetHealthCurrent(HealthCurrent)
            .SetProxy(Proxy)
            .SetTags(new(Tags))
            .SetUuid(Uuid)
            .SetMetadata(new(Metadata))
            .SetDatapackId(DatapackId)
            .SetDescription(Description)
            .SetName(Name);
            //.SetId(_id);
    }
};
