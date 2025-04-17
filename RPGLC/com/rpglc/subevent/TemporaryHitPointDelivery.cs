using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

/// <summary>
///   Delivers calculated temporary hit points to an object.
///   
///   <br /><br />
///   <i>This subevent is unavailable to be used directly inside an RPGLEvent.</i>
///   
///   <br /><br />
///   <b>Special Functions</b>
///   <list type="bullet">
///     <item>MaximizeTemporaryHitPoints</item>
///   </list>
///   
/// </summary>
public class TemporaryHitPointDelivery : Subevent {

    public TemporaryHitPointDelivery() : base("temporary_hit_point_delivery") { }

    public override Subevent Clone() {
        Subevent clone = new TemporaryHitPointDelivery();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new TemporaryHitPointDelivery();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override TemporaryHitPointDelivery? Invoke(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return (TemporaryHitPointDelivery?) base.Invoke(context, originPoint, invokingEffect);
    }

    public override TemporaryHitPointDelivery JoinSubeventData(JsonObject other) {
        return (TemporaryHitPointDelivery) base.JoinSubeventData(other);
    }

    public override TemporaryHitPointDelivery Prepare(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        json.PutIfAbsent("temporary_hit_points", new JsonArray());
        return this;
    }

    public override TemporaryHitPointDelivery Run(RPGLContext context, JsonArray originPoint, RPGLEffect? invokingEffect = null) {
        return this;
    }

    public override TemporaryHitPointDelivery SetOriginItem(string? originItem) {
        return (TemporaryHitPointDelivery) base.SetOriginItem(originItem);
    }

    public override TemporaryHitPointDelivery SetSource(RPGLObject source) {
        return (TemporaryHitPointDelivery) base.SetSource(source);
    }

    public override TemporaryHitPointDelivery SetTarget(RPGLObject target) {
        return (TemporaryHitPointDelivery) base.SetTarget(target);
    }

    public TemporaryHitPointDelivery MaximizeTemporaryHitPointDice() {
        JsonArray temporaryHitPointArray = json.GetJsonArray("temporary_hit_points");
        for (int i = 0; i < temporaryHitPointArray.Count(); i++) {
            JsonArray dice = temporaryHitPointArray.GetJsonObject(i).GetJsonArray("dice");
            for (int j = 0; j < dice.Count(); j++) {
                JsonObject die = dice.GetJsonObject(j);
                die.PutLong("roll", die.GetLong("size"));
            }
        }
        return this;
    }

    public long GetTemporaryHitPoints() {
        JsonArray temporaryHitPointArray = json.GetJsonArray("temporary_hit_points");
        long temporaryHitPoints = 0L;
        for (int i = 0; i < temporaryHitPointArray.Count(); i++) {
            JsonObject temporaryHitPointJson = temporaryHitPointArray.GetJsonObject(i);
            temporaryHitPoints += (long) temporaryHitPointJson.GetLong("bonus");
            JsonArray dice = temporaryHitPointJson.GetJsonArray("dice");
            for (int j = 0; j < dice.Count(); j++) {
                temporaryHitPoints += (long) dice.GetJsonObject(j).GetLong("roll");
            }
        }
        return temporaryHitPoints;
    }

};
