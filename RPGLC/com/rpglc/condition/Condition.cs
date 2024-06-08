using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.condition;

public abstract class Condition(string conditionId) {

    public static readonly Dictionary<string, Condition> Conditions = [];
    private static readonly Stack<JsonObject> ActiveConditions = [];

    private static bool exitingConditionLoop = false;
    private static JsonObject? loopedConditionJson = null;

    private readonly string conditionId = conditionId;

    public static void Initialize(bool includeTestingConditions = false) {
        Conditions.Clear();

        Conditions.Add("any", new Any());
        Conditions.Add("all", new All());
        Conditions.Add("invert", new Invert());

        if (includeTestingConditions) {
            Conditions.Add("false", new False());
            Conditions.Add("true", new True());
        }
    }

    private bool VerifyCondition(JsonObject conditionJson) {
        return conditionId == conditionJson.GetString("condition");
    }

    public bool Evaluate(RPGLEffect rpglEffect, Subevent subevent, JsonObject conditionJson, RPGLContext context, JsonArray originPoint) {
        if (VerifyCondition(conditionJson)) {
            if (ActiveConditions.Contains(conditionJson)) {
                // begin back-out if you detect a loop
                exitingConditionLoop = true;
                loopedConditionJson = conditionJson;
                return false;
            } else {
                // else proceed as usual
                ActiveConditions.Push(conditionJson);
                bool result = Run(rpglEffect, subevent, conditionJson, context, originPoint);
                ActiveConditions.Pop();

                if (exitingConditionLoop) {
                    // back out and fail the condition if you are exiting a loop
                    if (Equals(loopedConditionJson, conditionJson)) {
                        // end the back-out if you have reached the start of the loop
                        exitingConditionLoop = false;
                        loopedConditionJson = null;
                    }
                    return false;
                }
                return result;
            }
        }
        return false;
    }

    public abstract bool Run(
        RPGLEffect rpglEffect,
        Subevent subevent,
        JsonObject conditionJson,
        RPGLContext context,
        JsonArray originPoint
    );

    public static bool CompareValues(double value, double target, string comparison) {
        return comparison switch {
            "=" => value == target,
            "!=" => value != target,
            ">" => value > target,
            "<" => value < target,
            ">=" => value >= target,
            "<=" => value <= target,
            _ => false,
        };
    }

};
