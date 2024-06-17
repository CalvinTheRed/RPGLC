using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

public static class VampiricSubevent {

    public static void HandleVampirism(Subevent subevent, JsonObject damageByType, RPGLContext context, JsonArray originPoint) {
        JsonObject vampirismJson = subevent.json.GetJsonObject("vampirism");
        string vampiricDamageType = vampirismJson.GetString("damage_type");

        long vampiricHealing = CalculationSubevent.Scale(
            GetVampiricDamage(damageByType, vampiricDamageType),
            vampirismJson
        );

        if (vampiricHealing > 0L) {
            HealingCollection vampiricHealingCollection = new HealingCollection()
                .JoinSubeventData(new JsonObject()
                    .PutJsonArray("tags", subevent.GetTags().DeepClone()
                        .AddString("vampiric")
                    )
                )
                .SetOriginItem(subevent.GetOriginItem())
                .SetSource(subevent.GetSource())
                .Prepare(context, originPoint)
                .AddHealing(new JsonObject()
                    .PutJsonArray("dice", new())
                    .PutLong("bonus", vampiricHealing)
                )
                .SetTarget(subevent.GetTarget())
                .Invoke(context, originPoint);

            HealingRoll vampiricHealingRoll = new HealingRoll()
                .JoinSubeventData(new JsonObject()
                    .PutJsonArray("tags", subevent.GetTags().DeepClone()
                        .AddString("vampiric")
                    )
                    .PutJsonArray("healing", vampiricHealingCollection.GetHealingCollection())
                )
                .SetOriginItem(subevent.GetOriginItem())
                .SetSource(subevent.GetSource())
                .Prepare(context, originPoint)
                .SetTarget(subevent.GetTarget())
                .Invoke(context, originPoint);

            HealingDelivery vampiricHealingDelivery = new HealingDelivery()
                .JoinSubeventData(new JsonObject()
                    .PutJsonArray("tags", subevent.GetTags().DeepClone()
                        .AddString("vampiric")
                    )
                    .PutJsonArray("healing", vampiricHealingRoll.GetHealing())
                )
                .SetOriginItem(subevent.GetOriginItem())
                .SetSource(subevent.GetSource())
                .Prepare(context, originPoint)
                .SetTarget(subevent.GetTarget())
                .Invoke(context, originPoint);

            subevent.GetSource().ReceiveHealing(vampiricHealingDelivery, context);
        }
    }

    private static long GetVampiricDamage(JsonObject damageByType, string vampiricDamageType) {
        if (Equals(vampiricDamageType, "")) {
            long vampiricDamage = 0L;
            foreach(string key in damageByType.AsDict().Keys) {
                vampiricDamage += (long) damageByType.GetLong(key);
            }
            return vampiricDamage;
        }
        return (long) damageByType.GetLong(vampiricDamageType);
    }

};
