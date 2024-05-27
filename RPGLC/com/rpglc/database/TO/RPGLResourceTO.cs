namespace com.rpglc.database.TO;

public class RPGLResourceTO : TaggableContentTO {
    public List<object> refreshCriterion { get; set; }
    public string originItem { get; set; }
    public Int64 potency { get; set; }
    public bool exhausted { get; set; }
};
