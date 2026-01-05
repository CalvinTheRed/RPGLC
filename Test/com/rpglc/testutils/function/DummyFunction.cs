using com.rpglc.core;
using com.rpglc.function;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.testutils.function;

public class DummyFunction : Function {

    public static long Counter = 0L;

    public DummyFunction() : base("dummy_function") {
        functionSteps.AddRange([
            (rpglEffect, subevent, functionJson, context) => {
                Counter = 1;
                return new() {
                    dependency = null,
                    stepCompleted = true,
                };
            },
            (rpglEffect, subevent, functionJson, context) => {
                Counter = 2;
                return new() {
                    dependency = null,
                    stepCompleted = true,
                };
            },
            (rpglEffect, subevent, functionJson, context) => {
                Counter = 3;
                return new() {
                    dependency = null,
                    stepCompleted = true,
                };
            },
        ]);
    }

    public override void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        Counter++;
    }

    public static void ResetCounter() {
        Counter = 0L;
    }

};
