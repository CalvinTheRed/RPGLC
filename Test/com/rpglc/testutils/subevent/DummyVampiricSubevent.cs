using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.testutils.subevent;

public class DummyVampiricSubevent : Subevent, IVampiricSubevent {

    public DummyVampiricSubevent() : base("dummy_vampiric_subevent") { }

    public override Subevent Clone() {
        return this;
    }

    public override Subevent Clone(JsonObject jsonData) {
        return this;
    }

    public override DummyVampiricSubevent? Invoke(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return (DummyVampiricSubevent?) base.Invoke(context, originPoint, invokingEffect);
    }

    public override DummyVampiricSubevent JoinSubeventData(JsonObject other) {
        return (DummyVampiricSubevent) base.JoinSubeventData(other);
    }

    public override DummyVampiricSubevent Prepare(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        json.PutIfAbsent("vampirism", new JsonArray());
        return this;
    }

    public override DummyVampiricSubevent Run(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return this;
    }

    public override DummyVampiricSubevent SetOriginItem(string? originItem) {
        return (DummyVampiricSubevent) base.SetOriginItem(originItem);
    }

    public override DummyVampiricSubevent SetSource(RPGLObject source) {
        return (DummyVampiricSubevent) base.SetSource(source);
    }

    public override DummyVampiricSubevent SetTarget(RPGLObject target) {
        return (DummyVampiricSubevent) base.SetTarget(target);
    }

};
