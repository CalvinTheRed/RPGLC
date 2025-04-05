using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.condition;

/// <summary>
///   Returns true if at least one nested condition returns true. If a nested condition returns true, none of the following conditions will be evaluated.
///
///   <code>
///   {
///     "condition": "any",
///     "conditions": [
///       &lt;nested conditions here&gt;
///     ]
///   }
///   </code>
///   <list type="bullet">
///     <item>"conditions" is a list of 1 or more nested conditions.</item>
///   </list>
///   
/// </summary>
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
