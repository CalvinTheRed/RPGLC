using com.rpglc.database;
using com.rpglc.json;

namespace com.rpglc.core;

public class RPGLEvent : DatabaseContent {

    public JsonObject GetAreaOfEffect() {
        return GetJsonObject("area_of_effect");
    }

    public RPGLEvent SetAreaOfEffect(JsonObject areaOfEffect) {
        PutJsonObject("area_of_effect", areaOfEffect);
        return this;
    }

    public JsonArray GetSubevents() {
        return GetJsonArray("subevents");
    }

    public RPGLEvent SetSubevents(JsonArray subevents) {
        PutJsonArray("subevents", subevents);
        return this;
    }

    public JsonArray GetCost() {
        return GetJsonArray("cost");
    }

    public RPGLEvent SetCost(JsonArray cost) {
        PutJsonArray("cost", cost);
        return this;
    }

    public string? GetOriginItem() {
        return GetString("origin_item");
    }

    public RPGLEvent SetOriginItem(string? originItem) {
        PutString("origin_item", originItem);
        return this;
    }

    // =====================================================================
    // Utility methods.
    // =====================================================================

    public RPGLEvent SetSource(string objectUuid) {
        PutString("source", objectUuid);
        return this;
    }

    public bool ResourcesSatisfyCost(List<RPGLResource> resources) {
        JsonArray cost = GetCost().DeepClone();
        foreach (RPGLResource rpglResource in resources) {
            long availableResourceUses = rpglResource.GetAvailableUses();
            if (availableResourceUses > 0) {
                for (int i = 0; i < cost.Count(); i++) {
                    JsonObject costJson = cost.GetJsonObject(i);
                    if (rpglResource.GetTags().ContainsAll(costJson.GetJsonArray("resource_tags").AsList())
                        && rpglResource.GetPotency() >= costJson.GetInt("minimum_potency")
                        && costJson.GetInt("count") > 0
                    ) {
                        long costCount = (long) costJson.GetInt("count");
                        if (availableResourceUses <= costCount) {
                            // resource would be depleted; update costJson and move to next rpglResource
                            costJson.PutInt("count", costCount - availableResourceUses);
                            break;
                        } else {
                            // resource would not be depleted; update costJson and availableResourceUses and move to next costJson
                            availableResourceUses -= costCount;
                            costJson.PutInt("count", 0L);
                        }
                    }
                }
            } else {
                return false;
            }
        }
        // return false if any cost was not fully satisfied, else true
        for (int i = 0; i < cost.Count(); i++) {
            JsonObject costJson = cost.GetJsonObject(i);
            if (costJson.GetInt("count") != 0) {
                return false;
            }
        }
        return true;
    }

    public void Scale(List<RPGLResource> resources) {
        JsonArray cost = GetCost();
        foreach (RPGLResource rpglResource in resources) {
            long availableResourceUses = rpglResource.GetAvailableUses();
            if (availableResourceUses > 0) {
                for (int i = 0; i < cost.Count(); i++) {
                    JsonObject costJson = cost.GetJsonObject(i);
                    if (rpglResource.GetTags().ContainsAll(costJson.GetJsonArray("resource_tags").AsList())
                        && rpglResource.GetPotency() >= costJson.GetInt("minimum_potency")
                        && costJson.GetInt("count") > 0
                    ) {
                        // while the resource has uses and the cost count is greater than 0, scale once and update counters
                        long costCount = (long) costJson.GetInt("count");
                        while (availableResourceUses > 0 && costCount > 0) {
                            availableResourceUses--;
                            costCount--;
                            long potencyDifference = rpglResource.GetPotency() - (long) costJson.GetInt("minimum_potency");
                            if (potencyDifference > 0) {
                                JsonArray scaling = costJson.GetJsonArray("scale");
                                for (int j = 0; j < scaling.Count(); j++) {
                                    JsonObject scalingData = scaling.GetJsonObject(j);
                                    long magnitude = (long) scalingData.GetInt("magnitude");
                                    string field = scalingData.GetString("field");
                                    InsertInt(field, SeekInt(field) + (potencyDifference * magnitude));
                                }
                            }
                        }
                        // short-circuit if resource is fully depleted
                        if (availableResourceUses == 0) {
                            continue;
                        }
                    }
                }
            }
        }
    }

    public void SpendResources(List<RPGLResource> resources) {
        JsonArray cost = GetCost().DeepClone();
        foreach (RPGLResource rpglResource in resources) {
            long availableResourceUses = rpglResource.GetAvailableUses();
            if (availableResourceUses > 0) {
                for (int i = 0; i < cost.Count(); i++) {
                    JsonObject costJson = cost.GetJsonObject(i);
                    if (rpglResource.GetTags().ContainsAll(costJson.GetJsonArray("resource_tags").AsList())
                        && rpglResource.GetPotency() >= costJson.GetInt("minimum_potency")
                        && costJson.GetInt("count") > 0
                    ) {
                        long costCount = (long) costJson.GetInt("count");
                        if (availableResourceUses <= costCount) {
                            // deplete resource uses and update costCount, then proceed to next resource
                            costJson.PutInt("count", costCount - availableResourceUses);
                            rpglResource.SetAvailableUses(0L);
                            DBManager.UpdateRPGLResource(rpglResource);
                            break;
                        } else {
                            // update costCount and resource use count and then proceed to next costJson
                            costJson.PutInt("count", 0L);
                            availableResourceUses -= costCount;
                            rpglResource.SetAvailableUses(availableResourceUses);
                            DBManager.UpdateRPGLResource(rpglResource);
                        }
                    }
                }
            }
        }
    }

};
