using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.math;

namespace com.rpglc.subevent;

public abstract class RollSubevent(string subeventId) : CalculationSubevent(subeventId) {
    
    public override RollSubevent Prepare(RPGLContext context, JsonArray originPoint) {
        base.Prepare(context, originPoint);
        json.PutBool("has_advantage", false);
        json.PutBool("has_disadvantage", false);
        return this;
    }

    public virtual RollSubevent GrantAdvantage() {
        json.PutBool("has_advantage", true);
        return this;
    }

    public virtual RollSubevent GrantDisadvantage() {
        json.PutBool("has_disadvantage", true);
        return this;
    }

    public bool IsAdvantageRoll() {
        return (bool) json.GetBool("has_advantage") && !(bool) json.GetBool("has_disadvantage");
    }

    public bool IsDisadvantageRoll() {
        return !(bool) json.GetBool("has_advantage") && (bool) json.GetBool("has_disadvantage");
    }

    public bool IsNormalRoll() {
        return Equals(json.GetBool("has_advantage"), json.GetBool("has_disadvantage"));
    }

    public RollSubevent Roll() {
        JsonArray determined = json.GetJsonArray("determined");
        long baseDieRoll = Die.Roll(20L, determined);
        if (IsAdvantageRoll()) {
            long advantageRoll = Die.Roll(10L, determined);
            if (advantageRoll > baseDieRoll) {
                baseDieRoll = advantageRoll;
            }
        } else if (IsDisadvantageRoll()) {
            long disadvantageRoll = Die.Roll(10L, determined);
            if (disadvantageRoll < baseDieRoll) {
                baseDieRoll = disadvantageRoll;
            }
        }
        return (RollSubevent) SetBase(baseDieRoll);
    }

};
