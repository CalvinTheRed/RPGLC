using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Grants a tag to an object.
///   
///   <code>
///   {
///     "function": "add_object_tag",
///     "tag": &lt;string&gt;
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"tag" is a tag to grant to an object.</item>
///   </list>
///   
///   <b>Compatible Subevents</b>
///   <list type="bullet">
///     <item>GetObjectTags</item>
///   </list>
///   
/// </summary>
public class AddObjectTag : Function {

    public AddObjectTag() : base("add_object_tag") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is GetObjectTags getObjectTags) {
            getObjectTags.AddObjectTag(functionJson.GetString("tag"));
        }
    }

};
