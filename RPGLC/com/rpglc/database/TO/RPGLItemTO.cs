namespace com.rpglc.database.TO;

public class RPGLItemTO {
    public long weight { get; set; }
    public long cost { get; set; }
    public List<object> effects { get; set; }
    public List<object> events { get; set; }
    public List<object> resources { get; set; }
};
