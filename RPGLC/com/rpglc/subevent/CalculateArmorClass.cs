using com.rpglc.core;
using com.rpglc.function;
using com.rpglc.json;

namespace com.rpglc.subevent;

public class CalculateArmorClass : CalculationSubevent {

    public CalculateArmorClass() : base("calculate_armor_class") {

    }

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

    public override CalculateArmorClass? Invoke(RPGLContext context, JsonArray originPoint) {
        return (CalculateArmorClass) base.Invoke(context, originPoint);
    }

    public override CalculateArmorClass JoinSubeventData(JsonObject other) {
        return (CalculateArmorClass) base.JoinSubeventData(other);
    }

    public override CalculateArmorClass Prepare(RPGLContext context, JsonArray originPoint) {
        base.Prepare(context, originPoint);
        SetBase(10L);
        return this;
    }

    public override CalculateArmorClass Run(RPGLContext context, JsonArray originPoint) {
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
