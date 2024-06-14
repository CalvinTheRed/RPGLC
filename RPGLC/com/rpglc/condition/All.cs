using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.condition;

public class All : Condition {

    public All() : base("all") { }

    public override bool Run(RPGLEffect rpglEffect, Subevent subevent, JsonObject conditionJson, RPGLContext context, JsonArray originPoint) {
        JsonArray nestedConditions = conditionJson.GetJsonArray("conditions");
        if (nestedConditions.Count() > 0) {
            for (int i = 0; i < nestedConditions.Count(); i++) {
                JsonObject nestedConditionJson = nestedConditions.GetJsonObject(i);
                Condition nestedCondition = Conditions[nestedConditionJson.GetString("condition")];
                // once a single nested condition returns false, iteration can short-circuit
                if (!nestedCondition.Evaluate(rpglEffect, subevent, nestedConditionJson, context, originPoint)) {
                    return false;
                }
            }
            return true;
        }
        return false;
    }

};
