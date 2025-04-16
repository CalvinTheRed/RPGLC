using com.rpglc.core;
using com.rpglc.function;
using com.rpglc.json;

namespace com.rpglc.subevent;

/// <summary>
///   Confirms whether a critical hit should be considered a critical hit for the purpose of calculating damage.
///   
///   <br /><br />
///   <i>This subevent is unavailable to be used directly inside an RPGLEvent.</i>
///   
///   <br /><br />
///   <b>Special Functions</b>
///   <list type="bullet">
///     <item>SuppressCriticalDamage</item>
///   </list>
///   
/// </summary>
public class CriticalDamageConfirmation : Subevent {
    
    public CriticalDamageConfirmation() : base("critical_damage_confirmation") { }

    public override Subevent Clone() {
        Subevent clone = new CriticalDamageConfirmation();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new CriticalDamageConfirmation();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override CriticalDamageConfirmation? Invoke(RPGLContext context, JsonArray originPoint) {
        return (CriticalDamageConfirmation?) base.Invoke(context, originPoint);
    }

    public override CriticalDamageConfirmation JoinSubeventData(JsonObject other) {
        return (CriticalDamageConfirmation) base.JoinSubeventData(other);
    }

    public override CriticalDamageConfirmation Prepare(RPGLContext context, JsonArray originPoint) {
        json.PutIfAbsent("deals_critical_damage", true);
        return this;
    }

    public override CriticalDamageConfirmation Run(RPGLContext context, JsonArray originPoint) {
        return this;
    }

    public override CriticalDamageConfirmation SetOriginItem(string? originItem) {
        return (CriticalDamageConfirmation) base.SetOriginItem(originItem);
    }

    public override CriticalDamageConfirmation SetSource(RPGLObject source) {
        return (CriticalDamageConfirmation) base.SetSource(source);
    }

    public override CriticalDamageConfirmation SetTarget(RPGLObject target) {
        return (CriticalDamageConfirmation) base.SetTarget(target);
    }

    public CriticalDamageConfirmation SuppressCriticalDamage() {
        json.PutBool("deals_critical_damage", false);
        return this;
    }

    public bool DealsCriticalDamage() {
        return (bool) json.GetBool("deals_critical_damage");
    }

};
