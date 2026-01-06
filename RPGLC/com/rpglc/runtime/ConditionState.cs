using com.rpglc.condition;
using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.runtime;

public class ConditionState(RPGLEffect? rpglEffect, Subevent subevent, Condition condition, JsonObject conditionJson, RPGLContext context) {

    public struct StateData {
        public Condition? conditionDependency;
        public Subevent? subeventDependency;
        public bool stepCompleted;
    };

    private readonly RPGLEffect? rpglEffect = rpglEffect;
    private readonly Subevent subevent = subevent;
    private readonly Condition condition = condition;
    private readonly JsonObject conditionJson = conditionJson;
    private readonly RPGLContext context = context;
    private int stepIndex = 0;

    public (Condition? condition, Subevent? subevent) AdvanceState() {
        StateData response = condition.conditionSteps[stepIndex](rpglEffect, subevent, conditionJson, context);
        if (response.stepCompleted) {
            stepIndex++;
        }

        return (response.conditionDependency, response.subeventDependency);
    }

};
