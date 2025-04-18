﻿using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.testutils.subevent;

public class DummySubevent : Subevent, IAbilitySubevent, IDamageTypeSubevent {

    public static long Counter = 0L;

    public DummySubevent() : base("dummy_subevent") { }

    public override Subevent Clone() {
        Subevent clone = new DummySubevent();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new DummySubevent();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override DummySubevent? Invoke(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return (DummySubevent?) base.Invoke(context, originPoint, invokingEffect);
    }

    public override DummySubevent JoinSubeventData(JsonObject other) {
        return (DummySubevent) base.JoinSubeventData(other);
    }

    public override DummySubevent Prepare(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return this;
    }

    public override DummySubevent Run(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        Counter++;
        return this;
    }

    public override DummySubevent SetOriginItem(string? originItem) {
        return (DummySubevent) base.SetOriginItem(originItem);
    }

    public override DummySubevent SetSource(RPGLObject source) {
        return (DummySubevent) base.SetSource(source);
    }

    public override DummySubevent SetTarget(RPGLObject target) {
        return (DummySubevent) base.SetTarget(target);
    }

    public string GetAbility(RPGLContext context) {
        return "str";
    }

    public bool IncludesDamageType(string damageType) {
        return Equals(damageType, "fire");
    }

    public static void ResetCounter() {
        Counter = 0L;
    }

};
