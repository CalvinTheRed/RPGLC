using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.database;

public class RPGLEventTemplate : RPGLTemplate {

    public RPGLEventTemplate() {

    }

    public RPGLEventTemplate(JsonObject other) : this() {
        base.Join(other);
    }

    public override RPGLEvent NewInstance() {
        RPGLEvent rpglEvent = new();
        this.Setup(rpglEvent);

        // events should not be saved to the database
        return rpglEvent;
    }

    public override void Setup(JsonObject rpglEvent) {
        base.Setup(rpglEvent);
    }

};
