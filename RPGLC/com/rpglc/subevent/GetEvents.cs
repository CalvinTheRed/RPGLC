using com.rpglc.core;
using com.rpglc.json;

namespace com.rpglc.subevent;

/// <summary>
///   Collects events available to an object.
///   
///   <br /><br />
///   <i>This subevent is unavailable to be used directly inside an RPGLEvent.</i>
///   
///   <br /><br />
///   <b>Special Functions</b>
///   <list type="bullet">
///     <item>AddEvent</item>
///   </list>
///   
/// </summary>
public class GetEvents : Subevent {
    
    public GetEvents() : base("get_events") { }

    public override Subevent Clone() {
        Subevent clone = new GetEvents();
        clone.JoinSubeventData(json);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override Subevent Clone(JsonObject jsonData) {
        Subevent clone = new GetEvents();
        clone.JoinSubeventData(jsonData);
        clone.appliedEffects.AddRange(appliedEffects);
        return clone;
    }

    public override GetEvents? Invoke(RPGLContext context, JsonArray originPoint) {
        return (GetEvents?) base.Invoke(context, originPoint);
    }

    public override GetEvents JoinSubeventData(JsonObject other) {
        return (GetEvents) base.JoinSubeventData(other);
    }

    public override GetEvents Prepare(RPGLContext context, JsonArray originPoint) {
        json.PutIfAbsent("events", new JsonArray());
        return this;
    }

    public override GetEvents Run(RPGLContext context, JsonArray originPoint) {
        return this;
    }

    public override GetEvents SetOriginItem(string? originItem) {
        return (GetEvents) base.SetOriginItem(originItem);
    }

    public override GetEvents SetSource(RPGLObject source) {
        return (GetEvents) base.SetSource(source);
    }

    public override GetEvents SetTarget(RPGLObject target) {
        return (GetEvents) base.SetTarget(target);
    }

    public GetEvents AddEvent(string eventId) {
        json.GetJsonArray("events").AddString(eventId);
        return this;
    }

    public List<RPGLEvent> Events() {
        List<RPGLEvent> events = [];
        JsonArray eventIds = json.GetJsonArray("events");
        for (int i = 0; i < eventIds.Count(); i++) {
            events.Add(RPGLFactory.NewEvent(eventIds.GetString(i)));
        }
        return events;
    }
}
