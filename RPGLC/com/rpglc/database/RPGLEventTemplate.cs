using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.database;

public class RPGLEventTemplate : RPGLTemplate {

    public RPGLEventTemplate() : base() {

    }

    public RPGLEventTemplate(JsonObject other) : this() {
        Join(other);
    }

    public RPGLEvent NewInstance() {
        RPGLEvent rpglEvent = new();
        Setup(rpglEvent);
        ProcessCost(rpglEvent);

        // events should not be saved to the database
        return rpglEvent;
    }

    public override RPGLEventTemplate ApplyBonuses(JsonArray bonuses) {
        return new(base.ApplyBonuses(bonuses));
    }

    internal static void ProcessCost(RPGLEvent rpglEvent) {
        JsonArray rawCost = rpglEvent.GetCost();
        JsonArray processedCost = new();
        for (int i = 0; i < rawCost.Count(); i++) {
            JsonObject rawCostElement = rawCost.GetJsonObject(i);
            long count = rawCostElement.RemoveInt("count") ?? 1L;
            for (int j = 0; j < count; j++) {
                processedCost.AddJsonObject(rawCostElement.DeepClone());
            }
        }
        rpglEvent.SetCost(processedCost);
    }

};
