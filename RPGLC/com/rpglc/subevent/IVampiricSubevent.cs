using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

public interface IVampiricSubevent {

    public static JsonArray? GetVampirism(Subevent subevent) {
        return subevent.json.GetJsonArray("vampirism");
    }

    public void AddVampirism(Subevent subevent, JsonObject vampirismJson) {
        if (subevent is IVampiricSubevent) {
            GetVampirism(subevent).AddJsonObject(vampirismJson);
        }
    }

    public static void HandleVampirism(Subevent subevent, JsonObject damageByType, RPGLContext context, JsonArray originPoint) {
        if (subevent is IVampiricSubevent) {
            JsonArray totalVampiricHealing = GetTotalVampiricHealing(subevent, damageByType, context, originPoint);

            if (totalVampiricHealing.Count() > 0) {
                HealingDelivery vampiricHealingDelivery = new HealingDelivery()
                    .JoinSubeventData(new JsonObject().LoadFromString($$"""
                    {
                        "tags": {{subevent.GetTags().DeepClone().AddString("vampiric")}},
                        "healing": {{totalVampiricHealing}}
                    }
                    """))
                    .SetOriginItem(subevent.GetOriginItem())
                    .SetSource(subevent.GetSource())
                    .Prepare(context, originPoint)
                    .SetTarget(subevent.GetTarget())
                    .Invoke(context, originPoint);

                subevent.GetSource().ReceiveHealing(vampiricHealingDelivery, context);
            }
        }
    }

    private static JsonArray GetTotalVampiricHealing(Subevent subevent, JsonObject damageByType, RPGLContext context, JsonArray originPoint) {
        JsonArray vampirismArray = GetVampirism(subevent);
        JsonArray totalVampiricHealing = new();

        for (int i = 0; i < vampirismArray.Count(); i++) {
            JsonObject vampirismJson = vampirismArray.GetJsonObject(i);
            string vampiricDamageType = vampirismJson.GetString("damage_type") ?? "*";

            long vampiricHealing = CalculationSubevent.Scale(
                GetVampiricDamage(damageByType, vampiricDamageType),
                vampirismJson.GetJsonObject("scale") ?? new()
            );

            if (vampiricHealing > 0L) {
                HealingCollection vampiricHealingCollection = new HealingCollection()
                    .JoinSubeventData(new JsonObject().LoadFromString($$"""
                        {
                            "tags": {{subevent.GetTags().DeepClone().AddString("vampiric")}}
                        }
                        """))
                    .SetOriginItem(subevent.GetOriginItem())
                    .SetSource(subevent.GetSource())
                    .Prepare(context, originPoint)
                    .AddHealing(new JsonObject().LoadFromString($$"""
                        {
                            "bonus": {{vampiricHealing}},
                            "dice": [ ],
                            "scale": {
                                "denominator": 1,
                                "numerator": 1,
                                "round_up": false
                            }
                        }
                        """))
                    .SetTarget(subevent.GetTarget())
                    .Invoke(context, originPoint);

                HealingRoll vampiricHealingRoll = new HealingRoll()
                    .JoinSubeventData(new JsonObject().LoadFromString($$"""
                        {
                            "tags": {{subevent.GetTags().DeepClone().AddString("vampiric")}},
                            "healing": {{vampiricHealingCollection.GetHealingCollection()}}
                        }
                        """))
                    .SetOriginItem(subevent.GetOriginItem())
                    .SetSource(subevent.GetSource())
                    .Prepare(context, originPoint)
                    .SetTarget(subevent.GetTarget())
                    .Invoke(context, originPoint);

                JsonArray vampiricHealingRollArray = vampiricHealingRoll.GetHealing();
                for (int j = 0; j < vampiricHealingRollArray.Count(); j++) {
                    totalVampiricHealing.AddJsonObject(vampiricHealingRollArray.GetJsonObject(j));
                }
            }
        }

        return totalVampiricHealing;
    }

    private static long GetVampiricDamage(JsonObject damageByType, string vampiricDamageType) {
        if (vampiricDamageType == "*") {
            long vampiricDamage = 0L;
            foreach(string key in damageByType.AsDict().Keys) {
                vampiricDamage += (long) damageByType.GetLong(key);
            }
            return vampiricDamage;
        }
        return (long) damageByType.GetLong(vampiricDamageType);
    }

};
