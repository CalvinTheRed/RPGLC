using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.data;

public class RPGLEventTemplate : RPGLTemplate {

    public RPGLEventTemplate() : base() { }

    public RPGLEventTemplate(JsonObject other) : base(other) { }

    public override RPGLEventTemplate ApplyBonuses(JsonArray bonuses) {
        return new(base.ApplyBonuses(bonuses));
    }

    public RPGLEvent NewInstance() {
        RPGLEvent rpglEvent = new();
        Setup(rpglEvent);
        ProcessCost(rpglEvent);

        return rpglEvent;
    }

    private static void ProcessCost(RPGLEvent rpglEvent) {
        JsonArray cost = rpglEvent.GetCost();
        for (int i = 0; i <cost.Count(); i++) {
            JsonObject costJson = cost.GetJsonObject(i);
            if (!costJson.AsDict().ContainsKey("count")) {
                costJson.PutLong("count", 1L);
            }
        }
    }

};
