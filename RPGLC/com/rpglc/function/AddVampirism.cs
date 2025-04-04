using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

public class AddVampirism : Function {

    public AddVampirism() : base("add_vampirism") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is IVampiricSubevent vampiricSubevent) {
            JsonArray vampirismArray = functionJson.GetJsonArray("vampirism") ?? new JsonArray().AddJsonObject(new());
            if (vampirismArray.IsEmpty()) {
                vampirismArray.AddJsonObject(new());
            }
            for (int i = 0; i < vampirismArray.Count(); i++) {
                vampiricSubevent.AddVampirism(subevent, vampirismArray.GetJsonObject(i));
            }
        }
    }

};
