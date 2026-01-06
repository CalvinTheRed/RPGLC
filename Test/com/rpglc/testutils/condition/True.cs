using com.rpglc.condition;
using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.testutils.condition;

public class True : Condition {

    public True() : base("true") {
        conditionSteps.AddRange([
            (rpglEffect, subevent, conditionJson, context) => {
                return new() {
                    conditionDependency = null,
                    subeventDependency = null,
                    stepCompleted = true,
                };
            },
        ]);
    }

    public override True Clone() {
        return new();
    }

    public override bool Run(RPGLEffect rpglEffect, Subevent subevent, JsonObject conditionJson, RPGLContext context, JsonArray originPoint) {
        return true;
    }

};
