using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

/// <summary>
///   Calculates an object's armor class.
///   
///   <br /><br />
///   <i>This subevent is unavailable to be used directly inside an RPGLEvent.</i>
///   
///   <br /><br />
///   <b>Special Functions</b>
///   <list type="bullet">
///     <item>AddBonus</item>
///     <item>SetBase</item>
///     <item>SetMinimum</item>
///   </list>
///   
/// </summary>
public class CalculateArmorClass : CalculationSubevent {

    public CalculateArmorClass() : base("calculate_armor_class") { }

    public override Subevent Clone() {
        Subevent clone = new CalculateArmorClass();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new CalculateArmorClass();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override CalculateArmorClass? Invoke(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return (CalculateArmorClass?) base.Invoke(context, originPoint, invokingEffect);
    }

    public override CalculateArmorClass JoinSubeventData(JsonObject other) {
        return (CalculateArmorClass) base.JoinSubeventData(other);
    }

    public override CalculateArmorClass Prepare(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        base.Prepare(context, originPoint, invokingEffect);
        SetBase(10L);
        return this;
    }

    public override CalculateArmorClass Run(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return this;
    }

    public override CalculateArmorClass SetOriginItem(string? originItem) {
        return (CalculateArmorClass) base.SetOriginItem(originItem);
    }

    public override CalculateArmorClass SetSource(RPGLObject source) {
        return (CalculateArmorClass) base.SetSource(source);
    }

    public override CalculateArmorClass SetTarget(RPGLObject target) {
        return (CalculateArmorClass) base.SetTarget(target);
    }

};
