using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.testutils.subevent;

public class DummyRollSubevent : RollSubevent {

    public DummyRollSubevent() : base("dummy_roll_subevent") { }

    public override Subevent Clone() {
        DummyRollSubevent clone = new DummyRollSubevent();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        DummyRollSubevent clone = new DummyRollSubevent();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override DummyRollSubevent? Invoke(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return (DummyRollSubevent?) base.Invoke(context, originPoint, invokingEffect);
    }

    public override DummyRollSubevent JoinSubeventData(JsonObject other) {
        return (DummyRollSubevent) base.JoinSubeventData(other);
    }

    public override DummyRollSubevent Prepare(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return (DummyRollSubevent) base.Prepare(context, originPoint, invokingEffect);
    }

    public override Subevent Run(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return this;
    }

    public override DummyRollSubevent SetOriginItem(string? originItem) {
        return (DummyRollSubevent) base.SetOriginItem(originItem);
    }

    public override DummyRollSubevent SetSource(RPGLObject source) {
        return (DummyRollSubevent) base.SetSource(source);
    }

    public override DummyRollSubevent SetTarget(RPGLObject target) {
        return (DummyRollSubevent) base.SetTarget(target);
    }

};
