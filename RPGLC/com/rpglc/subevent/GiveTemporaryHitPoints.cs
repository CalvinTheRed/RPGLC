using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

/// <summary>
///   Gives temporary hit points to an object and possibly assigns rider effects.
///   
///   <code>
///   {
///     "subevent": "give_temporary_hit_points",
///     "tags": [
///       &lt;string&gt;
///     ],
///     "temporary_hit_points": [
///       &lt;bonus formula&gt;
///     ],
///     "rider_effects": [
///       &lt;string&gt;
///     ]
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"tags" is an optional field and will default to a value of [ ] if left unspecified. Any tags provided will be inherited by any nested subevents.</item>
///     <item>"temporary_hit_points" is an optional field and it will default to a value of [ ] if left unspecified. This field indicates how many temporary hit points will be delivered, if any.</item>
///     <item>"rider_effects" is an optional field and it will default to a value of [ ] if left unspecified. This field indicates what rider effects, if any, to give to the target along with the temporary hit points.</item>
///   </list>
///   
/// </summary>
public class GiveTemporaryHitPoints : Subevent {

    public GiveTemporaryHitPoints() : base("give_temporary_hit_points") { }

    public override Subevent Clone() {
        Subevent clone = new GiveTemporaryHitPoints();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new GiveTemporaryHitPoints();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override GiveTemporaryHitPoints? Invoke(RPGLContext context, JsonArray originPoint) {
        return (GiveTemporaryHitPoints?) base.Invoke(context, originPoint);
    }

    public override GiveTemporaryHitPoints JoinSubeventData(JsonObject other) {
        return (GiveTemporaryHitPoints) base.JoinSubeventData(other);
    }

    public override GiveTemporaryHitPoints Prepare(RPGLContext context, JsonArray originPoint) {
        json.PutIfAbsent("temporary_hit_points", new JsonArray());
        json.PutIfAbsent("rider_effects", new JsonArray());
        GetBaseTemporaryHitPoints(context, originPoint);
        return this;
    }

    public override GiveTemporaryHitPoints Run(RPGLContext context, JsonArray originPoint) {
        GetTargetTemporaryHitPoints(context, originPoint);
        DeliverTemporaryHitPoints(context, originPoint);
        return this;
    }

    public override GiveTemporaryHitPoints SetOriginItem(string? originItem) {
        return (GiveTemporaryHitPoints) base.SetOriginItem(originItem);
    }

    public override GiveTemporaryHitPoints SetSource(RPGLObject source) {
        return (GiveTemporaryHitPoints) base.SetSource(source);
    }

    public override GiveTemporaryHitPoints SetTarget(RPGLObject target) {
        return (GiveTemporaryHitPoints) base.SetTarget(target);
    }

    private void GetBaseTemporaryHitPoints(RPGLContext context, JsonArray originPoint) {
        RPGLObject source = GetSource();
        
        TemporaryHitPointCollection baseTemporaryHitPointCollection = new TemporaryHitPointCollection()
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "temporary_hit_points": {{json.GetJsonArray("temporary_hit_points")}},
                    "tags": {{GetTags().DeepClone().AddString("base_temporary_hit_point_collection")}}
                }
                """))
            .SetOriginItem(GetOriginItem())
            .SetSource(source)
            .Prepare(context, originPoint)
            .SetTarget(source)
            .Invoke(context, originPoint);

        TemporaryHitPointRoll baseTemporaryHitPointRoll = new TemporaryHitPointRoll()
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "temporary_hit_points": {{baseTemporaryHitPointCollection.GetTemporaryHitPointCollection()}},
                    "tags": {{GetTags().DeepClone().AddString("base_temporary_hit_point_roll")}}
                }
                """))
            .SetOriginItem(GetOriginItem())
            .SetSource(source)
            .Prepare(context, originPoint)
            .SetTarget(source)
            .Invoke(context, originPoint);

        json.PutJsonArray("temporary_hit_points", baseTemporaryHitPointRoll.GetTemporaryHitPoints());
    }

    private void GetTargetTemporaryHitPoints(RPGLContext context, JsonArray originPoint) {
        RPGLObject source = GetSource();
        RPGLObject target = GetTarget();

        TemporaryHitPointCollection targetTemporaryHitPointCollection = new TemporaryHitPointCollection()
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "tags": {{GetTags().DeepClone().AddString("target_temporary_hit_point_collection")}}
                }
                """))
            .SetOriginItem(GetOriginItem())
            .SetSource(source)
            .Prepare(context, originPoint)
            .SetTarget(target)
            .Invoke(context, originPoint);

        TemporaryHitPointRoll targetTemporaryHitPointRoll = new TemporaryHitPointRoll()
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "temporary_hit_points": {{targetTemporaryHitPointCollection.GetTemporaryHitPointCollection()}},
                    "tags": {{GetTags().DeepClone().AddString("target_temporary_hit_point_roll")}}
                }
                """))
            .SetOriginItem(GetOriginItem())
            .SetSource(source)
            .Prepare(context, originPoint)
            .SetTarget(target)
            .Invoke(context, originPoint);

        json.GetJsonArray("temporary_hit_points").AsList().AddRange(targetTemporaryHitPointRoll.GetTemporaryHitPoints().AsList());
    }

    private void DeliverTemporaryHitPoints(RPGLContext context, JsonArray originPoint) {
        RPGLObject target = GetTarget();

        TemporaryHitPointDelivery temporaryHitPointDelivery = new TemporaryHitPointDelivery()
            .JoinSubeventData(new JsonObject().LoadFromString($$"""
                {
                    "temporary_hit_points": {{json.GetJsonArray("temporary_hit_points")}},
                    "tags": {{GetTags()}}
                }
                """))
            .SetOriginItem(GetOriginItem())
            .SetSource(GetSource())
            .Prepare(context, originPoint)
            .SetTarget(target)
            .Invoke(context, originPoint);

        target.ReceiveTemporaryHitPoints(temporaryHitPointDelivery, json.GetJsonArray("rider_effects"));
    }

};
