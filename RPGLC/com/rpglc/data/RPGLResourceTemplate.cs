using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.math;

namespace com.rpglc.data;

public class RPGLResourceTemplate : RPGLTemplate {

    public RPGLResourceTemplate() : base() { }

    public RPGLResourceTemplate(JsonObject other) : base(other) { }

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

            // default frequency
            if (!criterion.AsDict().ContainsKey("frequency")) {
                criterion.PutJsonObject("frequency", new JsonObject()
                    .PutLong("bonus", 1L)
                    .PutJsonArray("dice", new())
                );
            }

            // default or unpack tries
            if (!criterion.AsDict().ContainsKey("tries")) {
                criterion.PutJsonObject("tries", new JsonObject()
                    .PutLong("bonus", 1L)
                    .PutJsonArray("dice", new())
                );
            }

            // default actor
            if (!criterion.AsDict().ContainsKey("actor")) {
                criterion.PutString("actor", "source");
            }

            // unpack dice
            criterion.InsertJsonArray(
                "frequency.dice",
                Die.Unpack(criterion.SeekJsonArray("frequency.dice"))
            );
            criterion.InsertJsonArray(
                "tries.dice",
                Die.Unpack(criterion.SeekJsonArray("tries.dice"))
            );
        }
    }

};
