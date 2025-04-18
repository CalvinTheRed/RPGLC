using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Grants an effect to a spawned object.
///   
///   <code>
///   {
///     "function": "add_spawn_object_effect",
///     "effect": &lt;string&gt;
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"effect" is an RPGLEffect datapack id.</item>
///   </list>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>SpawnObject</item>
///   </list>
///   
/// </summary>
public class AddSpawnObjectEffect: Function {

    public AddSpawnObjectEffect() : base("add_spawn_object_effect") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is SpawnObject spawnObject) {
            spawnObject.json.GetJsonArray("extra_effects").AddString(functionJson.GetString("effect"));
        }
    }

};
