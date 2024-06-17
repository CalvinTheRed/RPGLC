using com.rpglc.core;
using com.rpglc.function;
using com.rpglc.json;

namespace com.rpglc.subevent;

public class CalculateMaximumHitPoints : CalculationSubevent {

    public CalculateMaximumHitPoints() : base("calculate_maximum_hit_points") {

    }

    public override Subevent Clone() {
        Subevent clone = new CalculateMaximumHitPoints();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new CalculateMaximumHitPoints();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override CalculateMaximumHitPoints? Invoke(RPGLContext context, JsonArray originPoint) {
        return (CalculateMaximumHitPoints) base.Invoke(context, originPoint);
    }

    public override CalculateMaximumHitPoints JoinSubeventData(JsonObject other) {
        return (CalculateMaximumHitPoints) base.JoinSubeventData(other);
    }

    public override CalculateMaximumHitPoints Prepare(RPGLContext context, JsonArray originPoint) {
        base.Prepare(context, originPoint);
        RPGLObject source = GetSource();

        SetBase(source.GetHealthBase());

        new AddBonus().Execute(
            null,
            this,
            new JsonObject()
                /*{
                    "function": "add_bonus",
                    "bonus": [
                        {
                            "formula": "range",
                            "dice": [ ],
                            "bonus": <con modifier * level>
                        }
                    ]
                }*/
                .PutString("function", "add_bonus")
                .PutJsonArray("bonus", new JsonArray()
                    .AddJsonObject(new JsonObject()
                        .PutString("formula", "range")
                        .PutJsonArray("dice", new())
                        .PutLong("bonus", source.GetAbilityModifierFromAbilityName("con", context) * source.GetLevel())
                    )
                ),
            context,
            originPoint
        );

        return this;
    }

    public override CalculateMaximumHitPoints Run(RPGLContext context, JsonArray originPoint) {
        return this;
    }

    public override CalculateMaximumHitPoints SetOriginItem(string? originItem) {
        return (CalculateMaximumHitPoints) base.SetOriginItem(originItem);
    }

    public override CalculateMaximumHitPoints SetSource(RPGLObject source) {
        return (CalculateMaximumHitPoints) base.SetSource(source);
    }

    public override CalculateMaximumHitPoints SetTarget(RPGLObject target) {
        return (CalculateMaximumHitPoints) base.SetTarget(target);
    }

};
