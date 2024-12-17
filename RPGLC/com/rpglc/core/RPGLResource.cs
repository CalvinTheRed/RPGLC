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
        return (long) GetLong("available_uses");
    }

    public RPGLResource SetAvailableUses(long availableUses) {
        PutLong("available_uses", availableUses);
        return this;
    }

    public long GetMaximumUses() {
        return (long) GetLong("maximum_uses");
    }

    public RPGLResource SetMaximumUses(long maximumUses) {
        PutLong("maximum_uses", maximumUses);
        return this;
    }

    public long GetPotency() {
        return (long) GetLong("potency");
    }

    public RPGLResource SetPotency(long potency) {
        PutLong("potency", potency);
        return this;
    }

    // =====================================================================
    // Utility methods.
    // =====================================================================

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
        string actorAlias = criterion.GetString("actor");
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

    public RPGLResource Exhaust(long uses) {
        if (GetAvailableUses() >= uses) {
            SetAvailableUses(GetAvailableUses() - uses);

            // ensure countdowns are active
            JsonArray refreshCriterion = GetRefreshCriterion();
            for (int i = 0; i < refreshCriterion.Count(); i++) {
                JsonObject criterion = refreshCriterion.GetJsonObject(i);
                if (!criterion.AsDict().ContainsKey("frequency_countdown")) {
                    long countdownInitialValue = (long) criterion.SeekLong("frequency.bonus");
                    JsonArray dice = criterion.SeekJsonArray("frequency.dice");
                    for (int j = 0; j < dice.Count(); i++) {
                        countdownInitialValue += Die.Roll(dice.GetJsonObject(j));
                    }
                    criterion.PutLong("frequency_countdown", countdownInitialValue);
                }
            }
        }
        return this;
    }

    public bool AttemptRefresh(JsonObject criterion) {
        if (GetAvailableUses() < GetMaximumUses()) {
            bool refreshed = false;
            // update countdown
            long updatedCountdown = (long) criterion.RemoveLong("frequency_countdown") - 1L;
            if (updatedCountdown > 0) {
                // decrement countdown
                criterion.PutLong("frequency_countdown", updatedCountdown);
            } else {
                // determine how many tries to make
                JsonObject triesJson = criterion.GetJsonObject("tries");
                long tries = (long) triesJson.GetLong("bonus");
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
            return refreshed;
        }
        return false;
    }

};
