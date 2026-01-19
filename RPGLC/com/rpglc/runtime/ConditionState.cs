using com.rpglc.condition;
using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.runtime;

public class ConditionState(Condition condition, JsonObject conditionJson) {

    public struct StateData {
        public ConditionState? conditionDependency;
        public Subevent? subeventDependency;
        public bool stepCompleted;
    };

    public readonly Condition condition = condition;
    public readonly JsonObject conditionJson = conditionJson;
    public int stepIndex = 0;

    public (ConditionState? condition, Subevent? subevent, bool completed) Advance(RPGLEffect rpglEffect, Subevent subevent, RPGLContext context) {
        StateData response = condition.conditionSteps[stepIndex](rpglEffect, subevent, conditionJson, context);
        if (response.stepCompleted) {
            stepIndex++;
        }

        return (response.conditionDependency, response.subeventDependency, stepIndex == condition.conditionSteps.Count);
    }

};
