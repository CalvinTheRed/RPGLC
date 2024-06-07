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

    public bool ResourcesMatchCost(List<RPGLResource> resources) {
        JsonArray cost = GetCost();
        if (cost.Count() == resources.Count) {
            bool resourcesSatisfied = true;
            for (int i = 0; i < cost.Count(); i++) {
                JsonObject costData = cost.GetJsonObject(i);
                RPGLResource rpglResource = resources[i];
                if (!rpglResource.GetTags().ContainsAny(costData.GetJsonArray("resource_tags").AsList())
                    || rpglResource.GetPotency() < costData.GetInt("minimum_potency")
                ) {
                    resourcesSatisfied = false;
                    break;
                }
            }
            return resourcesSatisfied;
        }
        return false;
    }

    public void Scale(List<RPGLResource> resources) {
        JsonArray cost = GetCost();
        for (int i = 0; i < cost.Count(); i++) {
            JsonObject costData = cost.GetJsonObject(i);
            RPGLResource providedResource = resources[i];
            long potencyDifference = providedResource.GetPotency() - (long) costData.GetInt("minimum_potency");
            if (potencyDifference > 0) {
                JsonArray scaling = costData.GetJsonArray("scale");
                for (int j = 0; j < scaling.Count(); j++) {
                    JsonObject scalingData = scaling.GetJsonObject(j);
                    long magnitude = (long) scalingData.GetInt("magnitude");
                    string field = scalingData.GetString("field");
                    InsertInt(field, SeekInt(field) + (potencyDifference * magnitude));
                }
            }
        }
    }

};
