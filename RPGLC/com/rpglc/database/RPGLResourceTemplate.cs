using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.database;

public class RPGLResourceTemplate : RPGLTemplate {

    public RPGLResourceTemplate() : base() { }

    public RPGLResourceTemplate(JsonObject other) : this() {
        Join(other);
    }

    public override RPGLResourceTemplate ApplyBonuses(JsonArray bonuses) {
        return new(base.ApplyBonuses(bonuses));
    }

    public RPGLResource NewInstance(string uuid) {
        RPGLResource rpglResource = (RPGLResource) new RPGLResource().SetUuid(uuid);
        Setup(rpglResource);
        ProcessUses(rpglResource);
        ProcessRefreshCriterion(rpglResource);

        return rpglResource;
    }

    private static void ProcessUses(RPGLResource rpglResource) {
        if (rpglResource.AsDict().ContainsKey("maximum_uses")) {
            rpglResource.SetAvailableUses(rpglResource.GetMaximumUses());
        } else {
            rpglResource.SetMaximumUses(1);
            rpglResource.SetAvailableUses(1);
        }
    }

    private static void ProcessRefreshCriterion(RPGLResource rpglResource) {
        JsonArray refreshCriterion = rpglResource.GetRefreshCriterion();
        for (int i = 0; i < refreshCriterion.Count(); i++) {
            JsonObject criterion = refreshCriterion.GetJsonObject(i);
            if (!criterion.AsDict().ContainsKey("frequency")) {
                criterion.PutJsonObject("frequency", new JsonObject()
                    .PutInt("bonus", 1L)
                    .PutJsonArray("dice", new())
                );
            }
            if (!criterion.AsDict().ContainsKey("tries")) {
                criterion.PutJsonObject("tries", new JsonObject()
                    .PutInt("bonus", 1L)
                    .PutJsonArray("dice", new())
                );
            }
            if (!criterion.AsDict().ContainsKey("chance")) {
                criterion.PutJsonObject("tries", new JsonObject()
                    .PutInt("numerator", 1L)
                    .PutInt("denominator", 1L)
                );
            }
        }
    }

};
