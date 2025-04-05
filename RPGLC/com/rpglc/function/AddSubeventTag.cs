using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

/// <summary>
///   Adds a tag to a subevent.
///   
///   <code>
///   {
///     "function": "add_subevent_tag",
///     "tag": &lt;string&gt;
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"tag" is a tag to give the subevent.</item>
///   </list>
///   
/// </summary>
public class AddSubeventTag : Function {

    public AddSubeventTag() : base("add_subevent_tag") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        subevent.AddTag(functionJson.GetString("tag"));
    }

};
