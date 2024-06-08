﻿using com.rpglc.database;
using com.rpglc.database.TO;
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

    public JsonObject GetResources() {
        return GetJsonObject("resources");
    }

    public RPGLItem SetResources(JsonObject resources) {
        PutJsonObject("resources", resources);
        return this;
    }

    public JsonArray GetEvents() {
        return GetJsonArray("events");
    }

    public RPGLItem SetEvents(JsonArray events) {
        PutJsonArray("events", events);
        return this;
    }

    public long GetCost() {
        return (long) GetInt("cost");
    }

    public RPGLItem SetCost(long cost) {
        PutInt("cost", cost);
        return this;
    }

    public long GetWeight() {
        return (long) GetInt("weight");
    }

    public RPGLItem SetWeight(long weight) {
        PutInt("weight", weight);
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
                    grantedEffects.Add(DBManager.QueryRPGLEffect(x => x.Uuid == effectUuid));
                    break;
                }
            }
        }
        return grantedEffects;
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
                    grantedResources.Add(DBManager.QueryRPGLResource(x => x.Uuid == resourceUuid));
                    break;
                }
            }
        }
        return grantedResources;
    }

};
