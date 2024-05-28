namespace com.rpglc.database.TO;

public class RPGLEffectTO : PersistentContentTO {
    public Dictionary<string, object> subeventFilters { get; set; }
    public string originItem { get; set; }
    public string source { get; set; }
    public string target { get; set; }
};
