using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

/// <summary>
///   Collects temporary hit points for a temporary hit point subevent.
///   
///   <br /><br />
///   <i>This subevent is unavailable to be used directly inside an RPGLEvent.</i>
///   
///   <br /><br />
///   <i>Note that all temporary hit point subevents will create two TemporaryHitPointCollection subevents. The first will have the "base" tag, and will represent temporary hit points that are applied to all targets of the temporary hit point subevent. The second will have the "target" tag, and will represent temporary hit points that are only applied to a specific target of the temporary hit point subevent.</i>
///   
///   <br /><br />
///   <b>Special Functions</b>
///   <list type="bullet">
///     <item>AddTemporaryHitPoints</item>
///   </list>
///   
/// </summary>
public class TemporaryHitPointCollection : Subevent {

    public TemporaryHitPointCollection() : base("temporary_hit_point_collection") { }

    public override Subevent Clone() {
        Subevent clone = new TemporaryHitPointCollection();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new TemporaryHitPointCollection();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override TemporaryHitPointCollection? Invoke(RPGLContext context, JsonArray originPoint) {
        return (TemporaryHitPointCollection?) base.Invoke(context, originPoint);
    }

    public override TemporaryHitPointCollection JoinSubeventData(JsonObject other) {
        return (TemporaryHitPointCollection) base.JoinSubeventData(other);
    }

    public override TemporaryHitPointCollection Prepare(RPGLContext context, JsonArray originPoint) {
        json.PutIfAbsent("temporary_hit_points", new JsonArray());
        PrepareTemporaryHitPoints(context);
        return this;
    }

    public override TemporaryHitPointCollection Run(RPGLContext context, JsonArray originPoint) {
        return this;
    }

    public override TemporaryHitPointCollection SetOriginItem(string? originItem) {
        return (TemporaryHitPointCollection) base.SetOriginItem(originItem);
    }

    public override TemporaryHitPointCollection SetSource(RPGLObject source) {
        return (TemporaryHitPointCollection) base.SetSource(source);
    }

    public override TemporaryHitPointCollection SetTarget(RPGLObject target) {
        return (TemporaryHitPointCollection) base.SetTarget(target);
    }

    private void PrepareTemporaryHitPoints(RPGLContext context) {
        JsonArray temporaryHitPointArray = json.RemoveJsonArray("temporary_hit_points");
        json.PutJsonArray("temporary_hit_points", new());

        RPGLEffect rpglEffect = new();
        rpglEffect.SetSource(GetSource().GetUuid());
        rpglEffect.SetTarget(null);
        for (int i = 0; i < temporaryHitPointArray.Count(); i++) {
            AddTemporaryHitPoints(CalculationSubevent.SimplifyCalculationFormula(
                rpglEffect,
                this,
                temporaryHitPointArray.GetJsonObject(i),
                context
            ));
        }
    }

    public TemporaryHitPointCollection AddTemporaryHitPoints(JsonObject temporaryHitPointJson) {
        GetTemporaryHitPointCollection().AddJsonObject(temporaryHitPointJson);
        return this;
    }

    public JsonArray GetTemporaryHitPointCollection() {
        return json.GetJsonArray("temporary_hit_points");
    }

};
