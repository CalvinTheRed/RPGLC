using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

public class AddEvent : Function {

    public AddEvent() : base("add_event") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is GetEvents getEvents) {
            getEvents.AddEvent(functionJson.GetString("event"));
        }
    }

};
