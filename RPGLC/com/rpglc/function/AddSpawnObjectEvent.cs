using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Grants an event to a spawned object.
///   
///   <code>
///   {
///     "function": "add_spawn_object_event",
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
///     <item>SpawnObject</item>
///   </list>
///   
/// </summary>
public class AddSpawnObjectEvent : Function {

    public AddSpawnObjectEvent() : base("add_spawn_object_event") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is SpawnObject spawnObject) {
            spawnObject.json.GetJsonArray("extra_events").AddString(functionJson.GetString("event"));
        }
    }

};
