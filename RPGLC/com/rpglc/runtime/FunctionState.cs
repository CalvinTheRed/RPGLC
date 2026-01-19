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

    public readonly Function function;
    public readonly JsonObject functionJson;
    public int stepIndex = 0;

    public FunctionState(Function function, JsonObject functionJson) {
        this.function = function;
        this.functionJson = functionJson;
    }

    public (Subevent? subevent, bool isCompleted) Advance(RPGLEffect rpglEffect, Subevent subevent, RPGLContext context) {
        StateData response = function.functionSteps[stepIndex](rpglEffect, subevent, functionJson, context);
        if (response.stepCompleted) {
            stepIndex++;
        }
        
        return (response.dependency, stepIndex == function.functionSteps.Count);
    }

};
