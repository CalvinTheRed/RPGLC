using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.condition;

public class Any : Condition {

    public Any() : base("any") { }

    public override bool Run(RPGLEffect rpglEffect, Subevent subevent, JsonObject conditionJson, RPGLContext context, JsonArray originPoint) {
        JsonArray nestedConditions = conditionJson.GetJsonArray("conditions");
        for (int i = 0; i < nestedConditions.Count(); i++) {
            JsonObject nestedConditionJson = nestedConditions.GetJsonObject(i);
            Condition nestedCondition = Conditions[nestedConditionJson.GetString("condition")];
            // once a single nested condition returns true, iteration can short-circuit
            if (nestedCondition.Evaluate(rpglEffect, subevent, nestedConditionJson, context, originPoint)) {
                return true;
            }
        }
        return false;
    }

};
