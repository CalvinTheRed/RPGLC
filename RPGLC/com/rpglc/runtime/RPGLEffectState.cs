using com.rpglc.condition;
using com.rpglc.core;
using com.rpglc.function;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.runtime;

public class RPGLEffectState {

    public struct SubeventFilterBehavior {
        public Stack<ConditionState> conditions = [];
        public Stack<FunctionState> functions = [];

        public SubeventFilterBehavior() { }
    };

    public enum Phase {
        EvaluatingConditions,
        RequestingUserInput,
        ExecutingFunctions,
        Completed,
    }

    public RPGLEffect rpglEffect;
    public Phase phase = Phase.EvaluatingConditions;
    public Dictionary<string, List<SubeventFilterBehavior>> subeventFilters = [];
    public int subeventFilterIndex = 0;
    
    private bool? userInput = null;

    public RPGLEffectState(RPGLEffect rpglEffect) {
        this.rpglEffect = rpglEffect;
        Dictionary<string, object> subeventFilters = rpglEffect.GetSubeventFilters().AsDict();
        foreach (string key in subeventFilters.Keys) {
            List<SubeventFilterBehavior> subeventFilter = [];
            JsonArray behaviorArray = new(subeventFilters[key] as List<object>);
            for (int i = 0; i < behaviorArray.Count(); i++) {
                JsonObject behaviorJson = behaviorArray.GetJsonObject(i);
                SubeventFilterBehavior subeventFilterBehavior = new();
                JsonArray conditionArray = behaviorJson.GetJsonArray("conditions");
                for (int j = 0; j < conditionArray.Count(); j++) {
                    JsonObject conditionJson = conditionArray.GetJsonObject(j);
                    subeventFilterBehavior.conditions.Push(new(
                        Condition.Conditions[conditionJson.GetString("condition")].Clone(),
                        conditionJson
                    ));
                }
                JsonArray functionArray = behaviorJson.GetJsonArray("functions");
                for (int j = 0; j < functionArray.Count(); j++) {
                    JsonObject functionJson = functionArray.GetJsonObject(j);
                    subeventFilterBehavior.functions.Push(new(
                        Function.Functions[functionJson.GetString("function")].Clone(),
                        functionJson
                    ));
                }
                subeventFilter.Add(subeventFilterBehavior);
            }
            this.subeventFilters.Add(key, subeventFilter);
        }
    }

    public void SetUserInput(bool userInput) {
        this.userInput = userInput;
    }

    public (Subevent? subevent, bool canProceed) Advance(Subevent subevent, RPGLContext context) {
        switch (phase) {
            case Phase.EvaluatingConditions:
                return AdvanceConditions(subevent, context);
            case Phase.RequestingUserInput:
                return AdvanceUserInput(subevent, context);
            case Phase.ExecutingFunctions:
                return AdvanceFunctions(subevent, context);
            default:
                return (null, true);
        }
    }

    public (Subevent? subevent, bool canProceed) AdvanceConditions(Subevent subevent, RPGLContext context) {
        List<SubeventFilterBehavior>? subeventFilter = subeventFilters[subevent.GetSubeventId()];
        (ConditionState? condition, Subevent? subevent, bool completed) result = (null, null, true);
        if (subeventFilterIndex < (subeventFilter?.Count ?? 0)) {
            SubeventFilterBehavior subeventFilterBehavior = subeventFilter[subeventFilterIndex];
            ConditionState condition = subeventFilterBehavior.conditions.Peek();
            result = condition.Advance(this.rpglEffect, subevent, context);
            
            if (result.completed) {
                // pop completed condition from stack
                subeventFilterBehavior.conditions.Pop();

                if (condition.condition.evaluation) {
                    if (subeventFilterBehavior.conditions.Count == 0) {
                        // check for user input
                        this.phase = rpglEffect.GetOptional() ? Phase.RequestingUserInput : Phase.ExecutingFunctions;
                        return (null, !rpglEffect.GetOptional());
                    }
                } else {
                    // advance to next filter behavior due to failed condition
                    subeventFilterIndex++;
                }
            }
            
            if (result.condition is not null) {
                // prioritize dependency condition for next step if exists
                subeventFilterBehavior.conditions.Push(result.condition);
            }
            if (result.subevent is not null) {
                // TODO prepend to subevent stack here
            }
        } else {
            // failed to match the subevent to a filter
            this.phase = Phase.Completed;
        }

        // carry on with algorithm
        return (result.subevent, true);
    }

    public (Subevent? subevent, bool canProceed) AdvanceUserInput(Subevent subevent, RPGLContext context) {
        if (this.userInput is null) {
            return (null, false);
        } else if (this.userInput == true) {
            this.phase = Phase.ExecutingFunctions;
            return AdvanceFunctions(subevent, context);
        } else {
            this.phase = Phase.Completed;
            return (null, true);
        }
    }

    public (Subevent? subevent, bool canProceed) AdvanceFunctions(Subevent subevent, RPGLContext context) {
        List<SubeventFilterBehavior> subeventFilter = subeventFilters[subevent.GetSubeventId()];
        SubeventFilterBehavior subeventFilterBehavior = subeventFilter[subeventFilterIndex];
        if (subeventFilterBehavior.functions.Count > 0) {
            FunctionState function = subeventFilterBehavior.functions.Peek();
            var result = function.Advance(this.rpglEffect, subevent, context);

            if (result.isCompleted) {
                subeventFilterBehavior.functions.Pop();
            }

            return (result.subevent, true);
        } else {
            phase = Phase.Completed;
            return (null, true);
        }
    }

};
