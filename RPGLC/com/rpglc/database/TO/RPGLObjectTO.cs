﻿namespace com.rpglc.database.TO;

public class RPGLObjectTO : TaggableContentTO {
    public Dictionary<string, object> abilityScores {  get; set; }
    public Dictionary<string, object> equippedItems { get; set; }
    public List<object> classes { get; set; }
    public List<object> effects { get; set; }
    public List<object> events { get; set; }
    public List<object> inventory { get; set; }
    public List<object> races { get; set; }
    public List<object> resources { get; set; }
    public List<object> position { get; set; }
    public List<object> rotation { get; set; }
    public string originObject { get; set; }
    public string proxyObject { get; set; }
    public string userId { get; set; }
    public long healthBase { get; set; }
    public long healthCurrent { get; set; }
    public long healthTemporary { get; set; }
    public long proficiencyBonus { get; set; }
};
