using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.condition;

public class CheckLevel : Condition {

    public CheckLevel() : base("check_level") { }

    public override bool Run(RPGLEffect rpglEffect, Subevent subevent, JsonObject conditionJson, RPGLContext context, JsonArray originPoint) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, subevent, conditionJson.GetJsonObject("object"));
        string classDatapackId = conditionJson.GetString("class");
        long level = classDatapackId is null
            ? rpglObject.GetLevel()
            : rpglObject.GetLevel(classDatapackId);
        
        return CompareValues(
            level,
            conditionJson.GetString("comparison"),
            (long) conditionJson.GetLong("compare_to")
        );
    }

};
