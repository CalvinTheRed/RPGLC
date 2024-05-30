using com.rpglc.json;

namespace com.rpglc.core;

public class RPGLResource : TaggableContent {

    public JsonArray GetRefreshCriterion() {
        return GetJsonArray("refresh_criterion");
    }

    public RPGLResource SetRefreshCriterion(JsonArray refreshCriterion) {
        PutJsonArray("refresh_criterion", refreshCriterion);
        return this;
    }

    public long? GetOriginItem() {
        return GetInt("origin_item");
    }

    public RPGLResource SetOriginItem(long? originItem) {
        PutInt("origin_item", originItem);
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

};
