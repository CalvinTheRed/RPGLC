using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Grants an event to an object.
///   
///   <code>
///   {
///     "function": "add_event",
///     "event": &lt;string&gt;
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"event" is an RPGLEvent datapack id.</item>
///   </list>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>GetEvents</item>
///   </list>
///   
/// </summary>
public class AddEvent : Function {

    public AddEvent() : base("add_event") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is GetEvents getEvents) {
            getEvents.AddEvent(functionJson.GetString("event"));
        }
    }

};
