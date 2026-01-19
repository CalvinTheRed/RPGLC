using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.condition;

/// <summary>
///   Returns true if the nested condition returns false.
///   
///   <code>
///   {
///     "condition": "invert",
///     "invert": &lt;nested condition&gt;
///   }
///   </code>
///   
///   <list type="bullet">
///     <item>"invert" is a nested condition.</item>
///   </list>
///   
/// </summary>
public class Invert : Condition {

    public Invert() : base("invert") {
        conditionSteps.AddRange([
            (rpglEffect, subevent, conditionJson, context) => {
                if (this.conditionDependency is null) {
                    JsonObject nestedConditionJson = conditionJson.GetJsonObject("invert");
                    this.conditionDependency = Conditions[nestedConditionJson.GetString("condition")].Clone();
                    return new() {
                        conditionDependency = new(this.conditionDependency, nestedConditionJson),
                        subeventDependency = null,
                        stepCompleted = false,
                    };
                } else {
                    this.evaluation = !this.conditionDependency.evaluation;
                    this.conditionDependency = null;
                    return new() {
                        conditionDependency = null,
                        subeventDependency = null,
                        stepCompleted = true,
                    };
                }
            },
        ]);
    }

    public override Invert Clone() {
        return new();
    }

    public override bool Run(RPGLEffect rpglEffect, Subevent subevent, JsonObject conditionJson, RPGLContext context, JsonArray originPoint) {
        JsonObject nestedConditionJson = conditionJson.GetJsonObject("invert");
        return !Conditions[nestedConditionJson.GetString("condition")]
            .Evaluate(rpglEffect, subevent, nestedConditionJson, context, originPoint);
    }

};
