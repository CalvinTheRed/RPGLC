using com.rpglc.condition;
using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.testutils.condition;

public class False : Condition {

    public False() : base("false") {
        conditionSteps.AddRange([
            (rpglEffect, subevent, conditionJson, context) => {
                this.evaluation = false;
                return new() {
                    conditionDependency = null,
                    subeventDependency = null,
                    stepCompleted = true,
                };
            },
        ]);
    }

    public override False Clone() {
        return new();
    }

    public override bool Run(RPGLEffect rpglEffect, Subevent subevent, JsonObject conditionJson, RPGLContext context, JsonArray originPoint) {
        return false;
    }

};
