using com.rpglc.database;
using com.rpglc.json;
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

    public long GetPotency() {
        return (long) GetInt("potency");
    }

    public RPGLResource SetPotency(long potency) {
        PutInt("potency", potency);
        return this;
    }

    public bool GetExhausted() {
        return (bool) GetBool("exhausted");
    }

    public RPGLResource SetExhausted(bool exhausted) {
        PutBool("exhausted", exhausted);
        return this;
    }



    public void ProcessSubevent(Subevent subevent, RPGLObject owner) {
        if (GetExhausted()) {
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
            && new Random().NextDouble() * 100.0 <= criterion.GetInt("chance")
            && (anyActor || owner.GetUuid() == actor.GetUuid())
        ) {
            long completed = (long) criterion.GetInt("completed") + 1L;
            criterion.PutInt("completed", completed);
            if (completed > criterion.GetInt("required")) {
                Refresh();
                return true;
            }
        }
        return false;
    }

    public RPGLResource Exhaust() {
        SetExhausted(true);
        DBManager.UpdateRPGLResource(this);
        return this;
    }

    public RPGLResource Refresh() {
        SetExhausted(false);
        DBManager.UpdateRPGLResource(this);
        return this;
    }

};
