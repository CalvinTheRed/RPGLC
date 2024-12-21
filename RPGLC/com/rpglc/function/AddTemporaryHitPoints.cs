using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

public class AddTemporaryHitPoints : Function {

    public AddTemporaryHitPoints() : base("add_temporary_hit_points") { }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (subevent is TemporaryHitPointCollection temporaryHitPointCollection) {
            JsonArray temporaryHitPointArray = functionJson.GetJsonArray("temporary_hit_points");
            for (int i = 0; i < temporaryHitPointArray.Count(); i++) {
                temporaryHitPointCollection.AddTemporaryHitPoints(CalculationSubevent.ProcessBonusJson(
                    rpglEffect,
                    subevent,
                    temporaryHitPointArray.GetJsonObject(i),
                    context
                ));
            }
        }
    }

};
