using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

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
            AddTemporaryHitPoints(CalculationSubevent.ProcessBonusJson(
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
