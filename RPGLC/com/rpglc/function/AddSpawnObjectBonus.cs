using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Grants a bonus to a spawned object.
///   
///   <code>
///   {
///     "function": "add_spawn_object_bonus",
///     "bonus": {
///       "field": &lt;string&gt;,
///       "bonus": &lt;long&gt;
///     }
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"bonus" is an object containing a specified field and a bonus to be applied to that field.</item>
///   </list>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>SpawnObject</item>
///   </list>
///   
/// </summary>
public class AddSpawnObjectBonus: Function {

    public AddSpawnObjectBonus() : base("add_spawn_object_bonus") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is SpawnObject spawnObject) {
            spawnObject.json.GetJsonArray("object_bonuses").AddJsonObject(functionJson.GetJsonObject("bonus"));
        }
    }

};
