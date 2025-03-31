using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

public class AddObjectTag : Function {

    public AddObjectTag() : base("add_object_tag") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is GetObjectTags getObjectTags) {
            getObjectTags.AddObjectTag(functionJson.GetString("tag"));
        }
    }

};
