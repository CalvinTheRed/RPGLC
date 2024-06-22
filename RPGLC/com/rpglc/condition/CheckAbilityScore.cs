using com.rpglc.core;
using com.rpglc.json;
using com.rpglc.subevent;

namespace com.rpglc.condition;

public class CheckAbilityScore : Condition {

    public CheckAbilityScore() : base("check_ability_score") { }

    public override bool Run(RPGLEffect rpglEffect, Subevent subevent, JsonObject conditionJson, RPGLContext context, JsonArray originPoint) {
        RPGLObject rpglObject = RPGLEffect.GetObject(rpglEffect, subevent, conditionJson.GetJsonObject("object"));
        return CompareValues(
            rpglObject.GetAbilityScoreFromAbilityName(conditionJson.GetString("ability"), context),
            conditionJson.GetString("comparison"),
            (long) conditionJson.GetLong("compare_to")
        );
    }

};
