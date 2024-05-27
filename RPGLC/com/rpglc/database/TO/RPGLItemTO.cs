namespace com.rpglc.database.TO;

public class RPGLItemTO {
    public Int64 weight { get; set; }
    public Int64 cost { get; set; }
    public List<object> effects { get; set; }
    public List<object> events { get; set; }
    public List<object> resources { get; set; }
};
