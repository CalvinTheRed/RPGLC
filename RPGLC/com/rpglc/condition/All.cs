using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.condition;

/// <summary>
///   Returns true if all nested conditions return true. If a nested condition returns false, none of the following conditions will be evaluated.
///
///   <code>
///   {
///     "condition": "all",
///     "conditions": [
///       &lt;nested conditions here&gt;
///     ]
///   }
///   </code>
///
///   <list type="bullet">
///     <item>"conditions" is a list of 1 or more nested conditions.</item>
///   </list>
///   
/// </summary>
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
