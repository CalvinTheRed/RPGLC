using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Grants an event to a spawned object.
///   
///   <code>
///   {
///     "function": "add_spawn_object_tag",
///     "tag": &lt;string&gt;
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"tag" is a string representing an object tag.</item>
///   </list>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>SpawnObject</item>
///   </list>
///   
/// </summary>
public class AddSpawnObjectTag: Function {

    public AddSpawnObjectTag() : base("add_spawn_object_tag") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is SpawnObject spawnObject) {
            spawnObject.json.GetJsonArray("extra_tags").AddString(functionJson.GetString("tag"));
        }
    }

};
