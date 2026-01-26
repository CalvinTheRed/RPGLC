using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.runtime;
using com.rpglc.subevent;

namespace com.rpglc.testutils.subevent;

public class DummyVampiricSubevent : Subevent, IVampiricSubevent {

    public DummyVampiricSubevent() : base("dummy_vampiric_subevent") {
        subeventSteps.AddRange([
            (context) => {
                dependency = new(new DamageDelivery()
                    .SetOriginItem(GetOriginItem())
                    .SetSource(GetSource())
                    .SetTarget(GetTarget()));
                
                json.PutIfAbsent("vampirism", new JsonArray().LoadFromString("""
                    [
                        {
                            "damage_type": "necrotic",
                            "scale": {
                                "numerator": 1,
                                "denominator": 2,
                                "round_up": false
                            }
                        }
                    ]
                    """));

                IVampiricSubevent.AddVampirismSteps(this);

                return new() {
                    dependency = dependency,
                    nextPhase = SubeventState.Phase.Running,
                    stepCompleted = true,
                };
            },
        ]);
    }

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
