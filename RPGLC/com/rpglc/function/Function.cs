using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

public abstract class Function(string functionId) {

    public static readonly Dictionary<string, Function> Functions = [];

    public readonly string functionId = functionId;

    public static void Initialize(bool includeTestingFunctions = false) {
        Functions.Clear();

        Functions.Add("add_bonus", new AddBonus());
        Functions.Add("add_damage", new AddDamage());

        if (includeTestingFunctions) {
            Functions.Add("dummy_function", new DummyFunction());
        }
    }

    private bool VerifyFunction(JsonObject functionJson) {
        return functionId == functionJson.GetString("function");
    }

    public void Execute(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint) {
        if (VerifyFunction(functionJson)) {
            Run(rpglEffect, subevent, functionJson, context, originPoint);
        }
    }

    public abstract void Run(RPGLEffect? rpglEffect, Subevent subevent, JsonObject functionJson, RPGLContext context, JsonArray originPoint);

};
