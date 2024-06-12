﻿using com.rpglc.database;
using com.rpglc.json;
using com.rpglc.math;
using com.rpglc.subevent;

namespace com.rpglc.core;

public class RPGLResource : TaggableContent {

    public JsonArray GetRefreshCriterion() {
        return GetJsonArray("refresh_criterion");
    }

    public RPGLResource SetRefreshCriterion(JsonArray refreshCriterion) {
        PutJsonArray("refresh_criterion", refreshCriterion);
        return this;
    }

    public string? GetOriginItem() {
        return GetString("origin_item");
    }

    public RPGLResource SetOriginItem(string? originItem) {
        PutString("origin_item", originItem);
        return this;
    }

    public long GetAvailableUses() {
        return (long) GetInt("available_uses");
    }

    public RPGLResource SetAvailableUses(long availableUses) {
        PutInt("available_uses", availableUses);
        return this;
    }

    public long GetMaximumUses() {
        return (long) GetInt("maximum_uses");
    }

    public RPGLResource SetMaximumUses(long maximumUses) {
        PutInt("maximum_uses", maximumUses);
        return this;
    }

    public long GetPotency() {
        return (long) GetInt("potency");
    }

    public RPGLResource SetPotency(long potency) {
        PutInt("potency", potency);
        return this;
    }



    public void ProcessSubevent(Subevent subevent, RPGLObject owner) {
        if (GetAvailableUses() < GetMaximumUses()) {
            JsonArray refreshCriterion = GetRefreshCriterion();
            for (int i = 0; i < refreshCriterion.Count(); i++) {
                if (CheckCriterion(subevent, refreshCriterion.GetJsonObject(i), owner)) {
                    break;
                }
            }
        }
    }

    private bool CheckCriterion(Subevent subevent, JsonObject criterion, RPGLObject owner) {
        string actorAlias = criterion.GetString("actor"); // TODO could this have a better field name?
        RPGLObject? actor = null;
        bool anyActor = false;
        if (actorAlias == "source") { 
            actor = subevent.GetSource();
        } else if (actorAlias == "target") {
            actor = subevent.GetTarget();
        } else if (actorAlias == "any") {
            anyActor = true;
        }
        if (subevent.GetSubeventId() == criterion.GetString("subevent")
            && subevent.GetTags().ContainsAll(criterion.GetJsonArray("tags").AsList())
            && (anyActor || owner.GetUuid() == actor.GetUuid())
        ) {
            return AttemptRefresh(criterion);
        }
        return false;
    }

    public RPGLResource Exhaust() {
        if (GetAvailableUses() > 0) {
            SetAvailableUses(GetAvailableUses() - 1);
            DBManager.UpdateRPGLResource(this);
        }
        return this;
    }

    public bool AttemptRefresh(JsonObject criterion) {
        // should be a function of frequency, tries, and chance, not a guaranteed refresh of 1
        if (GetAvailableUses() < GetMaximumUses()) {
            bool refreshed = false;
            // ensure countdown is active
            if (!criterion.AsDict().ContainsKey("frequency_countdown")) {
                long countdownInitialValue = (long) criterion.SeekInt("frequency.bonus");
                JsonArray dice = criterion.SeekJsonArray("frequency.dice");
                for (int i = 0; i < dice.Count(); i++) {
                    countdownInitialValue += Die.Roll(dice.GetJsonObject(i));
                }
                criterion.PutInt("frequency_countdown", countdownInitialValue);
            }

            // update countdown
            long updatedCountdown = (long) criterion.RemoveInt("frequency_countdown") - 1L;
            if (updatedCountdown > 0) {
                // decrement countdown
                criterion.PutInt("frequency_countdown", updatedCountdown);
            } else {
                // determine how many tries to make
                JsonObject triesJson = criterion.GetJsonObject("tries");
                long tries = (long) triesJson.GetInt("bonus");
                JsonArray dice = triesJson.GetJsonArray("dice");
                for (int i = 0; i < dice.Count(); i++) {
                    tries += Die.Roll(dice.GetJsonObject(i));
                }
                for (int i = 0; i < tries; i++) {
                    // chance of each try succeeding at refreshing a use
                    if (GetAvailableUses() < GetMaximumUses() 
                        && Die.Random() <= (criterion.GetDouble("chance") ?? 1.0)
                    ) {
                        SetAvailableUses(GetAvailableUses() + 1L);
                        refreshed = true;
                    }
                }

                // TODO should the countdowns be removed from other criteria at this point?
            }
            DBManager.UpdateRPGLResource(this);
            return refreshed;
        }
        return false;
    }

};
