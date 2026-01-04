using com.rpglc.core;
using com.rpglc.function;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.runtime;

public class FunctionState {

    public struct StateData {
        public Subevent? dependency;
        public bool stepCompleted;
    };

    private readonly RPGLEffect? rpglEffect;
    private readonly Subevent subevent;
    private readonly Function function;
    private readonly JsonObject functionJson;
    private readonly RPGLContext context;
    private int stepIndex = 0;

    public FunctionState(RPGLEffect? rpglEffect, Subevent subevent, Function function, JsonObject functionJson, RPGLContext context) {
        this.rpglEffect = rpglEffect;
        this.subevent = subevent;
        this.function = function;
        this.functionJson = functionJson;
        this.context = context;
    }

    public Subevent? AdvanceState() {
        StateData response = function.functionSteps[stepIndex](rpglEffect, subevent, functionJson, context);
        if (response.stepCompleted) {
            stepIndex++;
        }
        
        return response.dependency;
    }

};
