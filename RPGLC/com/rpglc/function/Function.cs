using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.function;

public abstract class Function(string functionId) {

    public static readonly Dictionary<string, Function> Functions = [];

    public readonly string functionId = functionId;

    public static void Initialize(bool includeTestingFunctions = false) {
        Functions.Clear();

        Initialize([
            new AddBonus(),
            new AddDamage(),
            new AddHealing(),
            new AddSubeventTag(),
            new AddVampirism(),
            new GrantAdvantage(),
            new GrantDisadvantage(),
            new GrantImmunity(),
            new GrantResistance(),
            new GrantVulnerability(),
            new MaximizeDamage(),
            new RevokeImmunity(),
            new RevokeResistance(),
            new RevokeVulnerability(),
        ]);

        if (includeTestingFunctions) {
            Initialize([
                new DummyFunction(),
            ]);
        }
    }

    private static void Initialize(List<Function> functions) {
        foreach (Function function in functions) {
            Functions.Add(function.functionId, function);
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
