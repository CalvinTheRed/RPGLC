using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Causes a GetEvents subevent to return an empty list of RPGLEvents.
///   
///   <code>
///   {
///     "function": "suppress_events"
///   }
///   </code>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>GetEvents</item>
///   </list>
///   
/// </summary>
public class SuppressEvents : Function {

    public SuppressEvents() : base("suppress_events") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is GetEvents getEvents) {
            getEvents.SuppressEvents();
        }
    }

};
