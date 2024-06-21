using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

public class AddSubeventTag : Function {

    public AddSubeventTag() : base("add_subevent_tag") {

    }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        subevent.AddTag(functionJson.GetString("tag"));
    }

};
