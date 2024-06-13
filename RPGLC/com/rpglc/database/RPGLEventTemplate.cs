using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.database;

public class RPGLEventTemplate : RPGLTemplate {

    public RPGLEventTemplate() : base() {

    }

    public RPGLEventTemplate(JsonObject other) : this() {
        Join(other);
    }

    public override RPGLEventTemplate ApplyBonuses(JsonArray bonuses) {
        return new(base.ApplyBonuses(bonuses));
    }

    public RPGLEvent NewInstance() {
        RPGLEvent rpglEvent = new();
        Setup(rpglEvent);
        ProcessCost(rpglEvent);

        return rpglEvent;
    }

    internal static void ProcessCost(RPGLEvent rpglEvent) {
        JsonArray cost = rpglEvent.GetCost();
        for (int i = 0; i <cost.Count(); i++) {
            JsonObject costJson = cost.GetJsonObject(i);
            if (!costJson.AsDict().ContainsKey("count")) {
                costJson.PutInt("count", 1L);
            }
        }
    }

};
