using com.rpglc.json;

namespace com.rpglc.core;

public class RPGLItem : TaggableContent {

    public JsonObject GetEffects() {
        return GetJsonObject("effects");
    }

    public RPGLItem SetEffects(JsonObject effects) {
        PutJsonObject("effects", effects);
        return this;
    }

    public JsonObject GetEvents() {
        return GetJsonObject("events");
    }

    public RPGLItem SetEvents(JsonObject events) {
        PutJsonObject("events", events);
        return this;
    }

    public JsonObject GetResources() {
        return GetJsonObject("resources");
    }

    public RPGLItem SetResources(JsonObject resources) {
        PutJsonObject("resources", resources);
        return this;
    }

    public long GetCost() {
        return (long) GetLong("cost");
    }

    public RPGLItem SetCost(long cost) {
        PutLong("cost", cost);
        return this;
    }

    public long GetWeight() {
        return (long) GetLong("weight");
    }

    public RPGLItem SetWeight(long weight) {
        PutLong("weight", weight);
        return this;
    }

    // =====================================================================
    // Equipped features helper methods.
    // =====================================================================

    public List<RPGLEffect> GetEffectsForSlots(List<string> slots) {
        List<RPGLEffect> grantedEffects = [];
        JsonObject effects = GetEffects();
        foreach (string effectUuid in effects.AsDict().Keys) {
            JsonArray possibleSlotCombinations = effects.GetJsonArray(effectUuid);
            for (int i = 0; i < possibleSlotCombinations.Count(); i++) {
                JsonArray slotCombination = possibleSlotCombinations.GetJsonArray(i);
                bool slotCombinationMatch = true;
                foreach (string slot in slots) {
                    slotCombinationMatch &= slotCombination.Contains(slot);
                }
                for (int j = 0; j < slotCombination.Count(); j++) {
                    slotCombinationMatch &= slots.Contains(slotCombination.GetString(j));
                }
                if (slotCombinationMatch) {
                    grantedEffects.Add(RPGL.GetRPGLEffect(effectUuid));
                    break;
                }
            }
        }
        return grantedEffects;
    }

    public List<RPGLEvent> GetEventsForSlots(List<string> slots) {
        List<RPGLEvent> grantedEvents = [];
        JsonObject events = GetEvents();
        foreach (string eventDatapackId in events.AsDict().Keys) {
            JsonArray possibleSlotCombinations = events.GetJsonArray(eventDatapackId);
            for (int i = 0; i < possibleSlotCombinations.Count(); i++) {
                JsonArray slotCombination = possibleSlotCombinations.GetJsonArray(i);
                bool slotCombinationMatch = true;
                foreach (string slot in slots) {
                    slotCombinationMatch &= slotCombination.Contains(slot);
                }
                for (int j = 0; j < slotCombination.Count(); j++) {
                    slotCombinationMatch &= slots.Contains(slotCombination.GetString(j));
                }
                if (slotCombinationMatch) {
                    grantedEvents.Add(RPGL.GetRPGLEventTemplate(eventDatapackId)
                        .NewInstance());
                    break;
                }
            }
        }
        return grantedEvents;
    }

    public List<RPGLResource> GetResourcesForSlots(List<string> slots) {
        List<RPGLResource> grantedResources = [];
        JsonObject resources = GetResources();
        foreach (string resourceUuid in resources.AsDict().Keys) {
            JsonArray possibleSlotCombinations = resources.GetJsonArray(resourceUuid);
            for (int i = 0; i < possibleSlotCombinations.Count(); i++) {
                JsonArray slotCombination = possibleSlotCombinations.GetJsonArray(i);
                bool slotCombinationMatch = true;
                foreach (string slot in slots) {
                    slotCombinationMatch &= slotCombination.Contains(slot);
                }
                for (int j = 0; j < slotCombination.Count(); j++) {
                    slotCombinationMatch &= slots.Contains(slotCombination.GetString(j));
                }
                if (slotCombinationMatch) {
                    grantedResources.Add(RPGL.GetRPGLResource(resourceUuid));
                    break;
                }
            }
        }
        return grantedResources;
    }

};
