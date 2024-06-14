using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

public class DummyFunction : Function {

    public static long Counter = 0L;

    public DummyFunction() : base("dummy_function") { }

    public override void Run(RPGLEffect rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        Counter++;
    }

    public static void ResetCounter() {
        Counter = 0L;
    }

};
