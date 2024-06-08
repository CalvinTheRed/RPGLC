using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.condition;

public class Invert : Condition {

    public Invert() : base("invert") { }

    public override bool Run(RPGLEffect rpglEffect, Subevent subevent, JsonObject conditionJson, RPGLContext context, JsonArray originPoint) {
        JsonObject nestedConditionJson = conditionJson.GetJsonObject("invert");
        return !Conditions[nestedConditionJson.GetString("condition")]
            .Evaluate(rpglEffect, subevent, nestedConditionJson, context, originPoint);
    }

};
