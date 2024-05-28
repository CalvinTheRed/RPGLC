﻿using com.rpglc.json;

namespace com.rpglc.core;

public class RPGLResource : TaggableContent {

    public JsonArray GetRefreshCriterion() {
        return base.GetJsonArray("resource_criterion");
    }

    public RPGLResource SetRefreshCriterion(JsonArray refreshCriterion) {
        base.PutJsonArray("refresh_criterion", refreshCriterion);
        return this;
    }

    public string GetOriginItem() {
        return base.GetString("origin_item");
    }

    public RPGLResource SetOriginItem(string originItem) {
        base.PutString("origin_item", originItem);
        return this;
    }

    public long GetPotency() {
        return base.GetInt("potency");
    }

    public RPGLResource SetPotency(long potency) {
        base.PutInt("potency", potency);
        return this;
    }

    public bool GetExhausted() {
        return base.GetBool("exhausted");
    }

    public RPGLResource SetExhausted(bool exhausted) {
        base.PutBool("exhausted", exhausted);
        return this;
    }

};
